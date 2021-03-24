using UnityEngine;

namespace RectangleTrainer.ChromaTower.View
{
    public class BallTracker : MonoBehaviour
    {
        [SerializeField, Range(0f, 1f), Tooltip("Zero = instant")] float dampening = 0.8f;
        [SerializeField, Range(0f, 1f), Tooltip("Zero = instant")] float shakeDiminish = 0.8f;
        [SerializeField] float shakeIncrement = 0.2f;
        [SerializeField] Transform childTransform;

        private Vector3 childInitialPosition;
        private Transform ballTransform;
        private float yMin;
        private float delta;
        private float targetPos;

        private float shake = 0;
        private bool firstRestart = true;

        public void Initialize(Transform target)
        {
            ballTransform = target;
            Initialize();
        }

        public void SubscribeToEngineEvents(Engine.ChromaTower tower)
        {
            tower.OnNewGame += Restart;
            tower.OnDamage += AddShake;
        }

        private void Initialize ()
        {
            targetPos = transform.localPosition.y;
            yMin = ballTransform.localPosition.y;
            delta = yMin - targetPos;
            childInitialPosition = new Vector3(childTransform.localPosition.x, childTransform.localPosition.y, childTransform.localPosition.z);
        }

        public void Restart()
        {
            if(firstRestart)
            {
                firstRestart = false;
                return;
            }

            yMin = ballTransform.localPosition.y;
            targetPos = yMin;
        }

        private void LateUpdate()
        {
            TrackTarget();
            DiminishShake();
        }

        private void TrackTarget()
        {
            if (!ballTransform)
                return;

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
            childTransform.localPosition = new Vector3( childInitialPosition.x + Random.Range(-shake, shake), 
                                                        childInitialPosition.y + Random.Range(-shake, shake),
                                                        childInitialPosition.z + Random.Range(-shake, shake));
            shake *= shakeDiminish;
        }
    }
}