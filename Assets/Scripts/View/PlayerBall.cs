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
        private Rigidbody rb;
        private ChromaTowerRenderer tower;
        public int colorId { get; private set; }
        private Vector3 startPos;
        private GameObject lastCollided = null;

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
            meshRenderer.material.SetColor("_Color", color);
            this.colorId = colorId;
        }

        private void OnCollisionEnter(Collision collision)
        {
            FakeRotation();
            lastCollided = collision.gameObject;
            rb.velocity = new Vector3(0, fixedBounceVelocity, 0);
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
