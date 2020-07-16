namespace com.faith.gameplay.service {

    using UnityEngine;

    public class GlobalMonetizationStateController : MonoBehaviour
    {
        #region Public Variables

        public static GlobalMonetizationStateController Instance;

        public UIMonetizationController earningBooster;
        public UIMonetizationController waterPowerBooster;

        #endregion

        #region Mono Behaviour

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

        #endregion

        #region Public Callback

        public void StartMonetization()
        {

            earningBooster.StartMonetizationController();
            waterPowerBooster.StartMonetizationController();
        }

        public void StopMonetization()
        {

            earningBooster.StopMonetizationController();
            waterPowerBooster.StopMonetizationController();
        }

        public bool IsCoinEarnBoostEnabled()
        {

            return earningBooster.IsMonetizationActive();
        }

        public bool IsWaterBoostEnabled()
        {

            return waterPowerBooster.IsMonetizationActive();
        }

        #endregion



    }
}


