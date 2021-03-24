using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace RectangleTrainer.ChromaTower.UI
{
    public class HPBar : MonoBehaviour
    {
        [SerializeField] RectTransform innerBarRT;
        [SerializeField] Image innerBarImage;
        [Space]
        [SerializeField] AnimationCurve curve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        [SerializeField] float resizeDuration = 0.5f;
        [Space]
        [SerializeField] Text hpText;
        [SerializeField] Text hpTextColored;
        [SerializeField] string hpTextFormat = "{0}%";

        Coroutine resizeCR = null;

        public void SetHealthProgress(float hpNormalized)
        {
            if (resizeCR != null) StopCoroutine(resizeCR);
            StartCoroutine(ResizeBar(hpNormalized));
        }
        public void UpdateColor(Color color)
        {
            innerBarImage.color = color;
            hpTextColored.color = color;
        }

        private IEnumerator ResizeBar(float target)
        {
            float start = innerBarRT.localScale.x;

            for(float f = 0; f < resizeDuration; f += Time.deltaTime)
            {
                float barScale = GetNormalized(start, target, f);

                SetHPTexts(barScale);
                ResizeBarWithTextCorrection(barScale);

                yield return new WaitForEndOfFrame();
            }

            resizeCR = null;
            innerBarRT.localScale = new Vector3(target, innerBarRT.localScale.y, innerBarRT.localScale.z);
        }

        private float GetNormalized(float start, float target, float f)
        {
            float fn = f / resizeDuration;
            return Mathf.Lerp(start, target, curve.Evaluate(fn));
        }

        private void SetHPTexts(float scale)
        {
            string barWidthStr = (scale * 100).ToString("N1");
            hpText.text = string.Format(hpTextFormat, barWidthStr);
            hpTextColored.text = string.Format(hpTextFormat, barWidthStr);
        }
        private void ResizeBarWithTextCorrection(float width)
        {
            hpText.transform.SetParent(transform);
            innerBarRT.localScale = new Vector3(width, innerBarRT.localScale.y, innerBarRT.localScale.z);
            hpText.transform.SetParent(innerBarRT);
        }
    }
}