namespace com.faith.gameplay.service
{
    using UnityEditor;
    using UnityEngine;

    [CustomEditor(typeof(AudioManager))]
    public class AudioManagerEditor : Editor
    {

        AudioManager AudioManagerReference;

        private int inputNumberOfAudioSource;

        private void OnEnable()
        {

            AudioManagerReference = (AudioManager)target;
        }

        public override void OnInspectorGUI()
        {

            serializedObject.Update();

            //DrawCustonGUI();
            DrawMusicAndSoundFXGUI();
            DrawDefaultInspector();

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawMusicAndSoundFXGUI()
        {

            EditorGUILayout.BeginHorizontal();
            {

                if (AudioManagerReference.IsMusicEnabled())
                {

                    if (GUILayout.Button("(Disabled) Music"))
                    {

                        AudioManagerReference.DisableMusic();
                    }
                }
                else
                {

                    if (GUILayout.Button("(Enabled) Music"))
                    {

                        AudioManagerReference.EnableMusic();
                    }
                }

                if (AudioManagerReference.IsSoundFXEnabled())
                {

                    if (GUILayout.Button("(Disabled) SoundFX"))
                    {

                        AudioManagerReference.DisableSoundFX();
                    }
                }
                else
                {

                    if (GUILayout.Button("(Enabled) SoundFX"))
                    {

                        AudioManagerReference.EnableSoundFX();
                    }
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        private void DrawCustonGUI()
        {

            EditorGUILayout.BeginHorizontal();
            {
                inputNumberOfAudioSource = EditorGUILayout.IntField(
                    "AudioSource",
                    inputNumberOfAudioSource
                );
                if (GUILayout.Button("Create AudioSource"))
                {

                    AudioClipAttribute[] m_Backup = null;

                    if (AudioManagerReference.audio != null)
                    {
                        m_Backup = AudioManagerReference.audio;
                    }

                    AudioManagerReference.audio = new AudioClipAttribute[inputNumberOfAudioSource];

                    if (AudioManagerReference.audio != null)
                    {

                        for (int audioIndex = 0; audioIndex < inputNumberOfAudioSource; audioIndex++)
                        {

                            AudioManagerReference.audio[audioIndex] = m_Backup[audioIndex];
                        }
                    }
                }
            }
            EditorGUILayout.EndHorizontal();

            if (AudioManagerReference.audio != null)
            {

                for (int audioIndex = 0; audioIndex < AudioManagerReference.audio.Length; audioIndex++)
                {

                    AudioManagerReference.audio[audioIndex].usePreloadedAudioSource = EditorGUILayout.Toggle(
                        "PreLoaded Sound",
                        AudioManagerReference.audio[audioIndex].usePreloadedAudioSource
                    );
                }
            }
        }
    }
}

