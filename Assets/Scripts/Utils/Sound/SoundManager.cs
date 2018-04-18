using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
#if UNITY_EDITOR

using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Audio;

namespace Util.Sound
{
    /// <summary>
    /// サウンドを管理する
    /// </summary>
    public class SoundManager : SingletonMonoBehaviour<SoundManager>
    {
        // デシベル関連の定数
        private const float MAX_DECIBEL = 0f;
        private const float MIN_DECIBEL = -80f;

        // 使用するオーディオミキサー
        [SerializeField]
        private AudioMixer _mixer = null;

        // 音の名前リスト
        [SerializeField]
        private List<string> _audioNames = new List<string>();

        // オーディオソースのリスト
        [SerializeField]
        private List<AudioSource> _audioDatas = new List<AudioSource>();

        // 音の設定関連
        private SoundSetting _soundSetting = new SoundSetting();

        /// <summary>
        /// SEのボリューム
        /// </summary>
        public float SEVolum
        {
            get
            { return _soundSetting.SEVolume; }
            set
            {
                _mixer.SetFloat("SEVolume", GetDecibelConversion(value));
                _soundSetting.SEVolume = value;
            }
        }

        /// <summary>
        /// BGMのボリューム
        /// </summary>
        public float BGMVolum
        {
            get { return _soundSetting.BGMVolume; }
            set
            {
                _mixer.SetFloat("BGMVolume", GetDecibelConversion(value));
                _soundSetting.BGMVolume = value;
            }
        }

        /// <summary>
        /// 初期化
        /// </summary>
        private void Start()
        {
            // 音量の初期設定
            _soundSetting.SEVolume = 1f;
            _soundSetting.BGMVolume = 1f;
            SEVolum = _soundSetting.SEVolume;
            BGMVolum = _soundSetting.BGMVolume;
        }

        /// <summary>
        /// サウンドを再生する
        /// </summary>
        /// <param name="key">サウンドのキー</param>
        public void Play(AudioKey key)
        {
            _audioDatas[(int)key].Play();
        }
        /// <param name="key">サウンドのキー</param>
        public void Play(string key)
        {
            Play(ConvertAudioKey(key));
        }

        /// <summary>
        /// サウンドを再生する
        /// 同一キーのサウンドを同時に複数鳴らすことが可能
        /// </summary>
        /// <param name="key">サウンドのキー</param>
        public void PlayOneShot(AudioKey key)
        {
            _audioDatas[(int)key].PlayOneShot(_audioDatas[(int)key].clip);
        }
        /// <param name="key">サウンドのキー</param>
        public void PlayOneShot(string key)
        {
            PlayOneShot(ConvertAudioKey(key));
        }

        /// <summary>
        /// 再生を停止する
        /// </summary>
        /// <param name="key">サウンドのキー</param>
        public void Stop(AudioKey key)
        {
            _audioDatas[(int)key].Stop();
        }
        /// <param name="key">サウンドのキー</param>
        public void Stop(string key)
        {
            Stop(ConvertAudioKey(key));
        }

        /// <summary>
        /// 再生中か？
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool IsPlaying(AudioKey key)
        {
            return _audioDatas[(int)key].isPlaying;
        }
        /// <summary>
        /// 設定ファイルを読み込む
        /// </summary>
        public void LoadSettingFile()
        {
            _soundSetting.LoadSettingFile();
        }

        /// <summary>
        /// 設定ファイルを保存する
        /// </summary>
        public void SaveSettingFile()
        {
            _soundSetting.SaveSettingFile();
        }

        /// <summary>
        /// 文字列をAudioKeyに変換する
        /// </summary>
        /// <param name="key">AudioKeyに変換する文字列</param>
        /// <returns>該当するAudioKey</returns>
        private AudioKey ConvertAudioKey(string key)
        {
            var keyIndex = _audioNames.IndexOf(key);
            if (keyIndex < 0)
                throw new Exception(key + "のAudioKeyへの変換に失敗しました");
            return (AudioKey)keyIndex;
        }

