namespace com.faith.gameplay.service
{
    using UnityEngine;
    using UnityEngine.Events;

    [System.Serializable]
    public struct DeviceOptimizedObject
    {
        [Header("iPhone Resolution")]
        public GameObject objectWith3x2;        // Device Index = 0
        public UnityEvent OnIPhoneLowEndDevicesEvent;

        [Space(5.0f)]
        public GameObject objectWith4x3;        // Device Index = 1
        public UnityEvent OnIPadDevicesEvent;

        [Space(5.0f)]
        public GameObject objectWith16x9;       // Device Index = 2
        public UnityEvent OnIPhoneDevicesEvent;

        [Space(5.0f)]
        public GameObject objectWith19_5x9;     // Device Index = 3
        public UnityEvent OnIPhoneXDevicesEvent;

        [Space(5.0f)]
        [Header("Android Resolution (With iOS)")]
        public GameObject objectWith16x10;      // Device Index = 4
        public UnityEvent OnAndroid10x16DevicesEvent;

        [Space(5.0f)]
        public GameObject objectWith17x10;      // Device Index = 5
        public UnityEvent OnAndroid10x17DevicesEvent;

    }

    [System.Serializable]
    public struct DeviceOptimizedEvent
    {

        [Header("iPhone Resolution")]
        public UnityEvent OnIPhoneLowEndDevicesEvent;
        public UnityEvent OnIPadDevicesEvent;
        public UnityEvent OnIPhoneDevicesEvent;
        public UnityEvent OnIPhoneXDevicesEvent;

        [Space(5.0f)]
        [Header("Android Resolution (With iOS)")]
        public UnityEvent OnAndroid10x16DevicesEvent;
        public UnityEvent OnAndroid10x17DevicesEvent;
    }

    public class DifferentDeviceEvent : MonoBehaviour
    {

        #region Public Variables

        public bool DisabledTriggerOnStart;

        [Space(10.0f)]
        public DeviceOptimizedObject[] deviceOptimizedObject;
        public DeviceOptimizedEvent[] deviceOptimizedEvent;

        /*
        #if UNITY_IOS

        [Header("(3:2) iPhone 3/4/4s")]
        public UnityEvent OnIPhoneLowEndDevicesEvent;
        [Space(5.0f)]
        [Header("(16:9) iPhone 4/4s/5/5s/6/6+/6s/6s+/7/7+/8/8+")]
        public UnityEvent OnIPhoneDevicesEvent;
        [Space(5.0f)]
        [Header("(19.5:9) iPhone X")]
        public UnityEvent OnIPhoneXDevicesEvent;
        [Space(5.0f)]
        [Header("(4:3) iPad")]
        public UnityEvent OnIPadDevicesEvent;

        #elif UNITY_ANDROID

        public UnityEvent OnAndroidLowEndDevicesEvent;
        public UnityEvent OnAndroid10x16DevicesEvent;
        public UnityEvent OnAndroid09x16DevicesEvent;
        public UnityEvent OnAndroid10x17DevicesEvent;

        #endif
        */
        #endregion

        //----------
        #region Private Variables

        private int m_DeviceIndex;

        #endregion

        // Use this for initialization
        void Start()
        {

            if (!DisabledTriggerOnStart)
            {

                EnableDeviceObjects();
            }
        }

