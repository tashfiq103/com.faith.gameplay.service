namespace com.faith.gameplay.service {

    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.UI;
    using TMPro;
    using System.Collections;

    public class UIMonetizationController : MonoBehaviour
    {
        #region Public Variables

        public string trackerName;
        [Range(1, 300)]
        public int durationForMonetization;
        [Range(5, 60)]
        public int randomDelayOnOffer;


        [Space(5.0f)]
        public Animator panelAnimatorReference;
        public GameObject panelForOffer;
        public GameObject panelForPurchased;

        [Space(5.0f)]
        public TextMeshProUGUI remainingTimeText;
        public Button adButton;

        [Space(5.0f)]
        public UnityEvent OnMonetizationStart;
        public UnityEvent OnMonetizationEnd;

        #endregion

        #region Private Variables

        private bool m_IsMonetizationControllerRunning = false;

        #endregion

        #region Mono Behaviour

        private void Awake()
        {
            adButton.onClick.AddListener(delegate
            {
                if (UniversalAdsController.Instance.IsRewardedVideoAdReady()
                && !IsMonetizationActive())
                {

                    UniversalAdsController.Instance.ShowRewardVideoAd(
                            delegate
                            {
                                PlayerPrefs.SetInt("TRACKER_FOR_REMAINING_TIME_" + trackerName, durationForMonetization);
                                PlayerPrefs.SetInt("TRACKER_FOR_ACTIVE_" + trackerName + "_MONETIZATION", 1);

                                OnMonetizationStart.Invoke();

#if UNITY_ANDROID



#elif UNITY_IOS
                            //UNCHECK : After Deploy
                            //FacebookAnalyticsManager.Instance.FBRewardedVideoAd(trackerName);
                            //FirebaseAnalyticsEventController.Instance.UpdateFirebaseEventForRewardVideoAdOnBoost(trackerName);
#endif


                        }
                        );
                }
            });
        }


        #endregion

        #region Configuretion

        private int GetRemainingTimeForMonetization()
        {
            return PlayerPrefs.GetInt("TRACKER_FOR_REMAINING_TIME_" + trackerName, 0);
        }

        private void DeductRemainingTime()
        {

            int t_CurrentRemainingTime = GetRemainingTimeForMonetization();

            if (t_CurrentRemainingTime > 0)
            {

                t_CurrentRemainingTime--;
                PlayerPrefs.SetInt("TRACKER_FOR_REMAINING_TIME_" + trackerName, t_CurrentRemainingTime);

                if (t_CurrentRemainingTime <= 0)
                {
                    PlayerPrefs.SetInt("TRACKER_FOR_ACTIVE_" + trackerName + "_MONETIZATION", 0);
                    OnMonetizationEnd.Invoke();
                }
            }
        }

        private IEnumerator ControllerForMonetization()
        {

            float t_CycleLength = 1;
            WaitForSeconds t_CycleDelay = new WaitForSeconds(t_CycleLength);
            WaitUntil t_WaitUntilMonetizationEnabled = new WaitUntil(() =>
            {
                if (IsMonetizationActive())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            });

            int t_CurrentRemainingTime;

            while (m_IsMonetizationControllerRunning)
            {

                if (IsMonetizationActive())
                {
                    // if : Already Purchased Monetization

                    OnMonetizationStart.Invoke();

                    panelForOffer.SetActive(false);
                    panelForPurchased.SetActive(true);

                    panelAnimatorReference.SetTrigger("APPEAR");

                    int t_RemainingTimeForMonetization = GetRemainingTimeForMonetization();
                    for (int timeIndex = 0; timeIndex < t_RemainingTimeForMonetization; timeIndex++)
                    {

                        t_CurrentRemainingTime = GetRemainingTimeForMonetization();

                        remainingTimeText.text =
                            (((t_CurrentRemainingTime / 60) == 0) ? "00" : "0" + (t_CurrentRemainingTime / 60).ToString())
                            + ":"
                            + ((t_CurrentRemainingTime % 60) < 10 ? ("0" + (t_CurrentRemainingTime % 10).ToString()) : (t_CurrentRemainingTime % 60).ToString());

                        yield return t_CycleDelay;
                        DeductRemainingTime();
                    }

                    panelAnimatorReference.SetTrigger("DISAPPEAR");

                    yield return new WaitForSeconds(Random.Range(randomDelayOnOffer * 0.5f, randomDelayOnOffer));

                    panelForOffer.SetActive(true);
                    panelForPurchased.SetActive(false);
                }
                else
                {
                    // if : Monetization is not purchased

                    if (UniversalAdsController.Instance.IsRewardedVideoAdReady())
                    {
                        yield return new WaitForSeconds(Random.Range(randomDelayOnOffer * 0.6f, randomDelayOnOffer * 0.8f));

                        panelForOffer.SetActive(true);
                        panelForPurchased.SetActive(false);

                        panelAnimatorReference.SetTrigger("APPEAR");

                        int t_RandomOfferTime = Random.Range(5, 10);
                        for (int timeStempIndex = 0; timeStempIndex < t_RandomOfferTime; timeStempIndex++)
                        {

                            if (IsMonetizationActive())
                                break;

                            yield return t_CycleDelay;
                        }

                        if (!IsMonetizationActive())
                            panelAnimatorReference.SetTrigger("DISAPPEAR");

                    }
                    else
                    {

                        yield return t_CycleDelay;
                    }
                }
            }

            m_IsMonetizationControllerRunning = false;
            StopCoroutine(ControllerForMonetization());
        }

        #endregion

        #region Public Callback

        public bool IsMonetizationActive()
        {

            if (PlayerPrefs.GetInt("TRACKER_FOR_ACTIVE_" + trackerName + "_MONETIZATION", 0) == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void StartMonetizationController()
        {

            if (!m_IsMonetizationControllerRunning)
            {

                m_IsMonetizationControllerRunning = true;
                StartCoroutine(ControllerForMonetization());
            }
        }

        public void StopMonetizationController()
        {

            m_IsMonetizationControllerRunning = false;

            if (panelForOffer.activeInHierarchy || panelForPurchased.activeInHierarchy)
                panelAnimatorReference.SetTrigger("DISAPPEAR");
        }

        #endregion
    }
}


