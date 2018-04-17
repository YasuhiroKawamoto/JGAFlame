using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(Transform))]
public class TransformInspector : Editor
{
    enum TargetType
    {
        Position,
        Rotation,
        Scale
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        var transform = target as Transform;

        DrawLine("P", TargetType.Position, transform);
        DrawLine("R", TargetType.Rotation, transform);
        DrawLine("S", TargetType.Scale, transform);

        serializedObject.ApplyModifiedProperties();
    }

    // 要素の描画
    void DrawLine(string label, TargetType type, Transform transform)
    {
        Vector3 newValue = Vector3.zero;
        bool reset = false;

        EditorGUI.BeginChangeCheck();

        // Property
        using (new EditorGUILayout.HorizontalScope())
        {
            if (GUILayout.Button(label, GUILayout.Width(20)))
            {
                newValue = type == TargetType.Scale ? Vector3.one : Vector3.zero;
                reset = true;
            }
            // 変更がある場合
            if (!reset)
            {
                switch (type)
                {
                    case TargetType.Position:
                        newValue = EditorGUILayout.Vector3Field("", transform.position, GUILayout.Height(16));
                        break;
                    case TargetType.Rotation:
                        newValue = EditorGUILayout.Vector3Field("", transform.localEulerAngles, GUILayout.Height(16));
                        break;
                    case TargetType.Scale:
                        newValue = EditorGUILayout.Vector3Field("", transform.localScale, GUILayout.Height(16));
                        break;
                }
            }
        }

        // 値を変更
        if (EditorGUI.EndChangeCheck() || reset)
        {
            Undo.RecordObjects(targets, string.Format("{0} {1} {2}", (reset ? "Reset" : "Change"), transform.gameObject.name, type.ToString()));

            var t = targets[0] as Transform;
            switch (type)
            {
                case TargetType.Position:
                    t.position = newValue;
                    break;
                case TargetType.Rotation:
                    t.localEulerAngles = newValue;
                    break;
                case TargetType.Scale:
                    t.localScale = newValue;
                    break;
            }
            EditorUtility.SetDirty(targets[0]);
        }
    }
}

#endif