        /// <summary>
        /// デシベル変換
        /// </summary>
        private float DecibelConversion(float volume)
        {
            return 20f * Mathf.Log10(volume);
        }

        /// <summary>
        /// デシベル変換後の値を得る
        /// 変換後の値は適切な値を取るようClampされる
        /// </summary>
        private float GetDecibelConversion(float value)
        {
            return Mathf.Clamp(DecibelConversion(value), MIN_DECIBEL, MAX_DECIBEL);
        }

        #region CustomInspector

        // エディタの表示
#if UNITY_EDITOR

        /// <summary>
        /// SoundManagerのEditor拡張
        /// </summary>
        [CustomEditor(typeof(SoundManager))]
        private class SoundManagetInspector : Editor
        {
            // 実体
            private SoundManager soundManager;
            // 選択しているインデクス
            private int selectAudioSourceIndex;
            // 新たに作るオーディオの名前
            private string newAudioDataName = string.Empty;
            private List<bool> foldSoundDatas = new List<bool>();

            public override void OnInspectorGUI()
            {
                soundManager = target as SoundManager;
                soundManager._mixer = EditorGUILayout.ObjectField("AudioMixer", soundManager._mixer, typeof(AudioMixer), false) as AudioMixer;

                EditorGUILayout.Separator();

                #region AudioData

                for (int i = 0; i < soundManager._audioDatas.Count; ++i)
                {
                    if (foldSoundDatas.Count < i + 1)
                        foldSoundDatas.Add(false);

                    if (foldSoundDatas[i] = EditorGUILayout.Foldout(foldSoundDatas[i], soundManager._audioNames[i]))
                    {
                        EditorGUILayout.BeginVertical("box");
                        EditorGUILayout.BeginHorizontal();
                        //再生ボタン表示
                        if (soundManager._audioDatas[i] != null && soundManager._audioDatas[i].isPlaying)
                        {
                            if (GUILayout.Button("■"))
                            {
                                soundManager._audioDatas[i].Stop();
                            }
                        }
                        else if (GUILayout.Button("▶"))
                        {
                            soundManager._audioDatas[i].Play();
                        }
                        EditorGUILayout.EndHorizontal();
                        // UIの表示たち
                        soundManager._audioDatas[i].clip = EditorGUILayout.ObjectField("AudioClip", soundManager._audioDatas[i].clip, typeof(AudioClip), false) as AudioClip;
                        soundManager._audioDatas[i].loop = EditorGUILayout.Toggle("Loop", soundManager._audioDatas[i].loop);
                        soundManager._audioDatas[i].playOnAwake = EditorGUILayout.Toggle("PlayOnAwake", soundManager._audioDatas[i].playOnAwake);
                        soundManager._audioDatas[i].volume = EditorGUILayout.Slider("Volume", soundManager._audioDatas[i].volume, 0, 1);
                        soundManager._audioDatas[i].pitch = EditorGUILayout.Slider("Pitch", soundManager._audioDatas[i].pitch, -3, 3);
                        soundManager._audioDatas[i].bypassEffects = EditorGUILayout.Toggle("BypassEffects", soundManager._audioDatas[i].bypassEffects);
                        soundManager._audioDatas[i].bypassListenerEffects = EditorGUILayout.Toggle("BypassListenerEffects", soundManager._audioDatas[i].bypassListenerEffects);
                        soundManager._audioDatas[i].bypassReverbZones = EditorGUILayout.Toggle("BypassReverbZone", soundManager._audioDatas[i].bypassReverbZones);
                        soundManager._audioDatas[i].reverbZoneMix = EditorGUILayout.Slider("ReverbZoneMix", soundManager._audioDatas[i].reverbZoneMix, 0, 1.1f);
                        soundManager._audioDatas[i].spatialBlend = EditorGUILayout.Slider("SpatialBlend", soundManager._audioDatas[i].spatialBlend, 0, 1);
                        soundManager._audioDatas[i].mute = EditorGUILayout.Toggle("Mute", soundManager._audioDatas[i].mute);
                        soundManager._audioDatas[i].outputAudioMixerGroup = EditorGUILayout.ObjectField("OutPut", soundManager._audioDatas[i].outputAudioMixerGroup, typeof(AudioMixerGroup), false) as AudioMixerGroup;
                        if (soundManager._audioDatas[i].outputAudioMixerGroup == null)
                        {
                            EditorGUILayout.HelpBox("Outputが設定されていないオーディオは音量の一括設定等が適用できません", MessageType.Error);
                        }
                        EditorGUILayout.EndVertical();
                    }
                }

                #endregion AudioData

                #region Add/Remove Button

                EditorGUILayout.BeginHorizontal();
                newAudioDataName = EditorGUILayout.TextField(newAudioDataName);
                if (GUILayout.Button("追加"))
                {
                    if (soundManager._audioNames.Contains(newAudioDataName) || string.IsNullOrEmpty(newAudioDataName))
                    {
                        GUIContent content = new GUIContent("既にKeyが存在するか、Keyが空です");
                        EditorWindow.focusedWindow.ShowNotification(content);
                    }
                    else
                    {
                        soundManager._audioNames.Add(newAudioDataName);
                        var newAudioSource = soundManager.gameObject.AddComponent<AudioSource>();
                        newAudioSource.hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector;
                        soundManager._audioDatas.Add(newAudioSource);
                    }
                    newAudioDataName = string.Empty;
                }
                EditorGUILayout.Space();
                selectAudioSourceIndex = EditorGUILayout.Popup(selectAudioSourceIndex, soundManager._audioNames.ToArray());
                if (GUILayout.Button("削除"))
                {
                    if (soundManager._audioNames.Count > selectAudioSourceIndex)
                    {
                        DestroyImmediate(soundManager._audioDatas[selectAudioSourceIndex]);
                        soundManager._audioNames.RemoveAt(selectAudioSourceIndex);
                        soundManager._audioDatas.RemoveAt(selectAudioSourceIndex);
                        foldSoundDatas.RemoveAt(selectAudioSourceIndex);
                    }
                }
                EditorGUILayout.EndHorizontal();

                DebugEditorScript();

                #endregion Add/Remove Button

                EditorGUILayout.Space();

                #region CreateScriptFile

                // csに保存
                if (GUILayout.Button("適用"))
                {
                    var gEnum = new Util.DynamicEnum("AudioKey");
                    if (gEnum.Save(soundManager._audioNames))
                    {
                        AssetDatabase.Refresh();
                    }
                }

                #endregion CreateScriptFile

                EditorUtility.SetDirty(soundManager);
            }

