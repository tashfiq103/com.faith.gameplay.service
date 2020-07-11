namespace com.faith.GameplayService
{
    using UnityEditor;
    using UnityEngine;

    [CustomEditor(typeof(DeviceInfoManager))]
    public class DeviceInfoManagerEditor : Editor
    {

        private DeviceInfoManager Reference;

        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// </summary>
        void OnEnable()
        {
            Reference = (DeviceInfoManager)target;
            if (DeviceInfoManager.Instance == null)
            {
                DeviceInfoManager.Instance = Reference;
            }
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.Space();
            if (GUILayout.Button(PlayerPrefs.GetInt(Reference.HAS_RESET_PLAYERPREF + Application.version, 0) == 0 ? "Reset PlayerPrefs" : "PlayerPref Already Reseted"))
            {

                PlayerPrefs.SetInt(Reference.HAS_RESET_PLAYERPREF + Application.version, 0);
            }
            EditorGUILayout.Space();

            DrawDefaultInspector();

            serializedObject.ApplyModifiedProperties();
        }
    }
}


