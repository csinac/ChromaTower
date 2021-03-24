using UnityEngine;

namespace RectangleTrainer.ChromaTower.View
{
    [RequireComponent(typeof(ParticleSystem))]
    public class HitParticles : MonoBehaviour
    {
        [SerializeField] private float lifetime = 1;
        private ParticleSystem pSystem;


        private void Awake()
        {
            pSystem = GetComponent<ParticleSystem>();
        }

        private void Start()
        {
            Invoke("SelfDestruct", lifetime);
        }

        private void SelfDestruct()
        {
            Destroy(gameObject);
        }

        public void SetColor(Color color)
        {
            pSystem.GetComponent<ParticleSystemRenderer>().material.SetColor("_Color", color);
        }
    }
}
