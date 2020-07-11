namespace com.faith.GameplayService
{
    using System.Collections;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.Video;

    [RequireComponent(typeof(VideoPlayer))]
    public class VideoPlayerController : MonoBehaviour
    {

        public UnityEvent OnVideoEnd;
        private VideoPlayer m_VideoPlayerReference;

        void Start()
        {

            m_VideoPlayerReference = gameObject.GetComponent<VideoPlayer>();
            StartCoroutine(WaitForVideoEnd());
        }

        private IEnumerator WaitForVideoEnd()
        {

            m_VideoPlayerReference.Play();

            yield return new WaitForSeconds(2.5f);

            while (true)
            {

                if (m_VideoPlayerReference.isPlaying)
                {

                    yield return new WaitForEndOfFrame();
                }
                else
                {

                    break;
                }
            }
            StopCoroutine(WaitForVideoEnd());
            OnVideoEnd.Invoke();
        }
    }
}


