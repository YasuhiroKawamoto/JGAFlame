using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
#if UNITY_EDITOR

using UnityEditor;
#endif

namespace Util.Sound
{
    /// <summary>
    /// サウンド設定の情報を取り扱うクラス
    /// </summary>
    public class SoundSetting
    {
        private const string SE = "SE";
        private const string BGM = "BGM";

        private Dictionary<string, double> data = new Dictionary<string, double>();

        /// <summary>
        /// SEのVolumeへのアクセサー
        /// </summary>
        public float SEVolume
        {
            get { return (float)data[SE]; }
            set { data[SE] = value; }
        }

        /// <summary>
        /// BGMのVolumeへのアクセサー
        /// </summary>
        public float BGMVolume
        {
            get { return (float)data[BGM]; }
            set { data[BGM] = value; }
        }

        /// <summary>
        /// サウンド設定を扱うクラスを生成する
        /// </summary>
        public SoundSetting()
        {
            data.Add(SE, 0);
            data.Add(BGM, 0);
        }

        /// <summary>
        /// TODO: 設定を読み込む
        /// </summary>
        public void LoadSettingFile()
        {

        }

        /// <summary>
        /// TODO: 設定を保存する
        /// </summary>
        public void SaveSettingFile()
        {

        }
    }
}