            /// <summary>
            /// エディタスクリプトデバッグ用機能
            /// </summary>
            [System.Diagnostics.Conditional("DEBUG")]
            private void DebugEditorScript()
            {
                EditorGUILayout.Space();
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("AudioSoueceの表示"))
                {
                    var allAudioSource = soundManager.GetComponents<AudioSource>();
                    foreach (var data in allAudioSource)
                        data.hideFlags = HideFlags.None | HideFlags.NotEditable;
                }
                if (GUILayout.Button("AudioSourceの非表示"))
                {
                    var allAudioSource = soundManager.GetComponents<AudioSource>();
                    foreach (var data in allAudioSource)
                        data.hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector | HideFlags.NotEditable;
                }
                if (GUILayout.Button("AudioSourceのクリア"))
                {
                    var allAudioSource = soundManager.GetComponents<AudioSource>();
                    //非表示中は削除できない
                    if (allAudioSource.Any(s => (s.hideFlags & HideFlags.HideInInspector) != HideFlags.HideInInspector))
                    {
                        GUIContent content = new GUIContent("AudioSourceを非表示にしてから実行してください");
                        EditorWindow.focusedWindow.ShowNotification(content);
                    }
                    else
                    {
                        for (int i = 0; i < allAudioSource.Count(); ++i)
                            DestroyImmediate(allAudioSource[i]);
                        soundManager._audioDatas.Clear();
                        soundManager._audioNames.Clear();
                        foldSoundDatas.Clear();
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
        }
    }
#endif
    #endregion CustomInspector
}