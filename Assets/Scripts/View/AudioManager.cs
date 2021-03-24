using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RectangleTrainer.ChromaTower.View
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] AudioClip bounce;
        [SerializeField] AudioClip damage;

        private AudioSource source;

        private void Awake()
        {
            source = GetComponent<AudioSource>();
        }

        public void Initialize(Engine.ChromaTower tower)
        {
            tower.OnDamage += PlayDamage;
            tower.OnSuccessfulHit += PlayBounce;
        }

        private void PlayClip(AudioClip clip)
        {
            if (source.isPlaying) source.Stop();
            source.clip = clip;
            source.Play();
        }

        public void PlayBounce() => PlayClip(bounce);
        public void PlayDamage() => PlayClip(damage);
    }
}
