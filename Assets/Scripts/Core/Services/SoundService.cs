using System;
using KNC.Utilities;
using UnityEngine;

namespace KNC.Core.Services
{
    public class SoundService : GenericMonoSingleton<SoundService>
    {
        [SerializeField] private AudioSource soundEffect;
        [SerializeField] private AudioSource soundMusic;
        [SerializeField] private SoundItem[] sounds;

        public bool isMute;

        private void OnEnable()
        {
            var es = EventService.Instance;
            if (es == null) return;

            es.OnKickStarted.AddListener(OnKick);
            es.OnBallCaught.AddListener(OnCaught);
            es.OnBallMissed.AddListener(OnMissed);
        }

        private void Start()
        {
            PlayMusic(Sounds.MUSIC);
        }

        private void OnDisable()
        {
            var es = EventService.Instance;
            if (es == null) return;

            es.OnKickStarted.RemoveListener(OnKick);
            es.OnBallCaught.RemoveListener(OnCaught);
            es.OnBallMissed.RemoveListener(OnMissed);
        }

        public void PlayMusic(Sounds sound)
        {
            if (isMute) return;

            var clip = GetClip(sound);
            if (clip == null) return;

            soundMusic.clip = clip;
            soundMusic.Play();
        }

        public void Play(Sounds sound)
        {
            if (isMute) return;

            var clip = GetClip(sound);
            if (clip == null) return;

            soundEffect.PlayOneShot(clip);
        }

        private AudioClip GetClip(Sounds sound)
        {
            foreach (var s in sounds)
                if (s.soundTypes == sound)
                    return s.audioClip;

            return null;
        }

        private void OnKick() => Play(Sounds.KICK);
        private void OnCaught() => Play(Sounds.CAUGHT);
        private void OnMissed() => Play(Sounds.GAMEOVER);

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
    }
}
