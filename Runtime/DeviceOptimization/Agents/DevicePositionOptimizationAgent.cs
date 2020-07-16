
namespace com.faith.gameplay.service
{
    using UnityEngine;

    public struct UnitVector2D{
        [Range(-1,1)]
        public float x;
        [Range(-1,1)]
        public float y;
    }

    [System.Serializable]
    public struct PositionalObject
    {
        public Transform reference;
        public UnitVector2D position;
    }

    public class DevicePositionOptimizationAgent : MonoBehaviour
    {

        /// <summary>
        /// Callback to draw gizmos only if the object is selected.
        /// </summary>
        void OnDrawGizmosSelected()
        {
            if (positionalObject != null)
            {

                Gizmos.color = Color.cyan;
                int t_NumberOfPositionalObject = positionalObject.Length;
                for (int index = 0; index < t_NumberOfPositionalObject; index++)
                {

                    if (positionalObject[index].reference != null)
                    {

                        Gizmos.DrawWireSphere(
                            positionalObject[index].reference.position,
                            0.5f
                        );
                    }
                }
            }
        }

        public bool enableScaleWithAxis;
        public Vector2 scaledWithAxis;
        [Space(2.5f)]
        public PositionalObject[] positionalObject;

        void Start()
        {

            SetPositionOfAllObject();
        }

        public void SetPositionOfAllObject()
        {

            float t_CameraOrthographicSize = Camera.main.orthographicSize;
            Vector2 t_CameraPosition = Camera.main.transform.position;
            Vector2 t_ViewBoundary = Vector2.zero;

            if (DeviceInfoManager.Instance.IsPortraitMode())
            {

                t_ViewBoundary = new Vector2(
                    (t_CameraOrthographicSize / DeviceInfoManager.Instance.GetAspectRatioFactor()) * (enableScaleWithAxis ? scaledWithAxis.x : 1.0f),
                    t_CameraOrthographicSize * (enableScaleWithAxis ? scaledWithAxis.y : 1.0f)
                );
            }
            else
            {

                t_ViewBoundary = new Vector2(
                    (t_CameraOrthographicSize * DeviceInfoManager.Instance.GetAspectRatioFactor()) * (enableScaleWithAxis ? scaledWithAxis.x : 1.0f),
                    t_CameraOrthographicSize * (enableScaleWithAxis ? scaledWithAxis.y : 1.0f)
                );
            }

            if (positionalObject != null)
            {

                for (int index = 0; index < positionalObject.Length; index++)
                {

                    if (positionalObject[index].reference != null)
                    {

                        positionalObject[index].reference.position = new Vector2(
                            t_CameraPosition.x + (positionalObject[index].position.x * t_ViewBoundary.x),
                            t_CameraPosition.y + (positionalObject[index].position.y * t_ViewBoundary.y)
                        );
                    }
                }
            }
        }

        public void SetPositionOfObject(Transform reference, UnitVector2D position)
        {

            int t_NumberOfPositionObject = positionalObject.Length;
            for (int index = 0; index < t_NumberOfPositionObject; index++)
            {

                if (positionalObject[index].reference == reference)
                {

                    Vector2 t_CameraPosition = Camera.main.transform.position;
                    float t_CameraOrthographicSize = Camera.main.orthographicSize;
                    Vector2 t_ViewBoundary = Vector2.zero;

                    if (DeviceInfoManager.Instance.IsPortraitMode())
                    {

                        t_ViewBoundary = new Vector2(
                            t_CameraOrthographicSize / DeviceInfoManager.Instance.GetAspectRatioFactor(),
                            t_CameraOrthographicSize
                        );
                    }
                    else
                    {

                        t_ViewBoundary = new Vector2(
                            t_CameraOrthographicSize * DeviceInfoManager.Instance.GetAspectRatioFactor(),
                            t_CameraOrthographicSize
                        );
                    }

                    Vector2 t_NewPosition = new Vector2(
                        t_CameraPosition.x + (position.x * t_ViewBoundary.x),
                        t_CameraPosition.y + (position.y * t_ViewBoundary.y)
                    );

                    positionalObject[index].reference.position = t_NewPosition;

                    break;
                }
            }
        }
    }
}

