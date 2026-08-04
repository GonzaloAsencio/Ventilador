using System;
using System.Collections.Generic;
using UnityEngine;

namespace Fan.Audio
{
    /// <summary>
    /// Singleton persistente para reproducir SFX y música desde cualquier script,
    /// sin que cada uno maneje su propio AudioSource.
    /// </summary>
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        [Serializable]
        public struct SonidoEntry
        {
            public string Id;
            public AudioClip Clip;
        }

        [Header("Fuentes de audio")]
        [SerializeField] private AudioSource _fuenteSFX;
        [SerializeField] private AudioSource _fuenteMusica;

        [Header("Biblioteca de SFX (acceso por id)")]
        [SerializeField] private List<SonidoEntry> _sfxLibrary = new();

        private Dictionary<string, AudioClip> _sfxPorId;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            _sfxPorId = new Dictionary<string, AudioClip>(_sfxLibrary.Count);
            foreach (var entry in _sfxLibrary)
            {
                if (!string.IsNullOrEmpty(entry.Id) && entry.Clip != null)
                    _sfxPorId[entry.Id] = entry.Clip;
            }
        }

        public void PlaySFX(AudioClip clip)
        {
            if (clip == null || _fuenteSFX == null) return;
            _fuenteSFX.PlayOneShot(clip);
        }

        public void PlaySFX(string id)
        {
            if (_sfxPorId.TryGetValue(id, out var clip))
                PlaySFX(clip);
            else
                Debug.LogWarning($"[AudioManager] No se encontró un SFX con id '{id}'.");
        }

        public void PlayMusic(AudioClip clip, bool loop = true)
        {
            if (clip == null || _fuenteMusica == null) return;
            _fuenteMusica.clip = clip;
            _fuenteMusica.loop = loop;
            _fuenteMusica.Play();
        }

        public void StopMusic()
        {
            if (_fuenteMusica != null)
                _fuenteMusica.Stop();
        }

        public void SetVolumenGeneral(float valor01)
        {
            valor01 = Mathf.Clamp01(valor01);
            if (_fuenteSFX != null) _fuenteSFX.volume = valor01;
            if (_fuenteMusica != null) _fuenteMusica.volume = valor01;
        }
    }
}
