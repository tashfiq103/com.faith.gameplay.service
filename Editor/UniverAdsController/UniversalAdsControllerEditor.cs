using System.Collections;
using System.Collections.Generic;
namespace com.faith.GameplayService
{
    using UnityEngine;
    using UnityEditor;

    [CustomEditor(typeof(UniversalAdsController))]
    public class UniversalAdsControllerEditor : Editor
    {

        private UniversalAdsController Reference;

        private void OnEnable()
        {

            Reference = (UniversalAdsController)target;
        }

        public override void OnInspectorGUI()
        {

            serializedObject.Update();

            EditorGUILayout.BeginHorizontal();
            {

                if (GUILayout.Button("Show InterstetialAd"))
                {
                    Reference.ShowInterstetialAd();
                }

                if (GUILayout.Button("Show RewardedVideoAd"))
                {
                    Reference.ShowRewardVideoAd();
                }
            }
            EditorGUILayout.EndHorizontal();

            DrawDefaultInspector();

            serializedObject.ApplyModifiedProperties();
        }
    }
}


