using System.Collections;
using UnityEngine;

namespace RectangleTrainer.ChromaTower.View
{
    class PlatformSlice: MonoBehaviour
    {
        public int colorId;
        public float dissolveTime { private get; set; }
        public Vector2 dissolveTimeRange { private get; set; }
        public AnimationCurve dissolvePattern { private get; set; }
        public float panicEscapeSpeed { private get; set; }
        public float panicSpinSpeed { private get; set; }

        private bool dissolving = false;
        public float maxZ { private get; set; }
        private float animTimeOffest = 0;

        public Platform ParentPlatform
        {
            get
            {
                if(transform.parent != null)
                    return transform.parent.GetComponent<Platform>();

                return null;
            }
        }

        private void Start()
        {
            animTimeOffest = Random.Range(0f, 2 * Mathf.PI);
        }

        public void Dissolve(bool panic)
        {
            dissolving = true;
            MeshCollider collider = GetComponent<MeshCollider>();
            if (collider)
                Destroy(collider);

            StartCoroutine(DissolveCR(panic));
        }

        private IEnumerator DissolveCR(bool panic)
        {
            Vector3 randomSpinAxis = Vector3.zero;
            Vector3 randomDirection = Vector3.zero;

            if(panic)
            {
                randomSpinAxis = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
                randomDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * panicEscapeSpeed;
            }

            yield return new WaitForSeconds(Random.Range(dissolveTimeRange.x, dissolveTimeRange.y));

            for(float f = 0; f < dissolveTime; f += Time.deltaTime)
            {
                float fn = f / dissolveTime; //normalized
                float scale = dissolvePattern.Evaluate(fn);
                transform.localScale = new Vector3(scale, scale, scale);

                if(panic)
                    transform.Rotate(randomSpinAxis, panicSpinSpeed);

                transform.localPosition += randomDirection;

                yield return new WaitForEndOfFrame();
            }

            Destroy(gameObject);
        }

        private void Update()
        {
            AnimateThickness();
        }

        private void AnimateThickness()
        {
            if (dissolving)
                return;

            float thickness = 1 + Mathf.Cos(Time.time + animTimeOffest) * maxZ;
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, thickness);
        }
    }
}