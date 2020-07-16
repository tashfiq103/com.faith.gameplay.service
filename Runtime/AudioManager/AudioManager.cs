namespace com.faith.gameplay.service
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    [System.Serializable]
    public struct AudioClipAttribute
    {

        [HideInInspector]
        public string name;
        [HideInInspector]
        public bool usePreloadedAudioSource;
        [Range(0.0f, 1.0f)]
        public float volumn;
        public AudioClip audioClip;

        [Space(5.0f)]
        [Header("Sound Type")]
        public bool IsMusic;
        public bool IsSoundFX;
    }

    [RequireComponent(typeof(AudioListener))]
    public class AudioManager : MonoBehaviour
    {

        public static AudioManager Instance;

        //----------
        #region Custom Variables

        [System.Serializable]
        public struct AudioSourceAttribute
        {
            public GameObject audioSourceObject;
            public AudioSource audioSourceReference;
            public float autoDestructTime;
        }

        #endregion

        //----------
        #region Public Variables

        [Range(0.0f, 5.0f)]
        public float lifeCycleCheckDuration;
        public AudioClipAttribute[] audio;

        #endregion

        //----------
        #region Private Variables

        private string IS_GAME_MUSIC_DISABLED = "GAME_MUSIC_CONTROLLER";
        private string IS_GAME_SOUNDFX_DISABLED = "GAME_SOUNDFX_CONTROLLER";

        private bool m_RunASLCController;
        private bool m_ResetAudioSourceOnChangedScene;
        private bool m_OnMainSceneLoaded;

        private int m_NumberOfPreloadedAudio;

        private List<AudioSourceAttribute> m_AudioSourceList;

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

            AudioListener m_AudioListener = gameObject.GetComponent<AudioListener>();
            if (m_AudioListener == null)
                gameObject.AddComponent<AudioListener>();

            PreProcess();
        }

        #region Configuretion

        void OnSceneChanged(Scene m_PreviousScene, Scene m_CurrentScene)
        {

            //if (m_ResetAudioSourceOnChangedScene) {

            //	m_ResetAudioSourceOnChangedScene = false;
            //	ResetAudioSource ();
            //}
            ResetAudioSource();
        }

        private void PreProcess()
        {

            m_ResetAudioSourceOnChangedScene = true;

            if (!m_OnMainSceneLoaded)
                m_OnMainSceneLoaded = true;
            else
                SceneManager.activeSceneChanged += OnSceneChanged;

            m_AudioSourceList = new List<AudioSourceAttribute>();

            for (int audioClipIndex = 0; audioClipIndex < audio.Length; audioClipIndex++)
            {

                audio[audioClipIndex].name = audio[audioClipIndex].audioClip.name;
                audio[audioClipIndex].audioClip.LoadAudioData();

                if (!audio[audioClipIndex].IsMusic && !audio[audioClipIndex].IsSoundFX)
                    audio[audioClipIndex].IsMusic = true;
            }

            LoadDefaultAudioSource();
            StartASLCController();
        }

        private void LoadDefaultAudioSource()
        {

            List<int> m_ListOfPreloadedAudioSourceIndex = new List<int>();
            for (int index = 0; index < audio.Length; index++)
            {

                if (audio[index].usePreloadedAudioSource)
                    m_ListOfPreloadedAudioSourceIndex.Add(index);
            }

            int m_PreloadedAudioSourceIndex = 0;
            m_NumberOfPreloadedAudio = m_ListOfPreloadedAudioSourceIndex.Count;

            for (int index = 0; index < m_NumberOfPreloadedAudio; index++)
            {

                GameObject m_NewAudioSource = new GameObject();
                m_NewAudioSource.name = "Audio Source (" + m_AudioSourceList.Count + ")";
                m_NewAudioSource.transform.parent = transform;
                m_NewAudioSource.AddComponent<AudioSource>();

                //Adding to AudioSource List
                AudioSourceAttribute m_NewAudioSourceAttribute = new AudioSourceAttribute();
                m_NewAudioSourceAttribute.audioSourceObject = m_NewAudioSource;
                m_NewAudioSourceAttribute.audioSourceReference = m_NewAudioSource.GetComponent<AudioSource>();
                m_NewAudioSourceAttribute.audioSourceReference.playOnAwake = false;

                if (m_PreloadedAudioSourceIndex < m_ListOfPreloadedAudioSourceIndex.Count)
                {
                    m_NewAudioSourceAttribute.audioSourceReference.clip = audio[m_ListOfPreloadedAudioSourceIndex[m_PreloadedAudioSourceIndex]].audioClip;
                    m_NewAudioSourceAttribute.audioSourceReference.volume = audio[m_ListOfPreloadedAudioSourceIndex[m_PreloadedAudioSourceIndex]].volumn;
                    m_PreloadedAudioSourceIndex++;
                }

                m_AudioSourceList.Add(m_NewAudioSourceAttribute);
            }
        }

        private void StartASLCController()
        {

            m_RunASLCController = true;
            StartCoroutine(AudioSourceLifeCycleOpertaion());
        }

        private void StopASLCController()
        {

            m_RunASLCController = false;
        }

        private IEnumerator AudioSourceLifeCycleOpertaion()
        {

            while (m_RunASLCController)
            {

                yield return new WaitForSeconds(lifeCycleCheckDuration);
                for (int audioSourceIndex = m_NumberOfPreloadedAudio; audioSourceIndex < m_AudioSourceList.Count; audioSourceIndex++)
                {

                    if (m_AudioSourceList.Count > m_NumberOfPreloadedAudio &&
                        Time.time > m_AudioSourceList[audioSourceIndex].autoDestructTime)
                    {

                        if (m_AudioSourceList[audioSourceIndex].audioSourceReference.loop &&
                            IsMusicEnabled())
                        {

                            AudioSourceAttribute t_ModifiedAudioSourceAttribute = m_AudioSourceList[audioSourceIndex];
                            t_ModifiedAudioSourceAttribute.autoDestructTime = Time.time + t_ModifiedAudioSourceAttribute.audioSourceReference.clip.length + (lifeCycleCheckDuration / 3.0f);
                            m_AudioSourceList[audioSourceIndex] = t_ModifiedAudioSourceAttribute;

                        }
                        else
                        {

                            Destroy(m_AudioSourceList[audioSourceIndex].audioSourceObject);
                            m_AudioSourceList.RemoveAt(audioSourceIndex);
                            m_AudioSourceList.TrimExcess();
                            break;
                        }
                    }
                }
            }

            StopCoroutine(AudioSourceLifeCycleOpertaion());
        }

        private int GetAudioClipIndex(string name)
        {

            for (int audioIndex = 0; audioIndex < audio.Length; audioIndex++)
            {

                if (audio[audioIndex].audioClip.name == name)
                {

                    return audioIndex;
                }
            }

            Debug.LogError("Invalid AudioClip name : " + name);
            return -1;
        }

        private bool IsPlayingSoundAllowed(string name)
        {

            /* 
            if (!GameManager.Instance.IsGamePaused ()) {

                int m_AudioIndex = GetAudioClipIndex (name);
                if ((audio[m_AudioIndex].IsMusic && IsMusicEnabled ()) ||
                    (audio[m_AudioIndex].IsSoundFX && IsSoundFXEnabled ())) {

                    return true;
                } else {

                    return false;
                }
            }

            return false;
            */

            int m_AudioIndex = GetAudioClipIndex(name);
            if ((audio[m_AudioIndex].IsMusic && IsMusicEnabled()) ||
                (audio[m_AudioIndex].IsSoundFX && IsSoundFXEnabled()))
            {

                return true;
            }
            else
            {

                return false;
            }

        }

        #endregion

        //----------
        #region Public Callback : Music/SoundFX Controller

        public bool IsMusicEnabled()
        {

            if (PlayerPrefs.GetInt(IS_GAME_MUSIC_DISABLED, 0) == 0)
                return true;
            else return false;
        }

        public void EnableMusic()
        {

            PlayerPrefs.SetInt(IS_GAME_MUSIC_DISABLED, 0);
        }

        public void DisableMusic()
        {

            PlayerPrefs.SetInt(IS_GAME_MUSIC_DISABLED, 1);

            int m_AudioIndex = -1;
            for (int audioSourceIndex = 0; audioSourceIndex < m_AudioSourceList.Count; audioSourceIndex++)
            {

                m_AudioIndex = GetAudioClipIndex(m_AudioSourceList[audioSourceIndex].audioSourceReference.clip.name);

                if (m_AudioIndex != -1 && audio[m_AudioIndex].IsMusic)
                {

                    m_AudioSourceList[audioSourceIndex].audioSourceReference.Stop();

                    if (!audio[m_AudioIndex].usePreloadedAudioSource)
                    {

                        AudioSourceAttribute t_ModifiedAudioSourceAttribute = m_AudioSourceList[audioSourceIndex];
                        t_ModifiedAudioSourceAttribute.autoDestructTime = Time.time;
                        m_AudioSourceList[audioSourceIndex] = t_ModifiedAudioSourceAttribute;
                    }
                }
            }
        }

        public bool IsSoundFXEnabled()
        {

            if (PlayerPrefs.GetInt(IS_GAME_SOUNDFX_DISABLED, 0) == 0)
                return true;
            else return false;
        }

        public void EnableSoundFX()
        {

            PlayerPrefs.SetInt(IS_GAME_SOUNDFX_DISABLED, 0);
        }

        public void DisableSoundFX()
        {

            PlayerPrefs.SetInt(IS_GAME_SOUNDFX_DISABLED, 1);

            for (int audioSourceIndex = 0; audioSourceIndex < m_AudioSourceList.Count; audioSourceIndex++)
            {

                if (audio[GetAudioClipIndex(m_AudioSourceList[audioSourceIndex].audioSourceReference.clip.name)].IsSoundFX)
                {

                    m_AudioSourceList[audioSourceIndex].audioSourceReference.Stop();
                }
            }
        }

        #endregion

        //----------
        #region Public Callback : Play/Stop & Reset AudioSource

        public bool IsSoundAlreadyPlayingInAnyAudioSource(AudioClip soundClip)
        {

            bool t_Result = false;

            int t_AudioClipIndex = -1;

            for (int audioClipIndex = 0; audioClipIndex < audio.Length; audioClipIndex++)
            {

                if (soundClip == audio[audioClipIndex].audioClip)
                {

                    t_AudioClipIndex = audioClipIndex;
                    break;
                }
            }

            if (t_AudioClipIndex != -1)
            {

                for (int audioSourceIndex = 0; audioSourceIndex < m_AudioSourceList.Count; audioSourceIndex++)
                {

                    if (m_AudioSourceList[audioSourceIndex].audioSourceReference.clip == audio[t_AudioClipIndex].audioClip)
                    {
                        t_Result = true;
                        break;
                    }
                }
            }
            else
            {
                Debug.LogError("Invalid AudioSource");
            }

            return t_Result;
        }

        public void ResetAudioSource()
        {

            for (int audioSourcesIndex = 0; audioSourcesIndex < m_AudioSourceList.Count; audioSourcesIndex++)
            {

                m_AudioSourceList[audioSourcesIndex].audioSourceReference.Stop();
                Destroy(m_AudioSourceList[audioSourcesIndex].audioSourceObject);
                m_AudioSourceList.RemoveAt(audioSourcesIndex);
                m_AudioSourceList.TrimExcess();
            }
            m_AudioSourceList.Clear();
        }

        public void StopSound(AudioClip soundClip)
        {

            StopSound(soundClip.name);
        }

        public void StopSound(string name)
        {

            int m_AudioClipIndex = -1;

            //Find AudioClip
            for (int audioClipIndex = 0; audioClipIndex < audio.Length; audioClipIndex++)
            {

                if (name == audio[audioClipIndex].name)
                {

                    m_AudioClipIndex = audioClipIndex;
                    break;
                }
            }

            if (m_AudioClipIndex == -1)
            {

                Debug.LogError(name + " : The following sound clip wasn't added to AudioManager, please add it to audio manager");
            }
            else
            {

                for (int audioSourceIndex = 0; audioSourceIndex < m_AudioSourceList.Count; audioSourceIndex++)
                {

                    if (m_AudioSourceList[audioSourceIndex].audioSourceReference.clip != null &&
                        m_AudioSourceList[audioSourceIndex].audioSourceReference.clip.name == name)
                    {

                        m_AudioSourceList[audioSourceIndex].audioSourceReference.Stop();
                        m_AudioSourceList[audioSourceIndex].audioSourceReference.loop = false;
                        /* if (!audio[m_AudioClipIndex].usePreloadedAudioSource) {

                            m_AudioSourceList[audioSourceIndex].audioSourceReference.loop = false;
                        } */
                    }
                }
            }
        }

        public void PlaySound(AudioClip soundClip)
        {

            PlaySound(soundClip.name, false);
        }
        public void PlaySound(AudioClip soundClip, bool loop)
        {

            PlaySound(soundClip.name, loop);
        }

        public void PlaySound(string name)
        {

            PlaySound(name, false);
        }

        public void PlaySound(string name, bool loop)
        {

            //Debug.Log ("##Sound " + (loop ? "(Loop) " : " ") + name);

            if (IsPlayingSoundAllowed(name))
            {

                bool m_FoundFreeAudioSource = false;
                int m_FreeAudioSourceIndex = -1;

                int m_AudioClipIndex = -1;

                //Find AudioClip
                for (int audioClipIndex = 0; audioClipIndex < audio.Length; audioClipIndex++)
                {

                    if (name == audio[audioClipIndex].name)
                    {

                        m_AudioClipIndex = audioClipIndex;
                        break;
                    }
                }

                if (m_AudioClipIndex == -1)
                {

                    Debug.LogError(name + " : The following sound clip wasn't added to AudioManager, please add it to audio manager");
                }
                else
                {

                    //Find Free Audio Source
                    if (audio[m_AudioClipIndex].usePreloadedAudioSource)
                    {

                        //If Preloaded Audio Source
                        for (int audioSourceIndex = 0; audioSourceIndex < m_AudioSourceList.Count; audioSourceIndex++)
                        {

                            if (m_AudioSourceList[audioSourceIndex].audioSourceReference.clip == audio[m_AudioClipIndex].audioClip)
                            {
                                m_AudioSourceList[audioSourceIndex].audioSourceReference.loop = loop;
                                m_AudioSourceList[audioSourceIndex].audioSourceReference.Play();
                                break;
                            }
                        }
                    }
                    else
                    {

                        //if Not Preloaded Audio Source
                        for (int audioSourceIndex = m_NumberOfPreloadedAudio; audioSourceIndex < m_AudioSourceList.Count; audioSourceIndex++)
                        {

                            if (!m_AudioSourceList[audioSourceIndex].audioSourceReference.isPlaying)
                            {

                                m_FreeAudioSourceIndex = audioSourceIndex;
                                m_FoundFreeAudioSource = true;
                                break;
                            }
                        }

                        if (!m_FoundFreeAudioSource)
                        {

                            GameObject m_NewAudioSource = new GameObject();
                            m_NewAudioSource.name = "Audio Source (" + m_AudioSourceList.Count + ")";
                            m_NewAudioSource.transform.parent = transform;
                            m_NewAudioSource.AddComponent<AudioSource>();

                            //Adding to AudioSource List
                            AudioSourceAttribute m_NewAudioSourceAttribute = new AudioSourceAttribute();
                            m_NewAudioSourceAttribute.audioSourceObject = m_NewAudioSource;
                            m_NewAudioSourceAttribute.audioSourceReference = m_NewAudioSource.GetComponent<AudioSource>();
                            m_NewAudioSourceAttribute.audioSourceReference.playOnAwake = false;

                            m_AudioSourceList.Add(m_NewAudioSourceAttribute);
                            m_FreeAudioSourceIndex = m_AudioSourceList.Count - 1;
                        }

                        AudioSourceAttribute m_ModifiedSourceAttribute = new AudioSourceAttribute();
                        m_ModifiedSourceAttribute = m_AudioSourceList[m_FreeAudioSourceIndex];
                        m_ModifiedSourceAttribute.autoDestructTime = Time.time + audio[m_AudioClipIndex].audioClip.length + (lifeCycleCheckDuration / 3.0f);
                        m_AudioSourceList[m_FreeAudioSourceIndex] = m_ModifiedSourceAttribute;

                        m_AudioSourceList[m_FreeAudioSourceIndex].audioSourceReference.clip = audio[m_AudioClipIndex].audioClip;
                        m_AudioSourceList[m_FreeAudioSourceIndex].audioSourceReference.volume = audio[m_AudioClipIndex].volumn;
                        if (loop)
                        {

                            m_AudioSourceList[m_FreeAudioSourceIndex].audioSourceReference.loop = loop;
                        }

                        m_AudioSourceList[m_FreeAudioSourceIndex].audioSourceReference.Play();
                    }
                }
            }
        }

        #endregion
    }
}

