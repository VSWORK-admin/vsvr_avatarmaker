// Magica Cloth.
// Copyright (c) MagicaSoft, 2020.
// https://magicasoft.jp
using UnityEditor;

namespace MagicaCloth
{
    /// <summary>
    /// 物理マネージャのエディタ拡張
    /// </summary>
    [CustomEditor(typeof(MagicaPhysicsManager))]
    public class MagicaPhysicsManagerInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            //DrawDefaultInspector();

            MagicaPhysicsManager scr = target as MagicaPhysicsManager;

            serializedObject.Update();
            Undo.RecordObject(scr, "PhysicsManager");

            //Help();

            MainInspector();

            serializedObject.ApplyModifiedProperties();
        }

        void Help()
        {
            MagicaPhysicsManager scr = target as MagicaPhysicsManager;

            if (scr.UpdateMode == UpdateTimeManager.UpdateMode.OncePerFrame)
            {
                EditorGUILayout.HelpBox("[OncePerFrame] will be discontinued in the future.", MessageType.Warning);
            }
            else if (scr.UpdateTime.IsDelay)
            {
                EditorGUILayout.HelpBox(
                    "Delayed execution. [experimental]\n" +
                    "Improve performance by running simulations during rendering.\n" +
                    "Note, however, that the result is one frame late.\n" +
                    "This delay is covered by future predictions.",
                    MessageType.Info);
            }
        }

        void MainInspector()
        {
            MagicaPhysicsManager scr = target as MagicaPhysicsManager;

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Update", EditorStyles.boldLabel);
            using (new EditorGUI.IndentLevelScope())
            {
                var prop = serializedObject.FindProperty("updateTime.updatePerSeccond");
                EditorGUILayout.PropertyField(prop);

                var prop2 = serializedObject.FindProperty("updateTime.updateMode");
                EditorGUILayout.PropertyField(prop2);

                // 以下は遅延実行時のみ
                if (scr.UpdateTime.IsDelay)
                {
                    var prop3 = serializedObject.FindProperty("updateTime.futurePredictionRate");
                    EditorGUILayout.PropertyField(prop3);
                }

                Help();
            }
        }
    }
}