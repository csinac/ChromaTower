using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RectangleTrainer.ChromaTower.View
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerBall : MonoBehaviour
    {
        [SerializeField] Renderer meshRenderer;
        [SerializeField] float fixedBounceVelocity = 5;
        [SerializeField] HitParticles particlePF;
        [Space]
        [SerializeField] float transitionSpeed = 1;
        [SerializeField] AnimationCurve transitionCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

        Coroutine colorCR;

        private Rigidbody rb;
        private ChromaTowerRenderer tower;
        public int colorId { get; private set; }
        private Vector3 startPos;
        private GameObject lastCollided = null;
        private Color color;

        void Awake()
        {
            rb = GetComponent<Rigidbody>();
            FakeRotation();
        }

        public void ResetBall()
        {
            transform.position = startPos;
            rb.velocity = Vector3.zero;
        }

        public void Initialize(Vector3 startPos)
        {
            this.startPos = startPos;
            transform.position = startPos;
        }

        public void AttachTower(ChromaTowerRenderer tower)
        {
            this.tower = tower;
        }

        public void UpdateColor(Color color, int colorId)
        {
            this.color = color;
            this.colorId = colorId;

            if (colorCR != null)
                StopCoroutine(colorCR);

            StartCoroutine(ColorTransition());
        }

        private IEnumerator ColorTransition()
        {
            Color currentColor = meshRenderer.material.GetColor("_Color");

            for (float f = 0; f < transitionSpeed; f += Time.deltaTime)
            {
                float fn = f / transitionSpeed;
                fn = transitionCurve.Evaluate(fn);
                meshRenderer.material.SetColor("_Color", Color.Lerp(currentColor, color, fn));
                yield return new WaitForEndOfFrame();
            }

            meshRenderer.material.SetColor("gradientColor", color);
            colorCR = null;
            yield return null;
        }

        private void OnCollisionEnter(Collision collision)
        {
            FakeRotation();
            lastCollided = collision.gameObject;
            rb.velocity = new Vector3(0, fixedBounceVelocity, 0);

            if(particlePF)
            {
                HitParticles particles = Instantiate(particlePF);
                particles.transform.localPosition = collision.GetContact(0).point;
                particles.SetColor(color);
            }
        }

        private void LateUpdate()
        {
            if(lastCollided)
            {
                tower.OnPlayerCollision(lastCollided);
                lastCollided = null;
            }
        }

        private void FakeRotation()
        {
            rb.AddTorque(new Vector3(Random.Range(-100f, 100f), Random.Range(-100f, 100f), Random.Range(-100f, 100f)));
        }
    }
}
