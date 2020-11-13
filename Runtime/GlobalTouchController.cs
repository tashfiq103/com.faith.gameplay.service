namespace com.faith.gameplay.service
{
    using UnityEngine;

    public class GlobalTouchController : MonoBehaviour
    {

        #region Custom Variables

        public delegate void OnTouchDownEvent(Vector3 touchPosition, int touchIndex);
        public delegate void OnTouchEvent(Vector3 touchPosition, int touchIndex);
        public delegate void OnTouchUpEvent(Vector3 touchPosition, int touchIndex);

        #endregion

        #region Public Variables

        public static GlobalTouchController Instance;

        public OnTouchDownEvent OnTouchDown;
        public OnTouchEvent OnTouch;
        public OnTouchUpEvent OnTouchUp;

        #endregion


        #region Mono Bheaviour

        private void Awake()
        {
            enabled = false;

            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {

                Destroy(gameObject);
            }
        }

        private void Update()
        {
            
                TouchController();
            
        }

        #endregion

        #region Configuretion

        private void TouchController()
        {

#if UNITY_EDITOR

            if (Input.GetMouseButtonDown(0))
            {
                OnTouchDown?.Invoke(Input.mousePosition,0);
            }

            if (Input.GetMouseButton(0))
            {

                OnTouch?.Invoke(Input.mousePosition, 0);
            }

            if (Input.GetMouseButtonUp(0))
            {

                OnTouchUp?.Invoke(Input.mousePosition,0);
            }

#elif UNITY_ANDROID || UNITY_IOS

            Touch[] activeTouches = Input.touches;
            int touchCount = activeTouches.Length;
            for (int i = 0; i < touchCount; i++)
            {

                switch (activeTouches[i].phase)
                {

                    case TouchPhase.Began:
                        OnTouchDown?.Invoke(activeTouches[i].position, i);
                        break;

                    case TouchPhase.Stationary:
                        OnTouch?.Invoke(activeTouches[i].position, i);
                        break;

                    case TouchPhase.Moved:
                        OnTouch?.Invoke(activeTouches[i].position, i);
                        break;

                    case TouchPhase.Ended:
                        OnTouchUp?.Invoke(activeTouches[i].position, i);
                        break;

                    case TouchPhase.Canceled:
                        OnTouchUp?.Invoke(activeTouches[i].position, i);
                        break;
                }
            }

#endif

        }

        #endregion

        #region Public Callback

        public void EnableTouchController()
        {

            enabled = true;
        }

        public void DisableTouchController(bool t_ResetTouchEvents = false)
        {

            enabled = false;

            if (t_ResetTouchEvents)
            {

                OnTouchDown = null;
                OnTouch = null;
                OnTouchUp = null;
            }
        }

        #endregion
    }
}