        private void DisableOtherObject(GameObject enabledObject, int index)
        {

            if (enabledObject == deviceOptimizedObject[index].objectWith3x2)
            {

                if (deviceOptimizedObject[index].objectWith4x3 != null)
                    deviceOptimizedObject[index].objectWith4x3.SetActive(false);

                if (deviceOptimizedObject[index].objectWith16x9 != null)
                    deviceOptimizedObject[index].objectWith16x9.SetActive(false);

                if (deviceOptimizedObject[index].objectWith19_5x9 != null)
                    deviceOptimizedObject[index].objectWith19_5x9.SetActive(false);

                if (deviceOptimizedObject[index].objectWith16x10 != null)
                    deviceOptimizedObject[index].objectWith16x10.SetActive(false);

                if (deviceOptimizedObject[index].objectWith17x10 != null)
                    deviceOptimizedObject[index].objectWith17x10.SetActive(false);

            }
            else if (enabledObject == deviceOptimizedObject[index].objectWith4x3)
            {

                if (deviceOptimizedObject[index].objectWith3x2 != null)
                    deviceOptimizedObject[index].objectWith3x2.SetActive(false);

                if (deviceOptimizedObject[index].objectWith16x9 != null)
                    deviceOptimizedObject[index].objectWith16x9.SetActive(false);

                if (deviceOptimizedObject[index].objectWith19_5x9 != null)
                    deviceOptimizedObject[index].objectWith19_5x9.SetActive(false);

                if (deviceOptimizedObject[index].objectWith16x10 != null)
                    deviceOptimizedObject[index].objectWith16x10.SetActive(false);

                if (deviceOptimizedObject[index].objectWith17x10 != null)
                    deviceOptimizedObject[index].objectWith17x10.SetActive(false);

            }
            else if (enabledObject == deviceOptimizedObject[index].objectWith16x9)
            {

                if (deviceOptimizedObject[index].objectWith3x2 != null)
                    deviceOptimizedObject[index].objectWith3x2.SetActive(false);

                if (deviceOptimizedObject[index].objectWith4x3 != null)
                    deviceOptimizedObject[index].objectWith4x3.SetActive(false);

                if (deviceOptimizedObject[index].objectWith19_5x9 != null)
                    deviceOptimizedObject[index].objectWith19_5x9.SetActive(false);

                if (deviceOptimizedObject[index].objectWith16x10 != null)
                    deviceOptimizedObject[index].objectWith16x10.SetActive(false);

                if (deviceOptimizedObject[index].objectWith17x10 != null)
                    deviceOptimizedObject[index].objectWith17x10.SetActive(false);

            }
            else if (enabledObject == deviceOptimizedObject[index].objectWith19_5x9)
            {

                if (deviceOptimizedObject[index].objectWith3x2 != null)
                    deviceOptimizedObject[index].objectWith3x2.SetActive(false);

                if (deviceOptimizedObject[index].objectWith4x3 != null)
                    deviceOptimizedObject[index].objectWith4x3.SetActive(false);

                if (deviceOptimizedObject[index].objectWith16x9 != null)
                    deviceOptimizedObject[index].objectWith16x9.SetActive(false);

                if (deviceOptimizedObject[index].objectWith16x10 != null)
                    deviceOptimizedObject[index].objectWith16x10.SetActive(false);

                if (deviceOptimizedObject[index].objectWith17x10 != null)
                    deviceOptimizedObject[index].objectWith17x10.SetActive(false);

            }
            else if (enabledObject == deviceOptimizedObject[index].objectWith16x10)
            {

                if (deviceOptimizedObject[index].objectWith3x2 != null)
                    deviceOptimizedObject[index].objectWith3x2.SetActive(false);

                if (deviceOptimizedObject[index].objectWith4x3 != null)
                    deviceOptimizedObject[index].objectWith4x3.SetActive(false);

                if (deviceOptimizedObject[index].objectWith16x9 != null)
                    deviceOptimizedObject[index].objectWith16x9.SetActive(false);

                if (deviceOptimizedObject[index].objectWith19_5x9 != null)
                    deviceOptimizedObject[index].objectWith19_5x9.SetActive(false);

                if (deviceOptimizedObject[index].objectWith17x10 != null)
                    deviceOptimizedObject[index].objectWith17x10.SetActive(false);

            }
            else if (enabledObject == deviceOptimizedObject[index].objectWith17x10)
            {

                if (deviceOptimizedObject[index].objectWith3x2 != null)
                    deviceOptimizedObject[index].objectWith3x2.SetActive(false);

                if (deviceOptimizedObject[index].objectWith4x3 != null)
                    deviceOptimizedObject[index].objectWith4x3.SetActive(false);

                if (deviceOptimizedObject[index].objectWith16x9 != null)
                    deviceOptimizedObject[index].objectWith16x9.SetActive(false);

                if (deviceOptimizedObject[index].objectWith19_5x9 != null)
                    deviceOptimizedObject[index].objectWith19_5x9.SetActive(false);

                if (deviceOptimizedObject[index].objectWith16x10 != null)
                    deviceOptimizedObject[index].objectWith16x10.SetActive(false);
            }
        }

