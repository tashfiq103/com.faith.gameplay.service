namespace com.faith.gameplay_service
{

    //--------------------------
    //NOTE
    // Search for "UNCHECK" before building for appstore
    //--------------------------

    using System.Collections;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.UI;
    using UnityEngine.Video;

    //using com.faithstudio.SDK;

    [RequireComponent(typeof(VideoPlayer))]
    public class UniversalAdsController : MonoBehaviour
    {

        public static UniversalAdsController Instance;

        #region Public Variables

        [Header("Configuretion")]
        public bool showTestAds;
        public bool showBannerAd;

        [Space(5f)]
        [Header("Configuretion : DemoAdNetwork")]
        public bool enableDebugAdNetwork;
        [Range(0f, 60f)]
        public float durationForPrepearingRewardVideoAd;
        [Range(0f, 45f)]
        public float durationForPrepearingVideoAd;

        [Space(2.5f)]
        [Header("Reference : Media")]
        public Sprite afterAdSprite;
        public VideoClip videoAdClip;

        [Space(2.5f)]
        public GameObject cameraReference;
        public GameObject afterAdImageReference;
        public GameObject skipButtonReference;
        public GameObject closeButtonReference;

        #endregion

        //----------------------------------------------------------------------------------------------------
        #region Private Variables

        private VideoPlayer m_VideoPlayerControllerForAds;

        private bool m_IsAdvertisementVideoAd;
        private bool m_IsAdvertisementAdStopedForecfully;

        #endregion

        //----------------------------------------------------------------------------------------------------
        #region Mono Behaviour Function

        void Awake()
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

        void Start()
        {

            if (enableDebugAdNetwork)
            {

                PreProcessForDemoAdNetwork();
            }
            else
            {

                StartCoroutine(InitializeAdNetwork());
            }
        }

        #endregion

        //----------------------------------------------------------------------------------------------------
        #region Configuretion	:	General

        private IEnumerator InitializeAdNetwork()
        {

            yield return new WaitForSeconds(1f);

            NetworkReachabilityController.Instance.AddNetworkReachableEvent(delegate {
                //Write Your Code : For Ad NetWork

                //UNCHECK : After UnityAds Deployed
                //UnityAdsController.Instance.InitializedUnityAds(false, showTestAds);
#if UNITY_IOS
                //UNCHECK : After AppLovin SDK Deployed
			    //AppLovinManager.Instance.InitializeAppLovin(showTestAds);
#endif


            },
                true,
                true);

            yield return new WaitForSeconds(1f);

            if (showBannerAd)
            {
                EnableBannerAd();
            }
        }

        #endregion

        //----------------------------------------------------------------------------------------------------
        #region Configuretion	:	Demo Ad Network Controller

        private bool m_IsDemoRewardedVideoAdReady;
        private bool m_IsDemoVideoAdReady;

        private void PreProcessForDemoAdNetwork()
        {

            m_IsDemoRewardedVideoAdReady = true;
            m_IsDemoVideoAdReady = true;

            afterAdImageReference.GetComponent<Image>().sprite = afterAdSprite;

            m_VideoPlayerControllerForAds = gameObject.GetComponent<VideoPlayer>();
            m_VideoPlayerControllerForAds.clip = videoAdClip;

            skipButtonReference.GetComponent<Button>().onClick.AddListener(delegate {

                skipButtonReference.SetActive(false);
                closeButtonReference.SetActive(true);
                afterAdImageReference.SetActive(true);

                m_IsAdvertisementAdStopedForecfully = true;

                if (m_IsAdvertisementVideoAd)
                {

                    InvokeSkippedEventForRewardVideoAd();
                }
                else
                {

                    InvokeSkippedEventForInterstetialAd();
                }
            });

            closeButtonReference.GetComponent<Button>().onClick.AddListener(delegate {

                cameraReference.SetActive(false);
                closeButtonReference.SetActive(false);
                afterAdImageReference.SetActive(false);

                if (m_IsAdvertisementVideoAd)
                {

                    InvokeClosedEventForInterstetialAd();

                    if (enableDebugAdNetwork)
                        StartCoroutine(PrepeareDemoVideoAd());

                }
                else
                {

                    InvokeClosedEventForRewardVideoAd();

                    if (enableDebugAdNetwork)
                        StartCoroutine(PrepeareDemoRewardedVideoAd());
                }
            });
        }

        private IEnumerator DemoAdController()
        {

            WaitForSeconds t_CycleDelay = new WaitForSeconds(0.1f);

            double t_VideoLength = m_VideoPlayerControllerForAds.clip.length;
            double t_TimeForShowingSkipButton = 5f;
            double t_TimeForEnablingAfterAdImage = t_VideoLength * 0.95f;

            cameraReference.SetActive(true);
            afterAdImageReference.SetActive(false);
            skipButtonReference.SetActive(false);
            closeButtonReference.SetActive(false);

            m_VideoPlayerControllerForAds.Play();
            yield return new WaitForSeconds(0.5f);

            while (m_VideoPlayerControllerForAds.isPlaying && !m_IsAdvertisementAdStopedForecfully)
            {

                if (m_IsAdvertisementVideoAd)
                {

                    if (m_VideoPlayerControllerForAds.time >= t_TimeForShowingSkipButton &&
                        !skipButtonReference.activeInHierarchy)
                    {

                        skipButtonReference.SetActive(true);
                    }
                }

                yield return t_CycleDelay;
            }

            if (m_IsAdvertisementVideoAd && skipButtonReference.activeInHierarchy)
                skipButtonReference.SetActive(false);

            m_VideoPlayerControllerForAds.Stop();
            afterAdImageReference.SetActive(true);
            closeButtonReference.SetActive(true);

            if (m_IsAdvertisementVideoAd)
            {
                InvokeSuccessfulEventForRewardVideoAd();
            }

            StopCoroutine(DemoAdController());
        }

        private IEnumerator PrepeareDemoRewardedVideoAd()
        {
            yield return new WaitForSeconds(durationForPrepearingRewardVideoAd);
            m_IsDemoRewardedVideoAdReady = true;
            StopCoroutine(PrepeareDemoRewardedVideoAd());
        }

        private IEnumerator PrepeareDemoVideoAd()
        {
            Debug.Log("DemoVideoAd - NotReady");
            yield return new WaitForSeconds(durationForPrepearingVideoAd);
            m_IsDemoVideoAdReady = true;
            Debug.Log("DemoVideoAd - Ready");
            StopCoroutine(PrepeareDemoVideoAd());
        }

        private void ShowDemoRewardVideoAd()
        {

            m_IsAdvertisementVideoAd = false;
            m_IsAdvertisementAdStopedForecfully = false;

            m_IsDemoRewardedVideoAdReady = false;

            StartCoroutine(DemoAdController());
        }

        private void ShowDemoInterstetialAd()
        {

            m_IsAdvertisementVideoAd = true;
            m_IsAdvertisementAdStopedForecfully = false;

            m_IsDemoVideoAdReady = false;

            StartCoroutine(DemoAdController());
        }

        #endregion

        //----------------------------------------------------------------------------------------------------
        #region Configuretion	:	Banner Ads

        private bool m_IsBannerAdReady;

        #endregion

        //----------------------------------------------------------------------------------------------------
        #region Public Callback	:	Banner Ads

        public bool IsBannerAdReady()
        {

            if (!enableDebugAdNetwork)
            {
                return false;
            }
            else
            {

                // Write Your Code Here

                return false;
            }
        }

        public void EnableBannerAd()
        {

            NetworkReachabilityController.Instance.AddNetworkReachableEvent(delegate {
                //Write Your Code Here

                //UNCHECK : After UnityAds Deployed
                //UnityAdsController.Instance.ShowBannerAd();

#if UNITY_IOS
            //UNCHECK : After UnityAds Deployed
            //AppLovinManager.Instance.ShowBannerAd();
#endif

            },
            true,
            true
            );
        }

        #endregion

        //----------------------------------------------------------------------------------------------------
        #region Configuretion	:	Interstetial Ads

        private bool m_IsInterstetialAdReady;

        private UnityAction OnInterstetialAdSkipedEvent;
        private UnityAction OnInterstetialAdClosedEvent;

        #endregion

        //----------------------------------------------------------------------------------------------------
        #region Public Callback	:	Interstetial Ads

        public bool IsInterstetialAdReady()
        {

            if (enableDebugAdNetwork)
            {
                return m_IsDemoVideoAdReady;
            }
            else
            {
                //UNCHECK : After UnityAds Deployed


                //UNCHECK : After UnityAds Deployed
                //if (AppLovinManager.Instance.IsInterstitialReady())
                //    return true;

                return false;
            }
        }

        public void ShowInterstetialAd()
        {

            ShowInterstetialAd(null, null);
        }

        public void ShowInterstetialAd(UnityAction OnInterstetialAdClosedEvent)
        {

            ShowInterstetialAd(OnInterstetialAdClosedEvent, null);
        }

        public void ShowInterstetialAd(UnityAction OnInterstetialAdClosedEvent, UnityAction OnInterstetialAdSkipedEvent)
        {

            this.OnInterstetialAdClosedEvent = OnInterstetialAdClosedEvent;
            this.OnInterstetialAdSkipedEvent = OnInterstetialAdSkipedEvent;

            if (IsInterstetialAdReady())
            {
                if (enableDebugAdNetwork)
                {

                    ShowDemoInterstetialAd();
                }
                else
                {
                    //UNCHECK : After UnityAds Deployed


                    //UNCHECK : After UnityAds Deployed
                    //AppLovinManager.Instance.ShowInterstitialAd(OnInterstetialAdClosedEvent);
                }
            }
            else
            {
                Debug.LogError("'InterstetialAd' not ready yet. Please use 'IsInterstetialAdReady' to ensure to show your ad");
            }
        }

        public void InvokeSkippedEventForInterstetialAd()
        {
            if (OnInterstetialAdSkipedEvent != null)
                OnInterstetialAdSkipedEvent.Invoke();
        }

        public void InvokeClosedEventForInterstetialAd()
        {
            if (OnInterstetialAdClosedEvent != null)
                OnInterstetialAdClosedEvent.Invoke();
        }

        #endregion

        //----------------------------------------------------------------------------------------------------
        #region Configuretion	:	Rewarded Video Ads

        private bool m_IsRewardedVideoAdReady;

        private UnityAction OnRewardedVideoAdSuccessfullyCompletedEvent;
        private UnityAction OnRewardedVideoAdSkipedEvent;
        private UnityAction OnRewardedVideoAdClosedEvent;

        #endregion

        //----------------------------------------------------------------------------------------------------
        #region Public Callback	:	Rewarded Video Ads

        public bool IsRewardedVideoAdReady()
        {

            if (enableDebugAdNetwork)
            {
                return m_IsDemoRewardedVideoAdReady;
            }
            else
            {

                //Write Your Code Here



#if UNITY_IOS

                //UNCHECK : After UnityAds Deployed
                //UNCHECK : After AppLovin SDK Deployed
                //if (AppLovinManager.Instance.IsRewardedInterstitialReady())
                //{
                //    return true;
                //}
                //else{
                //UNCHECK: After UnityAds Deployed
                //    if (UnityAdsController.Instance.IsRewardedVideoAdsReady())
                //        return true;
                //    else
                //        return false;
                //}

#elif UNITY_ANDROID || UNITY_EDITOR

                //UNCHECK : After UnityAds Deployed
                //if (UnityAdsController.Instance.IsRewardedVideoAdsReady())
                //{
                //    return true;
                //}
#endif
                return false;

            }
        }

        public void ShowRewardVideoAd()
        {
            ShowRewardVideoAd(null, null, null);
        }

        public void ShowRewardVideoAd(UnityAction OnRewardedVideoAdSuccessfullyCompletedEvent)
        {

            ShowRewardVideoAd(OnRewardedVideoAdSuccessfullyCompletedEvent, null, null);
        }

        public void ShowRewardVideoAd(UnityAction OnRewardedVideoAdSuccessfullyCompletedEvent, UnityAction OnRewardedVideoAdClosedEvent)
        {

            ShowRewardVideoAd(OnRewardedVideoAdSuccessfullyCompletedEvent, null, OnRewardedVideoAdClosedEvent);
        }

        public void ShowRewardVideoAd(UnityAction OnRewardedVideoAdSuccessfullyCompletedEvent, UnityAction OnRewardedVideoAdSkipedEvent, UnityAction OnRewardedVideoAdClosedEvent)
        {

            this.OnRewardedVideoAdSuccessfullyCompletedEvent = OnRewardedVideoAdSuccessfullyCompletedEvent;
            this.OnRewardedVideoAdSkipedEvent = OnRewardedVideoAdSkipedEvent;
            this.OnRewardedVideoAdClosedEvent = OnRewardedVideoAdClosedEvent;

            if (IsRewardedVideoAdReady())
            {

                if (enableDebugAdNetwork)
                {

                    ShowDemoRewardVideoAd();
                }
                else
                {

                    //Write Your Code Here

#if UNITY_ANDROID || UNITY_EDITOR


                    //UNCHECK : After UnityAds Deployed
                    //UnityAdsController.Instance.ShowRewardedVideoAdsWithEvent(
                    //                        OnRewardedVideoAdSuccessfullyCompletedEvent,
                    //                        OnRewardedVideoAdSkipedEvent,
                    //                        OnRewardedVideoAdClosedEvent
                    //                    );
#elif UNITY_IOS


                
                //UNCHECK : After AppLovin SDK Deployed
                //if (AppLovinManager.Instance.IsRewardedInterstitialReady())
                //{
                //    AppLovinManager.Instance.ShowRewardInterstitialAd(OnRewardedVideoAdSuccessfullyCompletedEvent);
                //}

                //UNCHECK : After UnityAds Deployed
                //else
                //{
                //    UnityAdsController.Instance.ShowRewardedVideoAdsWithEvent(
                //                                    OnRewardedVideoAdSuccessfullyCompletedEvent,
                //                                    OnRewardedVideoAdSkipedEvent,
                //                                    OnRewardedVideoAdClosedEvent
                //                                );
                //}
				
#endif


                }
            }
            else
            {
                Debug.LogError("'RewardedVideAd' not ready yet. Please use 'IsRewardedVideoAdReady' to ensure to show your ad");
            }
        }

        public void InvokeSuccessfulEventForRewardVideoAd()
        {
            if (OnRewardedVideoAdSuccessfullyCompletedEvent != null)
                OnRewardedVideoAdSuccessfullyCompletedEvent.Invoke();
        }

        public void InvokeSkippedEventForRewardVideoAd()
        {
            if (OnRewardedVideoAdSkipedEvent != null)
                OnRewardedVideoAdSkipedEvent.Invoke();
        }

        public void InvokeClosedEventForRewardVideoAd()
        {
            if (OnRewardedVideoAdClosedEvent != null)
                OnRewardedVideoAdClosedEvent.Invoke();
        }

        #endregion
    }
}

