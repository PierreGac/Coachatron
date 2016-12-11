using UnityEngine;

namespace CoachSimulator
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager instance = null;

        public AudioSource source01, source02, source03, uiSource, sourceCoach, music, ambient;
        [Space(30)]
        public AudioClip ploup;

        public AudioClip[] ambientClips;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public AudioSource GetAvailableAudioSource()
        {
            if(!source01.isPlaying)
            {
                return source01;
            }
            if(!source02.isPlaying)
            {
                return source02;
            }
            if(!source03.isPlaying)
            {
                return source03;
            }


            return source01;
        }

        public void PlayAmbient()
        {
            ambient.clip = ambientClips[Random.Range(0, ambientClips.Length)];
            ambient.Play();
        }

        public void StopAmbient()
        {
            ambient.Stop();
        }

        public static void Play(AudioClip clip)
        {
            AudioSource source = instance.GetAvailableAudioSource();
            source.clip = clip;
            source.Play();
        }

        public static void PlayCoach(AudioClip clip, bool loop = false)
        {
            instance.sourceCoach.clip = clip;
            instance.sourceCoach.loop = loop;

            instance.sourceCoach.Play();
        }

        /// <summary>
        /// Plays an audio clip designed for the UI
        /// </summary>
        /// <param name="clip"></param>
        public static void PlayUI(AudioClip clip)
        {
            instance.uiSource.clip = clip;

            instance.uiSource.Play();
        }

        public static void PlayPloup()
        {
            instance.uiSource.clip = instance.ploup;

            instance.uiSource.Play();
        }
    }
}