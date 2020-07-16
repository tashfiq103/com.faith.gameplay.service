namespace com.faith.gameplay.service
{
    using UnityEngine;

    public class GlobalTouchController : MonoBehaviour
    {

        #region Custom Variables

        public delegate void OnTouchDownEvent(Vector3 t_TouchPosition);
        public delegate void OnTouchEvent(Vector3 t_TouchPosition);
        public delegate void OnTouchUpEvent(Vector3 t_TouchPosition);

        #endregion

        #region Public Variables

        public static GlobalTouchController Instance;

        public OnTouchDownEvent OnTouchDown;
        public OnTouchEvent OnTouch;
        public OnTouchUpEvent OnTouchUp;

        #endregion

        #region Private Variables

        private bool m_IsTouchControllerRunning = false;

        private Touch m_ActiveTouch;

        #endregion

        #region Mono Bheaviour

        private void Awake()
        {
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
            if (m_IsTouchControllerRunning)
            {
                TouchController();
            }
        }

        #endregion

        #region Configuretion

        private void TouchController()
        {

#if UNITY_EDITOR

            if (Input.GetMouseButtonDown(0))
            {
                OnTouchDown?.Invoke(Input.mousePosition);
            }

            if (Input.GetMouseButton(0))
            {

                OnTouch?.Invoke(Input.mousePosition);
            }

            if (Input.GetMouseButtonUp(0))
            {

                OnTouchUp?.Invoke(Input.mousePosition);
            }


#elif UNITY_ANDROID || UNITY_IOS
       if (Input.touchCount > 0) {

                m_ActiveTouch = Input.GetTouch(0);

                switch (m_ActiveTouch.phase)
                {

                    case TouchPhase.Began:
                        OnTouchDown?.Invoke(m_ActiveTouch.position);
                        break;

                    case TouchPhase.Stationary:
                        OnTouch?.Invoke(m_ActiveTouch.position);
                        break;

                    case TouchPhase.Moved:
                        OnTouch?.Invoke(m_ActiveTouch.position);
                        break;

                    case TouchPhase.Ended:
                        OnTouchUp?.Invoke(m_ActiveTouch.position);
                        break;

                    case TouchPhase.Canceled:
                        OnTouchUp?.Invoke(m_ActiveTouch.position);
                        break;
                }
            }
#endif

        }

        #endregion

        #region Public Callback

        public void EnableTouchController()
        {

            m_IsTouchControllerRunning = true;
        }

        public void DisableTouchController(bool t_ResetTouchEvents = false)
        {

            m_IsTouchControllerRunning = false;

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


