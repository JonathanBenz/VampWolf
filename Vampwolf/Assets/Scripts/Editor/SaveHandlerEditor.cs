using UnityEditor;
using UnityEngine;
using Vampwolf.Persistence;

namespace Vampwolf.Editors
{
    [CustomEditor(typeof(SaveHandler), true)]
    public class CollectibleEditor : Editor
    {
        SaveHandler saveHandler;

        private void OnEnable()
        {
            saveHandler = (SaveHandler)target;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("Generate New SerializableGuid"))
            {
                saveHandler.ID = SerializableGuid.NewGuid();
                EditorUtility.SetDirty(saveHandler);
            }
        }
    }
}
