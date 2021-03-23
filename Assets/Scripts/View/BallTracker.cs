using UnityEngine;

namespace RectangleTrainer.ChromaTower.View
{
    public class BallTracker : MonoBehaviour
    {
        [SerializeField] float dampening = 0.8f;
        private Transform ballTransform;
        private float yMin;
        private float delta;
        private float targetPos;

        public void SetPlayerTransform(Transform target)
        {
            ballTransform = target;
            targetPos = transform.localPosition.y;
            yMin = ballTransform.localPosition.y;
            delta = yMin - targetPos;
        }

        private void FixedUpdate()
        {
            if(ballTransform.localPosition.y < yMin)
                yMin = ballTransform.localPosition.y;

            targetPos = targetPos * dampening + (yMin - delta) * (1 - dampening);
            transform.localPosition = new Vector3(transform.localPosition.x, targetPos, transform.localPosition.z);
        }
    }
}