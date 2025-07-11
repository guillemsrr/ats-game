// Copyright (c) Guillem Serra. All Rights Reserved.

using System.Collections;
using UnityEngine;

namespace Audio
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private AudioClip _menuAudioClip;
        [SerializeField] private AudioClip _inGameAudioClip;

        private AudioSource _menuAudioSource;
        private AudioSource _inGameAudioSource;

        [SerializeField] private float _fadeDuration = 5f;

        public static AudioManager Instance;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            _menuAudioSource = gameObject.AddComponent<AudioSource>();
            _inGameAudioSource = gameObject.AddComponent<AudioSource>();

            _menuAudioSource.loop = true;
            _inGameAudioSource.loop = true;
        }

        private void Start()
        {
            PlayMenuMusic();
        }

        public void PlayMenuMusic()
        {
            FadeOut(_inGameAudioSource);

            if (!_menuAudioSource.isPlaying)
            {
                FadeIn(_menuAudioSource, _menuAudioClip, 0.5f);
            }
        }

        public void PlayInGameMusic()
        {
            FadeOut(_menuAudioSource);

            if (!_inGameAudioSource.isPlaying)
            {
                FadeIn(_inGameAudioSource, _inGameAudioClip, 0.8f);
            }
        }

        private void FadeIn(AudioSource audioSource, AudioClip newClip, float volume)
        {
            audioSource.clip = newClip;
            audioSource.volume = 0f;
            audioSource.Play();

            StartCoroutine(FadeRoutine(audioSource, volume));
        }

        private void FadeOut(AudioSource audioSource)
        {
            StartCoroutine(FadeRoutine(audioSource, 0f));
        }

        private IEnumerator FadeRoutine(AudioSource audioSource, float fadeTarget)
        {
            float startVolume = audioSource.volume;
            float elapsedTime = 0f;

            while (elapsedTime < _fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                float percentageComplete = elapsedTime / _fadeDuration;
                audioSource.volume = Mathf.Lerp(startVolume, fadeTarget, percentageComplete);
                yield return null;
            }

            audioSource.volume = fadeTarget;
            if (fadeTarget == 0f)
            {
                audioSource.Stop();
            }
        }
    }
}