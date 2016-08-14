using UnityEngine;
using System;

namespace Framework
{
    /// <summary>
    /// Listens to an AudioSource and calls back when it has finished playing.
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class AudioSourceListener : MonoBehaviour
    {
        public event Action<AudioSourceListener> FinishedPlaying;

        public AudioSource AudioSource { get; private set; }

        private bool _startedPlaying;

        public void Awake()
        {
            AudioSource = GetComponent<AudioSource>();
        }

        public void Update()
        {
            if (AudioSource.isPlaying && !_startedPlaying)
                _startedPlaying = true;

            if (_startedPlaying && !AudioSource.isPlaying)
            {
                FinishedPlaying.InvokeSafe(this);
                _startedPlaying = false;
            }
        }
    }
}
