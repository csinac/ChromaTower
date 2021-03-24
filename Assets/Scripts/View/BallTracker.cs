using UnityEngine;

namespace RectangleTrainer.ChromaTower.View
{
    public class BallTracker : MonoBehaviour
    {
        [SerializeField] float dampening = 0.8f;
        [SerializeField] float shakeDiminish = 0.8f;
        [SerializeField] float shakeIncrement = 0.2f;
        [SerializeField] Transform childTransform;

        private Vector3 childInitialTransform;
        private Transform ballTransform;
        private float yMin;
        private float delta;
        private float targetPos;

        private float shake = 0;

        public void SetPlayerTransform(Transform target)
        {
            ballTransform = target;
            Initialize();
        }

        private void Initialize ()
        {
            targetPos = transform.localPosition.y;
            yMin = ballTransform.localPosition.y;
            delta = yMin - targetPos;
            childInitialTransform = new Vector3(childTransform.localPosition.x, childTransform.localPosition.y, childTransform.localPosition.z);
        }

        private void LateUpdate()
        {
            TrackTarget();
            DiminishShake();
        }

        private void TrackTarget()
        {
            if (ballTransform.localPosition.y < yMin)
                yMin = ballTransform.localPosition.y;

            targetPos = targetPos * dampening + (yMin - delta) * (1 - dampening);
            transform.localPosition = new Vector3(transform.localPosition.x, targetPos, transform.localPosition.z);
        }

        public void AddShake()
        {
            shake += shakeIncrement;
        }

        private void DiminishShake()
        {
            childTransform.localPosition = new Vector3( childInitialTransform.x + Random.Range(-shake, shake), 
                                                        childInitialTransform.y + Random.Range(-shake, shake),
                                                        childInitialTransform.z + Random.Range(-shake, shake));
            shake *= shakeDiminish;
        }
    }
}