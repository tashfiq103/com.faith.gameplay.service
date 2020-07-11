namespace com.faith.gameplay_service {
    using System.Collections;
    using UnityEngine;

    public class DeviceInfoManager : MonoBehaviour
    {

        public static DeviceInfoManager Instance;

#if UNITY_IOS

	private Vector2 BASE_RESOLUTION_FOR_IPHONEX = new Vector2 (1125, 2436);
	private Vector2 BASE_RESOLUTION_FOR_IPAD_3x4 = new Vector2 (768, 1024);
	private Vector2 BASE_RESOLUTION_FOR_IPHONE_09x16 = new Vector2 (640, 1136);
	private Vector2 BASE_RESOLUTION_FOR_IPHONE_LOW_END = new Vector2 (320, 480);

#elif UNITY_ANDROID

        private Vector2 BASE_RESOLUTION_FOR_ANDROID_LOW_END = new Vector2(320, 480);
        private Vector2 BASE_RESOLUTION_FOR_ANDROID_10x16 = new Vector2(480, 800);
        private Vector2 BASE_RESOLUTION_FOR_ANDROID_09x16 = new Vector2(360, 640);
        private Vector2 BASE_RESOLUTION_FOR_ANDROID_10x17 = new Vector2(600, 1024);

#endif

        //----------
        #region Public Variables

        [Header("Device Info")]
        public bool showFPS;
        public bool showGameSpeed;
        [Range(12, 60)]
        public int targetedFramePerSec;

        [Space(5.0f)]
        public bool resetPlayerPrefAtBegining;

#if UNITY_EDITOR

        [Space(5f)]
        public bool isPortraitScreen;

        [Range(1.0f, 2.5f)]
        public float workingRatio;

#endif

#if UNITY_IOS

	[Header ("iPhone 3/4/4s")]
	public UnityEvent OnIPhoneLowEndDevicesEvent;
	[Space (5.0f)]
	[Header ("iPhone 4/4s/5/5s/6/6+/6s/6s+/7/7+/8/8+")]
	public UnityEvent OnIPhoneDevicesEvent;
	[Space (5.0f)]
	[Header ("iPhone X")]
	public UnityEvent OnIPhoneXDevicesEvent;
	[Space (5.0f)]
	[Header ("iPad")]
	public UnityEvent OnIPadDevicesEvent;

#elif UNITY_ANDROID

        public UnityEvent OnAndroidLowEndDevicesEvent;
        public UnityEvent OnAndroid10x16DevicesEvent;
        public UnityEvent OnAndroid09x16DevicesEvent;
        public UnityEvent OnAndroid10x17DevicesEvent;

#endif

        #endregion

        //----------
        #region Private Variables

        [HideInInspector]
        public string HAS_RESET_PLAYERPREF = "HAS_RESET_PLAYERPREF_";


        private Camera m_MainCameraReference;

        private float m_ScreenWidth;
        private float m_ScreenHeight;

        //Aspect Ratio
        private Vector2 m_AspectRatio;
        private float m_AspectRatioFactor;

        private Vector2 m_ScreenSizeMultiplier;
        private float m_UIPositionFactor = 0.0f;
        private float m_SpriteSizeFactor = 0.0f;

        private float deltaTime = 0f;

        #endregion

        //----------
        #region Mono Behaviour



        void Awake()
        {
#if UNITY_EDITOR
            if (resetPlayerPrefAtBegining)
            {
                StartCoroutine(ResetingPlayerPrefs());
            }
#elif UNITY_ANDROID
        if (resetPlayerPrefAtBegining) {
            if (PlayerPrefs.GetInt(HAS_RESET_PLAYERPREF + Application.version, 0) == 0)
            {
                StartCoroutine(ResetingPlayerPrefs());
            }
        }
#endif





            Application.targetFrameRate = targetedFramePerSec;
            QualitySettings.vSyncCount = 0;

            if (Instance == null)
            {

                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {

                Destroy(gameObject);
            }

            PreProcess();
        }

        void Start()
        {

#if UNITY_IOS

		if (m_AspectRatioFactor >= 2.16) {
			// iPhone-10

			OnIPhoneXDevicesEvent.Invoke ();
		} else if (m_AspectRatioFactor >= 1.77) {
			// iPhone-5, iPhone-5s, iPhone-6/6s, iPhone-6s+/7s+, iPhone-7/7+, iPhone-8/8+

			OnIPhoneDevicesEvent.Invoke ();
		} else if (m_AspectRatioFactor >= 1.5) {
			// (480x320) iPhone-3, iPhone-4, iPhone-4s

			OnIPhoneLowEndDevicesEvent.Invoke ();
		} else if (m_AspectRatioFactor >= 1.33) {
			// iPad (All)

			OnIPadDevicesEvent.Invoke ();
		}

#elif UNITY_ANDROID

            if (m_AspectRatioFactor >= 1.77f)
            {
                //Android Device : 9x16

                OnAndroid09x16DevicesEvent.Invoke();
            }
            else if (m_AspectRatioFactor >= 1.70f)
            {
                //Android Device : 10x17

                OnAndroid10x17DevicesEvent.Invoke();
            }
            else if (m_AspectRatioFactor >= 1.60f)
            {
                //Android Device : 10x16

                OnAndroid10x16DevicesEvent.Invoke();
            }
            else if (m_AspectRatioFactor >= 1.5f)
            {
                //Android Device : LowEnd

                OnAndroidLowEndDevicesEvent.Invoke();
            }

#endif
        }

        void Update()
        {
            deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        }

        void OnGUI()
        {

            int w = Screen.width, h = Screen.height;
            int t_Height = h * 2 / 100;

            if (showFPS)
            {

                GUIStyle style = new GUIStyle();

                Rect rect = new Rect(0, 0, w, t_Height);
                style.alignment = TextAnchor.UpperLeft;
                style.fontSize = t_Height;
                style.normal.textColor = new Color(0.0f, 1.0f, 0.0f, 1.0f);
                float msec = deltaTime * 1000.0f;
                float fps = 1.0f / deltaTime;
                string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
                GUI.Label(rect, text, style);
            }

            if (showGameSpeed)
            {

                GUIStyle style = new GUIStyle();

                Rect rect = new Rect(0, t_Height, w, t_Height);
                style.alignment = TextAnchor.UpperLeft;
                style.fontSize = t_Height;
                style.normal.textColor = new Color(0.0f, 1.0f, 0.0f, 1.0f);

                string text = Time.timeScale.ToString();

                // if (GameManager.Instance != null)
                // {
                //     text = "Speed : " + GameManager.Instance.GetGameSpeed();
                // }
                // else
                // {

                //     text = "Speed : GM not initialized";
                // }

                GUI.Label(rect, text, style);
            }
        }

        #endregion

        //----------
        #region Configuretion

        private IEnumerator ResetingPlayerPrefs()
        {

            PlayerPrefs.DeleteAll();
            yield return new WaitForSeconds(1f);
            PlayerPrefs.SetInt(HAS_RESET_PLAYERPREF + Application.version, 1);

            StopCoroutine(ResetingPlayerPrefs());
        }

        private void PreProcess()
        {

            m_MainCameraReference = Camera.main;

            m_ScreenWidth = Screen.width;
            m_ScreenHeight = Screen.height;

            CalculateUIPositionFactor();
            CalculateSpriteSizeFactor();
        }

        private void CalculateAspectRatioFactor()
        {

            if (m_ScreenHeight > m_ScreenWidth)
                m_AspectRatioFactor = m_ScreenHeight / m_ScreenWidth;
            else
                m_AspectRatioFactor = m_ScreenWidth / m_ScreenHeight;
        }

        private void CalculateAspectRatio()
        {

            CalculateAspectRatioFactor();

#if UNITY_IOS

		if (m_AspectRatioFactor >= 2.16) {
			// iPhone-10
			m_AspectRatio = new Vector2 (9.0f, 19.5f);

		} else if (m_AspectRatioFactor >= 1.77) {
			// iPhone-5, iPhone-5s, iPhone-6/6s, iPhone-6s+/7s+, iPhone-7/7+, iPhone-8/8+
			m_AspectRatio = new Vector2 (9f, 16.0f);

		} else if (m_AspectRatioFactor >= 1.5) {
			// (480x320) iPhone-3, iPhone-4, iPhone-4s
			m_AspectRatio = new Vector2 (2.0f, 3.0f);

		} else if (m_AspectRatioFactor >= 1.33) {
			// iPad (All)
			m_AspectRatio = new Vector2 (3.0f, 4.0f);
		}

#elif UNITY_ANDROID

            if (m_AspectRatioFactor >= 1.77f)
            {
                //Android Device : 9x16
                m_AspectRatio = new Vector2(9.0f, 16.0f);

            }
            else if (m_AspectRatioFactor >= 1.70f)
            {
                //Android Device : 10x17
                m_AspectRatio = new Vector2(10.0f, 17.0f);

            }
            else if (m_AspectRatioFactor >= 1.60f)
            {
                //Android Device : 10x16
                m_AspectRatio = new Vector2(10.0f, 16.0f);

            }
            else if (m_AspectRatioFactor >= 1.5f)
            {
                //Android Device : LowEnd
                m_AspectRatio = new Vector2(2.0f, 3.0f);
            }

#endif
        }

        private void CalculateUIPositionFactor()
        {

            CalculateAspectRatio();

#if UNITY_IOS

		if (m_AspectRatioFactor >= 2.16) {
			// iPhone-10

			m_ScreenSizeMultiplier = new Vector2 (
				m_ScreenWidth / BASE_RESOLUTION_FOR_IPHONEX.x,
				m_ScreenHeight / BASE_RESOLUTION_FOR_IPHONEX.y
			);

			//Special Case : For iPhone
			m_ScreenSizeMultiplier *= 1.77f;
		} else if (m_AspectRatioFactor >= 1.77) {
			// iPhone-5, iPhone-5s, iPhone-6/6s, iPhone-6s+/7s+, iPhone-7/7+, iPhone-8/8+

			m_ScreenSizeMultiplier = new Vector2 (
				m_ScreenWidth / BASE_RESOLUTION_FOR_IPHONE_09x16.x,
				m_ScreenHeight / BASE_RESOLUTION_FOR_IPHONE_09x16.y
			);
		} else if (m_AspectRatioFactor >= 1.5) {
			// (480x320) iPhone-3, iPhone-4, iPhone-4s

			m_ScreenSizeMultiplier = new Vector2 (
				m_ScreenWidth / BASE_RESOLUTION_FOR_IPHONE_LOW_END.x,
				m_ScreenHeight / BASE_RESOLUTION_FOR_IPHONE_LOW_END.y
			);
		} else if (m_AspectRatioFactor >= 1.33) {
			// iPad (All)

			m_ScreenSizeMultiplier = new Vector2 (
				m_ScreenWidth / BASE_RESOLUTION_FOR_IPAD_3x4.x,
				m_ScreenHeight / BASE_RESOLUTION_FOR_IPAD_3x4.y
			);
		}

#elif UNITY_ANDROID

            if (m_AspectRatioFactor >= 1.77f)
            {
                //Android Device : 9x16

                m_ScreenSizeMultiplier = new Vector2(
                    m_ScreenWidth / BASE_RESOLUTION_FOR_ANDROID_09x16.x,
                    m_ScreenHeight / BASE_RESOLUTION_FOR_ANDROID_09x16.y
                );
            }
            else if (m_AspectRatioFactor >= 1.70f)
            {
                //Android Device : 10x17

                m_ScreenSizeMultiplier = new Vector2(
                    m_ScreenWidth / BASE_RESOLUTION_FOR_ANDROID_10x17.x,
                    m_ScreenHeight / BASE_RESOLUTION_FOR_ANDROID_10x17.y
                );
            }
            else if (m_AspectRatioFactor >= 1.60f)
            {
                //Android Device : 10x16

                m_ScreenSizeMultiplier = new Vector2(
                    m_ScreenWidth / BASE_RESOLUTION_FOR_ANDROID_10x16.x,
                    m_ScreenHeight / BASE_RESOLUTION_FOR_ANDROID_10x16.y
                );
            }
            else if (m_AspectRatioFactor >= 1.5f)
            {

                m_ScreenSizeMultiplier = new Vector2(
                    m_ScreenWidth / BASE_RESOLUTION_FOR_ANDROID_LOW_END.x,
                    m_ScreenHeight / BASE_RESOLUTION_FOR_ANDROID_LOW_END.y
                );
            }

#endif

            m_UIPositionFactor = (m_ScreenSizeMultiplier.x + m_ScreenSizeMultiplier.y) / 2.0f;
        }

        private void CalculateSpriteSizeFactor()
        {

            float m_ScreenSizeRatio = 0.0f;

            if (m_ScreenHeight > m_ScreenWidth)
                m_ScreenSizeRatio = m_AspectRatio.x / m_AspectRatio.y;
            else
                m_ScreenSizeRatio = m_AspectRatio.y / m_AspectRatio.x;

            if (m_ScreenSizeRatio >= 0.75f) // Almost Square
                m_ScreenSizeRatio = m_ScreenSizeRatio * 2.0f;

            CalculateAspectRatioFactor();
            int m_Iteration = 1;
            float m_LocalAspectRatioFactor = m_AspectRatioFactor;
            m_SpriteSizeFactor = 0.0f;
            while (true)
            {

                if (m_LocalAspectRatioFactor >= 1.0f)
                {
                    m_LocalAspectRatioFactor--;
                    if (m_SpriteSizeFactor == 0.0f)
                        m_SpriteSizeFactor = 1.0f;
                    else
                        m_SpriteSizeFactor += ((1.0f / m_Iteration) * m_ScreenSizeRatio);
                }
                else
                {

                    m_SpriteSizeFactor += (m_LocalAspectRatioFactor * m_ScreenSizeRatio);
                    break;
                }
                m_Iteration++;
            }

            //Testing : Keeping This One Or Previous One
            //float m_ScreenMultiplicant 	= (m_ScreenSizeMultiplier.x + m_ScreenSizeMultiplier.y) / 2.0f;
            //m_SpriteSizeFactor 			=  m_ScreenMultiplicant == 1.0f ? m_SpriteSizeFactor : m_ScreenMultiplicant;
            //m_SpriteSizeFactor			*= (1.0f + (m_AspectRatioFactor - (m_AspectRatioFactor >= 2.0f ? 2.0f : 1.0f)) / 2.0f); 
            //m_SpriteSizeFactor = 1.0f;	
        }

        #endregion

        //----------
        #region Public Callback : UI

        public void ReCalculateDeviceInfo()
        {

            PreProcess();
        }

        public bool IsPortraitMode()
        {

#if UNITY_EDITOR

            if (isPortraitScreen)
            {

                return true;
            }
            else
            {
                return false;
            }

#else

		if (m_ScreenWidth > m_ScreenHeight) {

			return false;
		} else {
			return true;
		}

#endif

        }

        public Vector3 GetScaledValueForSprite(float t_ScalingFactor)
        {

            Vector3 t_Result = Vector3.zero;
            float t_UpperLimitOfSize = Camera.main.orthographicSize * 2.0f;
            float t_SelectedRatio = GetAspectRatioFactor();

            if (IsPortraitMode())
            {
                t_Result = new Vector3(
                    (t_UpperLimitOfSize / t_SelectedRatio) * t_ScalingFactor,
                    t_UpperLimitOfSize * t_ScalingFactor,
                    1.0f
                );
            }
            else
            {
                t_Result = new Vector3(
                    t_UpperLimitOfSize * t_ScalingFactor,
                    (t_UpperLimitOfSize / t_SelectedRatio) * t_ScalingFactor,
                    1.0f
                );
            }

            return t_Result;
        }

        public float GetUIPositionFactor()
        {
            return m_UIPositionFactor;
        }

        public float GetAspectRatioFactor()
        {

#if UNITY_EDITOR
            return workingRatio;

#else

		return m_AspectRatioFactor;

#endif

        }

        public Vector2 GetAspectRatio()
        {
            return m_AspectRatio;
        }

        public bool IsDevice_iPhone()
        {

#if UNITY_IOS

		if(m_AspectRatioFactor >= 1.76f && m_AspectRatioFactor <= 1.8f){
			return true;
		}else{

			return false;
		}

#elif UNITY_EDITOR

            if (workingRatio >= 1.76f && workingRatio <= 1.8f)
            {
                return true;
            }
            else
            {

                return false;
            }

#else

		return false;

#endif
        }

        public bool IsDevice_iPad()
        {

#if UNITY_IOS

		if(m_AspectRatioFactor >= 1.33f && m_AspectRatioFactor <= 1.35f){
			return true;
		}else{

			return false;
		}

#elif UNITY_EDITOR

            if (workingRatio >= 1.33 && workingRatio <= 1.35)
            {
                return true;
            }
            else
            {

                return false;
            }

#else

		return false;
		
#endif
        }

        public bool IsDevice_iPhoneX()
        {

#if UNITY_IOS

		if(m_AspectRatioFactor >= 2.15 && m_AspectRatioFactor <= 2.17){
			return true;
		}else{

			return false;
		}

#elif UNITY_EDITOR

            if (workingRatio >= 2.15 && workingRatio <= 2.17)
            {
                return true;
            }
            else
            {

                return false;
            }

#else

		return false;
		
#endif
        }

        #endregion
    }
}

