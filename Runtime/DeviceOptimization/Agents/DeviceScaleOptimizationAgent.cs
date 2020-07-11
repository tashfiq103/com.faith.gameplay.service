namespace com.faith.gameplay_service {

    using UnityEngine;

    public class DeviceScaleOptimizationAgent : MonoBehaviour
    {

        [System.Serializable]
        public struct ScalableObject
        {
            [Range(0.0f, 1.0f)]
            public float scaleFactor;
            public Transform reference;
        }

        public ScalableObject[] scalableObject;

        void Start()
        {

            OptimizeObjectScaling();
        }

        #region Configuretion		:		Class

        public void OptimizeObjectScaling()
        {

            if (scalableObject != null)
            {

                for (int index = 0; index < scalableObject.Length; index++)
                {

                    if (scalableObject[index].reference != null)
                    {

                        if (scalableObject[index].reference.GetComponent<RectTransform>())
                        {
                            scalableObject[index].reference.GetComponent<RectTransform>().sizeDelta = DeviceInfoManager.Instance.GetScaledValueForSprite(
                                scalableObject[index].scaleFactor
                            );
                        }
                        else
                        {
                            scalableObject[index].reference.localScale = DeviceInfoManager.Instance.GetScaledValueForSprite(
                                scalableObject[index].scaleFactor
                            );
                        }
                    }
                }
            }
        }

        #endregion

    }
}

