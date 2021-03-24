using UnityEngine;
using System.Collections;

namespace RectangleTrainer.ChromaTower.View
{
    using Engine;

    [RequireComponent(typeof(UnityEngine.UI.Image))]
    public class BackgroundRenderer : MonoBehaviour
    {
        [SerializeField] float transitionSpeed = 0.5f;
        [SerializeField] AnimationCurve transitionCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

        private Material material;

        void Awake()
        {
            material = GetComponent<UnityEngine.UI.Image>().material;
        }

        public void Initialize(ChromaTowerRenderer towerRenderer)
        {
            ChromaTower tower = towerRenderer.tower;
            towerRenderer.OnBallColorUpdate += UpdateGradient;
            tower.OnGameOver += () => { UpdateGradient(Color.black); };
        }

        Coroutine colorCR = null;
        private void UpdateGradient(Color color)
        {
            if (colorCR != null)
                StopCoroutine(colorCR);

            colorCR = StartCoroutine(ColorTransition(color));
        }

        private IEnumerator ColorTransition(Color targetColor)
        {
            Color currentColor = material.GetColor("gradientColor");

            for (float f = 0; f < transitionSpeed; f += Time.deltaTime)
            {
                float fn = f / transitionSpeed;
                fn = transitionCurve.Evaluate(fn);
                material.SetColor("gradientColor", Color.Lerp(currentColor, targetColor, fn));
                yield return new WaitForEndOfFrame();
            }

            material.SetColor("gradientColor", targetColor);
            colorCR = null;
            yield return null;
        }
    }
}