        public void EnableDeviceObjects()
        {

            float m_AspectRatioFactor = DeviceInfoManager.Instance.GetAspectRatioFactor();

#if UNITY_IOS

		if(m_AspectRatioFactor >= 2.16){
			// iPhone-10
			
			m_DeviceIndex = 3;
			//OnIPhoneXDevicesEvent.Invoke();
		}else if(m_AspectRatioFactor >= 1.77){
			// iPhone-5, iPhone-5s, iPhone-6/6s, iPhone-6s+/7s+, iPhone-7/7+, iPhone-8/8+
			
			m_DeviceIndex = 2;
			//OnIPhoneDevicesEvent.Invoke();
		}else if(m_AspectRatioFactor >= 1.5){
			// (480x320) iPhone-3, iPhone-4, iPhone-4s
			
			m_DeviceIndex = 0;
			//OnIPhoneLowEndDevicesEvent.Invoke();
		}else if(m_AspectRatioFactor >= 1.33){
			// iPad (All)
			
			m_DeviceIndex = 1;
			//OnIPadDevicesEvent.Invoke();
		}

#elif UNITY_ANDROID

            if (m_AspectRatioFactor >= 1.77f)
            {
                //Android Device : 9x16

                m_DeviceIndex = 2;
                //OnAndroid09x16DevicesEvent.Invoke();
            }
            else if (m_AspectRatioFactor >= 1.70f)
            {
                //Android Device : 10x17

                m_DeviceIndex = 5;
                //OnAndroid10x17DevicesEvent.Invoke();
            }
            else if (m_AspectRatioFactor >= 1.60f)
            {
                //Android Device : 10x16

                m_DeviceIndex = 4;
                //OnAndroid10x16DevicesEvent.Invoke();
            }
            else if (m_AspectRatioFactor >= 1.5f)
            {
                //Android Device : LowEnd

                m_DeviceIndex = 0;
                //OnAndroidLowEndDevicesEvent.Invoke();
            }

#endif

            bool m_IsAssigned = false;
            //Enable Object
            for (int index = 0; index < deviceOptimizedObject.Length; index++)
            {

                m_IsAssigned = false;
                switch (m_DeviceIndex)
                {
                    case 0:
                        if (deviceOptimizedObject[index].objectWith3x2 != null)
                        {

                            DisableOtherObject(deviceOptimizedObject[index].objectWith3x2, index);
                            deviceOptimizedObject[index].objectWith3x2.SetActive(true);
                            m_IsAssigned = true;
                        }
                        deviceOptimizedObject[index].OnIPhoneLowEndDevicesEvent.Invoke();
                        break;
                    case 1:
                        if (deviceOptimizedObject[index].objectWith4x3 != null)
                        {

                            DisableOtherObject(deviceOptimizedObject[index].objectWith4x3, index);
                            deviceOptimizedObject[index].objectWith4x3.SetActive(true);
                            m_IsAssigned = true;
                        }
                        deviceOptimizedObject[index].OnIPadDevicesEvent.Invoke();
                        break;
                    case 2:
                        if (deviceOptimizedObject[index].objectWith16x9 != null)
                        {

                            DisableOtherObject(deviceOptimizedObject[index].objectWith16x9, index);
                            deviceOptimizedObject[index].objectWith16x9.SetActive(true);
                            m_IsAssigned = true;
                        }
                        deviceOptimizedObject[index].OnIPhoneDevicesEvent.Invoke();
                        break;
                    case 3:
                        if (deviceOptimizedObject[index].objectWith19_5x9 != null)
                        {

                            DisableOtherObject(deviceOptimizedObject[index].objectWith19_5x9, index);
                            deviceOptimizedObject[index].objectWith19_5x9.SetActive(true);
                            m_IsAssigned = true;
                        }
                        deviceOptimizedObject[index].OnIPhoneXDevicesEvent.Invoke();
                        break;
                    case 4:
                        if (deviceOptimizedObject[index].objectWith16x10 != null)
                        {

                            DisableOtherObject(deviceOptimizedObject[index].objectWith16x10, index);
                            deviceOptimizedObject[index].objectWith16x10.SetActive(true);
                            m_IsAssigned = true;
                        }
                        deviceOptimizedObject[index].OnAndroid10x16DevicesEvent.Invoke();
                        break;
                    case 5:
                        if (deviceOptimizedObject[index].objectWith17x10 != null)
                        {

                            DisableOtherObject(deviceOptimizedObject[index].objectWith17x10, index);
                            deviceOptimizedObject[index].objectWith17x10.SetActive(true);
                            m_IsAssigned = true;
                        }
                        deviceOptimizedObject[index].OnAndroid10x17DevicesEvent.Invoke();
                        break;
                }

                //If The Specefic Device Doesn't Have THe Image, Selecting Default Image
                if (!m_IsAssigned)
                {

                    if (deviceOptimizedObject[index].objectWith3x2 != null)
                        deviceOptimizedObject[index].objectWith3x2.SetActive(true);
                    else if (deviceOptimizedObject[index].objectWith4x3 != null)
                        deviceOptimizedObject[index].objectWith4x3.SetActive(true);
                    else if (deviceOptimizedObject[index].objectWith16x9 != null)
                        deviceOptimizedObject[index].objectWith16x9.SetActive(true);
                    else if (deviceOptimizedObject[index].objectWith19_5x9 != null)
                        deviceOptimizedObject[index].objectWith19_5x9.SetActive(true);
                    else if (deviceOptimizedObject[index].objectWith16x10 != null)
                        deviceOptimizedObject[index].objectWith16x10.SetActive(true);
                    else if (deviceOptimizedObject[index].objectWith17x10 != null)
                        deviceOptimizedObject[index].objectWith17x10.SetActive(true);
                }
            }
        }

