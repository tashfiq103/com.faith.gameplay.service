
namespace com.faith.gameplay_service {

    using UnityEngine;
    using UnityEditor;

    [CustomEditor(typeof(DevicePositionOptimizationAgent))]

    public class DevicePositionOptimizationAgentEditor : Editor
    {

        private DevicePositionOptimizationAgent Reference;

        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// </summary>
        void OnEnable()
        {
            Reference = (DevicePositionOptimizationAgent)target;
        }

        public override void OnInspectorGUI()
        {

            serializedObject.Update();

            Reference.SetPositionOfAllObject();

            DrawDefaultInspector();

            serializedObject.ApplyModifiedPropertiesWithoutUndo();
        }
    }
}


