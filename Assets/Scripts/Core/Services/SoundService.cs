using KNC.Utilities;
using System;
using UnityEngine;

namespace KNC.Core.Services
{
    public class SoundService : GenericMonoSingleton<SoundService>
    {
        public AudioSource soundEffect;
        public AudioSource soundMusic;

        public bool isMute = false;
        public float Volume = 1.0f;
        public SoundItem[] sounds;


        private void OnEnable()
        {
            if (EventService.Instance == null)
                return;

            EventService.Instance.OnKickStarted.AddListener(OnKick);
            EventService.Instance.OnBallCaught.AddListener(OnCaught);
            EventService.Instance.OnBallMissed.AddListener(OnMissed);
        }

        private void Start()
        {
            PlayMusic(Sounds.MUSIC);
            SetVolume(Volume);
        }

        private void SetVolume(float volume)
        {
            soundMusic.volume = volume;
        }

        public void PlayMusic(Sounds sound)
        {
            if (isMute)
                return;

            AudioClip clip = GetSoundClip(sound);

            if (clip != null)
            {
                soundMusic.clip = clip;
                soundMusic.Play();
            }
            else
            {
                Debug.LogError("Clip not found for sound type: " + sound);
            }
        }

        public void Play(Sounds sound)
        {
            if (isMute)
                return;

            AudioClip clip = GetSoundClip(sound);

            if (clip != null)
            {
                soundEffect.PlayOneShot(clip);
            }
            else
            {
                Debug.LogError("Clip not found for sound type: " + sound);
            }
        }

        private AudioClip GetSoundClip(Sounds sound)
        {
            SoundItem item = Array.Find(sounds, i => i.soundTypes == sound);

            if (item != null)
            {
                return item.audioClip;
            }

            return null;
        }



        [Serializable]
        public class SoundItem
        {
            public Sounds soundTypes;
            public AudioClip audioClip;
        }

        public enum Sounds
        {
            BUTTONCLICK,
            GAMEOVER,
            CAUGHT,
            MUSIC,
            KICK
        }

        private void OnDisable()
        {
            if (EventService.Instance == null)
                return;

            EventService.Instance.OnKickStarted.RemoveListener(OnKick);
            EventService.Instance.OnBallCaught.RemoveListener(OnCaught);
            EventService.Instance.OnBallMissed.RemoveListener(OnMissed);
        }

        private void OnKick()
        {
            Play(Sounds.KICK);
        }

        private void OnCaught()
        {
            Play(Sounds.CAUGHT);
        }

        private void OnMissed()
        {
            Play(Sounds.GAMEOVER);
        }
    }
}