        public void InvokeDeviceEvent()
        {

            for (int index = 0; index < deviceOptimizedObject.Length; index++)
                InvokeDeviceEvent(index);
        }

        public void InvokeDeviceEvent(int objectIndex)
        {

            float m_AspectRatioFactor = DeviceInfoManager.Instance.GetAspectRatioFactor();


#if UNITY_IOS

		if(m_AspectRatioFactor >= 2.16){
			// iPhone-10
			
			m_DeviceIndex = 3;
			deviceOptimizedEvent[objectIndex].OnIPhoneXDevicesEvent.Invoke();
		}else if(m_AspectRatioFactor >= 1.77){
			// iPhone-5, iPhone-5s, iPhone-6/6s, iPhone-6s+/7s+, iPhone-7/7+, iPhone-8/8+
			
			m_DeviceIndex = 2;
			deviceOptimizedEvent[objectIndex].OnIPhoneDevicesEvent.Invoke();
		}else if(m_AspectRatioFactor >= 1.5){
			// (480x320) iPhone-3, iPhone-4, iPhone-4s
			
			m_DeviceIndex = 0;
			deviceOptimizedEvent[objectIndex].OnIPhoneLowEndDevicesEvent.Invoke();
		}else if(m_AspectRatioFactor >= 1.33){
			// iPad (All)
			
			m_DeviceIndex = 1;
			deviceOptimizedEvent[objectIndex].OnIPadDevicesEvent.Invoke();
		}

#elif UNITY_ANDROID

            if (m_AspectRatioFactor >= 1.77f)
            {
                //Android Device : 9x16

                m_DeviceIndex = 2;
                deviceOptimizedEvent[objectIndex].OnIPhoneDevicesEvent.Invoke();
            }
            else if (m_AspectRatioFactor >= 1.70f)
            {
                //Android Device : 10x17

                m_DeviceIndex = 5;
                deviceOptimizedEvent[objectIndex].OnAndroid10x17DevicesEvent.Invoke();
            }
            else if (m_AspectRatioFactor >= 1.60f)
            {
                //Android Device : 10x16

                m_DeviceIndex = 4;
                deviceOptimizedEvent[objectIndex].OnAndroid10x16DevicesEvent.Invoke();
            }
            else if (m_AspectRatioFactor >= 1.5f)
            {
                //Android Device : LowEnd

                m_DeviceIndex = 0;
                deviceOptimizedEvent[objectIndex].OnIPhoneLowEndDevicesEvent.Invoke();
            }

#endif
        }
    }

}

