namespace com.faith.gameplay.service
{
    using UnityEngine;
    using UnityEditor;

    [CustomEditor(typeof(UIMonetizationController))]
    public class UIMonetizationControllerEditor : Editor
    {
        private UIMonetizationController Reference;

        private void OnEnable()
        {
            Reference = (UIMonetizationController)target;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            CustomGUI();

            DrawDefaultInspector();

            serializedObject.ApplyModifiedProperties();
        }

        private void CustomGUI()
        {

            EditorGUILayout.Space();


            EditorGUILayout.BeginHorizontal();
            {

                EditorGUILayout.LabelField(
                        "Monetization ("
                        + (PlayerPrefs.GetInt("TRACKER_FOR_ACTIVE_" + Reference.trackerName + "_MONETIZATION", 0) == 0 ? "Disabled" : "Enabled")
                        + ") : RemainingTime = "
                        + PlayerPrefs.GetInt("TRACKER_FOR_REMAINING_TIME_" + Reference.trackerName, 0)
                        );

                if (GUILayout.Button("Reset Monetization"))
                {

                    PlayerPrefs.SetInt("TRACKER_FOR_ACTIVE_" + Reference.trackerName + "_MONETIZATION", 0);
                }

                if (GUILayout.Button("Reset Timer"))
                {
                    PlayerPrefs.SetInt("TRACKER_FOR_REMAINING_TIME_" + Reference.trackerName, Reference.durationForMonetization);
                }

            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();

        }
    }
}


