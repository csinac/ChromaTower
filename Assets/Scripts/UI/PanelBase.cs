using System.Collections;
using UnityEngine;
using UnityEngine.UI;


namespace RectangleTrainer.ChromaTower.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    [RequireComponent(typeof(RectTransform))]
    public class PanelBase : MonoBehaviour
    {
        private CanvasGroup cg;
        private RectTransform rt;

        private bool interactable;
        private float fadeTime = 0.5f;

        private void Awake()
        {
            cg = GetComponent<CanvasGroup>();
            rt = GetComponent<RectTransform>();

            interactable = cg.interactable;
        }

        virtual public void Toggle(PanelVisibility visibility)
        {
            bool state = visibility == PanelVisibility.Visible;

            cg.interactable = state && interactable;
            cg.alpha = state ? 1 : 0;
        }

        virtual public void Fade(PanelVisibility fadeTarget)
        {
            if (fadeTarget == PanelVisibility.Hidden)
                cg.interactable = false;

            StartCoroutine(FadeCR(fadeTarget));
        }

        private IEnumerator FadeCR(PanelVisibility fadeTarget)
        {
            float targetScale = fadeTarget == PanelVisibility.Visible ? 1f : 1.5f;
            float startScale = rt.localScale.x;

            for (float f = 0; f < fadeTime; f += Time.deltaTime)
            {
                float fn = f / fadeTime;
                cg.alpha = fadeTarget == PanelVisibility.Visible ? fn : (1 - fn);

                float scale = Mathf.Lerp(startScale, targetScale, fn);

                rt.localScale = new Vector3(scale, scale, scale);

                yield return new WaitForEndOfFrame();
            }

            if(fadeTarget == PanelVisibility.Visible)
            {
                cg.interactable = interactable;
                cg.alpha = 1;
                rt.localScale = Vector3.one;                
            }
            else
            {
                cg.alpha = 0;
            }

            yield return null;
        }


        public enum PanelVisibility { Visible, Hidden };
    }
}
