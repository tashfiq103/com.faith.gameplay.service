namespace com.faith.gameplay_service
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Events;

    #region Custom Data

    public struct NetworkEvent
    {
        public bool isCalledEveryTimeOnNetworkStatusChanged;
        public UnityAction OnNetworkEvent;

        public bool isCalledOnced;
    }

    #endregion

    public class NetworkReachabilityController : MonoBehaviour
    {

        public static NetworkReachabilityController Instance;

        #region Private Variables

        public List<NetworkEvent> m_OnNetworkReachable;
        public List<NetworkEvent> m_OnNetworkUnreachable;

        #endregion

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

            m_OnNetworkReachable = new List<NetworkEvent>();
            m_OnNetworkUnreachable = new List<NetworkEvent>();

            StartNetworkReachabilityStatusController();
        }

        #region Configuretion	:	Check For Internet Avilablity

        private bool m_IsNetworkReachabilityStatusControllerRunning;
        private bool m_IsConnectedWithTheNetwork;

        private bool IsNetworkReachableEventAlreadyAssigned(UnityAction t_RequestedEvent)
        {

            bool t_Result = false;

            int t_NumberOfNetworkReachableEvent = m_OnNetworkReachable.Count;
            for (int eventIndex = 0; eventIndex < t_NumberOfNetworkReachableEvent; eventIndex++)
            {

                if (m_OnNetworkReachable[eventIndex].OnNetworkEvent == t_RequestedEvent)
                {
                    t_Result = true;
                    break;
                }
            }

            return t_Result;
        }

        private bool IsNetworkUnreachableEventAlreadyAssigned(UnityAction t_RequestedEvent)
        {

            bool t_Result = false;

            int t_NumberOfNetworkUnreachableEvent = m_OnNetworkUnreachable.Count;
            for (int eventIndex = 0; eventIndex < t_NumberOfNetworkUnreachableEvent; eventIndex++)
            {

                if (m_OnNetworkUnreachable[eventIndex].OnNetworkEvent == t_RequestedEvent)
                {
                    t_Result = true;
                    break;
                }
            }

            return t_Result;
        }

        private void InvokeNetworkRechableEvent()
        {

            Queue<int> t_IndexCollectorForNonRepitativeEvent = new Queue<int>();
            int t_NumberOfNetworkReachableEvent = m_OnNetworkReachable.Count;

            for (int eventIndex = 0; eventIndex < t_NumberOfNetworkReachableEvent; eventIndex++)
            {

                if (!m_OnNetworkReachable[eventIndex].isCalledOnced)
                {

                    NetworkEvent t_ModifiedNetworkEvent = m_OnNetworkReachable[eventIndex];
                    t_ModifiedNetworkEvent.isCalledOnced = true;
                    m_OnNetworkReachable[eventIndex] = t_ModifiedNetworkEvent;

                    m_OnNetworkReachable[eventIndex].OnNetworkEvent.Invoke();

                    if (!m_OnNetworkReachable[eventIndex].isCalledEveryTimeOnNetworkStatusChanged)
                        t_IndexCollectorForNonRepitativeEvent.Enqueue(eventIndex);

                }
                else if (m_OnNetworkReachable[eventIndex].isCalledEveryTimeOnNetworkStatusChanged)
                {

                    m_OnNetworkReachable[eventIndex].OnNetworkEvent.Invoke();
                }
            }

            //Clear EventList
            int t_IndexError = 0;
            while (t_IndexCollectorForNonRepitativeEvent.Count > 0)
            {
                m_OnNetworkReachable.RemoveAt(t_IndexCollectorForNonRepitativeEvent.Dequeue() - t_IndexError);
                t_IndexError++;
            }
            m_OnNetworkReachable.TrimExcess();
        }

        private void InvokeNetworkUnrechableEvent()
        {

            Queue<int> t_IndexCollectorForNonRepitativeEvent = new Queue<int>();
            int t_NumberOfNetworkUnreachableEvent = m_OnNetworkUnreachable.Count;

            for (int eventIndex = 0; eventIndex < t_NumberOfNetworkUnreachableEvent; eventIndex++)
            {

                if (!m_OnNetworkUnreachable[eventIndex].isCalledOnced)
                {

                    NetworkEvent t_ModifiedNetworkEvent = m_OnNetworkUnreachable[eventIndex];
                    t_ModifiedNetworkEvent.isCalledOnced = true;
                    m_OnNetworkUnreachable[eventIndex] = t_ModifiedNetworkEvent;

                    m_OnNetworkUnreachable[eventIndex].OnNetworkEvent.Invoke();

                    if (!m_OnNetworkUnreachable[eventIndex].isCalledEveryTimeOnNetworkStatusChanged)
                        t_IndexCollectorForNonRepitativeEvent.Enqueue(eventIndex);

                }
                else if (m_OnNetworkUnreachable[eventIndex].isCalledEveryTimeOnNetworkStatusChanged)
                {

                    m_OnNetworkUnreachable[eventIndex].OnNetworkEvent.Invoke();
                }
            }

            //Clear EventList
            int t_IndexError = 0;
            while (t_IndexCollectorForNonRepitativeEvent.Count > 0)
            {
                m_OnNetworkUnreachable.RemoveAt(t_IndexCollectorForNonRepitativeEvent.Dequeue() - t_IndexError);
                t_IndexError++;
            }
            m_OnNetworkUnreachable.TrimExcess();
        }

        private void ToggleNetworkStatus()
        {
            m_IsConnectedWithTheNetwork = !m_IsConnectedWithTheNetwork;
        }

        private IEnumerator NetworkReachabilityStatusController()
        {

            m_IsConnectedWithTheNetwork = false;

            WaitForSeconds t_CycleDelay = new WaitForSeconds(1f);

            while (m_IsNetworkReachabilityStatusControllerRunning)
            {

                if (Application.internetReachability != NetworkReachability.NotReachable)
                {
                    //Network Rechable

                    if (!IsConnectedWithTheNetwork())
                    {

                        InvokeNetworkRechableEvent();
                        ToggleNetworkStatus();
                    }

                }
                else
                {

                    if (IsConnectedWithTheNetwork())
                    {

                        InvokeNetworkUnrechableEvent();
                        ToggleNetworkStatus();
                    }
                }

                yield return t_CycleDelay;
            }
        }

        #endregion

        #region Public Callback	:	General

        public bool IsConnectedWithTheNetwork()
        {
            return m_IsConnectedWithTheNetwork;
        }

        public void StartNetworkReachabilityStatusController()
        {

            m_IsNetworkReachabilityStatusControllerRunning = true;
            StartCoroutine(NetworkReachabilityStatusController());
        }

        public void StopNetworkReachabilityStatusController()
        {

            m_IsNetworkReachabilityStatusControllerRunning = false;
        }

        #endregion

        //------------------------------------------------------------------------------------------
        #region Public Callback	:	EventForConnectedWithTheNetwork

        /// <summary>
        /// Add : Event when connected with the network
        /// </summary>
        /// <param name="t_NetworkReachableEvent"></param>
        public void AddNetworkReachableEvent(UnityAction t_NetworkReachableEvent)
        {
            AddNetworkReachableEvent(t_NetworkReachableEvent, false, false);
        }

        /// <summary>
        /// Add : Event when connected with the network
        /// </summary>
        /// <param name="t_NetworkReachableEvent"></param>
        /// <param name="invokeImmediateIfAlreadyConnectedWithNetwork"> true : invoke if it's already connected with the network and then add to the network list </param>
        public void AddNetworkReachableEvent(UnityAction t_NetworkReachableEvent, bool invokeImmediateIfAlreadyConnectedWithNetwork)
        {

            AddNetworkReachableEvent(t_NetworkReachableEvent, invokeImmediateIfAlreadyConnectedWithNetwork, false);
        }

        /// <summary>
        /// Add : Event when connected with the network
        /// </summary>
        /// <param name="t_NetworkReachableEvent"></param>
        /// <param name="invokeImmediateIfAlreadyConnectedWithNetwork"> true : invoke if it's already connected with the network and then add to the network list </param>
        public void AddNetworkReachableEvent(UnityAction t_NetworkReachableEvent, bool invokeImmediateIfAlreadyConnectedWithNetwork, bool invokeEveryTimeItConnectedWithNetwork)
        {

            if (!IsNetworkReachableEventAlreadyAssigned(t_NetworkReachableEvent))
            {
                //Check : If the event is already registered

                bool t_IsCalledImmediate = false;

                if (invokeImmediateIfAlreadyConnectedWithNetwork &&
                    Application.internetReachability != NetworkReachability.NotReachable)
                {
                    // If : Already Connected With The Network

                    t_IsCalledImmediate = true;
                    t_NetworkReachableEvent.Invoke();
                }

                if ((invokeImmediateIfAlreadyConnectedWithNetwork && t_IsCalledImmediate) && !invokeEveryTimeItConnectedWithNetwork)
                {
                    //Not Include Event : As it supposed to called once and it already got called
                }
                else
                {

                    NetworkEvent t_NewNetworkEvent = new NetworkEvent();
                    t_NewNetworkEvent.OnNetworkEvent = t_NetworkReachableEvent;
                    t_NewNetworkEvent.isCalledOnced = t_IsCalledImmediate;
                    t_NewNetworkEvent.isCalledEveryTimeOnNetworkStatusChanged = invokeEveryTimeItConnectedWithNetwork;

                    m_OnNetworkReachable.Add(t_NewNetworkEvent);
                }

            }
            else if (invokeImmediateIfAlreadyConnectedWithNetwork && Application.internetReachability != NetworkReachability.NotReachable)
            {
                // if : already registered in the event list, but requested to invoke immediate of the network is connected
                t_NetworkReachableEvent.Invoke();
            }
            else
            {

                Debug.LogError("CE : The following event already been added to  the network");
            }

        }

        #endregion

        //------------------------------------------------------------------------------------------
        #region Public Callback	:	EventForDisconnectedWithTheNetwork

        public void AddNetworkUnreachableEvent(UnityAction t_NetworkUnreachableEvent)
        {
            AddNetworkUnreachableEvent(t_NetworkUnreachableEvent, false, false);
        }

        public void AddNetworkUnreachableEvent(UnityAction t_NetworkUnreachableEvent, bool invokeImmediateIfAlreadyDisconnectedFromNetwork)
        {
            AddNetworkUnreachableEvent(t_NetworkUnreachableEvent, invokeImmediateIfAlreadyDisconnectedFromNetwork, false);
        }

        public void AddNetworkUnreachableEvent(UnityAction t_NetworkUnreachableEvent, bool invokeImmediateIfAlreadyDisconnectedFromNetwork, bool invokeEveryTimeItDisconnectedFromNetwork)
        {

            if (!IsNetworkUnreachableEventAlreadyAssigned(t_NetworkUnreachableEvent))
            {
                //Check : If the event is already registered

                bool t_IsCalledImmediate = false;

                if (invokeImmediateIfAlreadyDisconnectedFromNetwork &&
                    Application.internetReachability == NetworkReachability.NotReachable)
                {
                    // If : Already Connected With The Network

                    t_IsCalledImmediate = true;
                    t_NetworkUnreachableEvent.Invoke();
                }

                if ((invokeImmediateIfAlreadyDisconnectedFromNetwork && t_IsCalledImmediate) && !invokeEveryTimeItDisconnectedFromNetwork)
                {
                    //Not Include Event : As it supposed to called once and it already got called
                }
                else
                {

                    NetworkEvent t_NewNetworkEvent = new NetworkEvent();
                    t_NewNetworkEvent.OnNetworkEvent = t_NetworkUnreachableEvent;
                    t_NewNetworkEvent.isCalledOnced = t_IsCalledImmediate;
                    t_NewNetworkEvent.isCalledEveryTimeOnNetworkStatusChanged = invokeEveryTimeItDisconnectedFromNetwork;

                    m_OnNetworkUnreachable.Add(t_NewNetworkEvent);
                }

            }
            else if (invokeImmediateIfAlreadyDisconnectedFromNetwork && Application.internetReachability == NetworkReachability.NotReachable)
            {
                // if : already registered in the event list, but requested to invoke immediate of the network is disconnected
                t_NetworkUnreachableEvent.Invoke();
            }
            else
            {

                Debug.LogError("CE : The following event already been added to  the network");
            }
        }

        #endregion
    }
}

