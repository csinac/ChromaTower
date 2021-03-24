using UnityEngine;
using System.Collections;

namespace RectangleTrainer.ChromaTower.View
{
    using Engine;

    [RequireComponent(typeof(UnityEngine.UI.Image))]
    public class VignetteRenderer : MonoBehaviour
    {
        [SerializeField] Color warningColor = new Color(0.75f, 0, 0);

        [SerializeField] private float flashTime = 0.25f;
        [SerializeField] private float damageTime = 1f;
        [SerializeField] AnimationCurve flashCurve;
        [Space]
        [SerializeField] private float fadeTime = 1f;
        [SerializeField] AnimationCurve fadeCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

        private Color ballColor;
        private Material material;

        private IDifficulty difficulty;
        private bool lastNearDeath;

        Coroutine transitionCR = null;

        void Awake()
        {
            material = GetComponent<UnityEngine.UI.Image>().material;
            Hide(true);
        }

        public void Initialize(ChromaTowerRenderer towerRenderer)
        {
            ChromaTower tower = towerRenderer.tower;
            difficulty = tower.difficulty;
            
            towerRenderer.OnBallColorUpdate += GetBallColor;
            tower.OnSuccessfulHit += SuccessVignette;
            tower.OnNewGame += () => { Hide(true); };
            tower.OnDamage += DamageFlash;
        }

        private void Hide(bool toggle)
        {
            if(toggle)
                material.SetFloat("overallAlpha", 0f);
            else
                material.SetFloat("overallAlpha", 1f);
        }

        private void GetBallColor(Color color)
        {
            ballColor = color;
        }

        private void SuccessVignette()
        {
            if (CheckNearDeath())
                return;

            ModifyMaterial(ballColor, 0f, 1.5f);

            if (transitionCR != null)
                StopCoroutine(transitionCR);

            transitionCR = StartCoroutine(Animate(flashTime, flashCurve, false));
        }

        private void DamageFlash()
        {
            if (CheckNearDeath())
                return;

            ModifyMaterial(warningColor, 0.25f, 0.6f);

            if (transitionCR != null)
                StopCoroutine(transitionCR);

            transitionCR = StartCoroutine(Animate(damageTime, flashCurve, false));
        }

        private bool CheckNearDeath()
        {
            if (NearDeathStatusChanged())
            {
                ModifyMaterial(warningColor, 0f, 0.5f);

                if (transitionCR != null)
                    StopCoroutine(transitionCR);

                transitionCR = StartCoroutine(Animate(fadeTime, fadeCurve, !lastNearDeath));
            }

            return lastNearDeath;
        }

        private bool NearDeathStatusChanged()
        {
            if(lastNearDeath != difficulty.NearDeath)
            {
                lastNearDeath = difficulty.NearDeath;
                return true;
            }

            return false;
        }

        private IEnumerator Animate(float duration, AnimationCurve curve, bool reverse)
        {
            for (float f = 0; f < duration; f += Time.deltaTime)
            {
                float fn = f / duration;
                if (reverse) fn = 1 - fn;
                material.SetFloat("overallAlpha", curve.Evaluate(fn));
                yield return new WaitForEndOfFrame();
            }

            material.SetFloat("overallAlpha", curve.Evaluate(reverse ? 0 : 1));
            transitionCR = null;
            yield return null;
        }


        private void ModifyMaterial(Color color, float minimumAlpha, float transparencySpeed)
        {
            material.SetColor("vignetteColor", color);
            material.SetFloat("alphaMin", minimumAlpha);
            material.SetFloat("speed", transparencySpeed);

        }
    }
}
