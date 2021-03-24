using UnityEngine;
using UnityEngine.UI;

namespace RectangleTrainer.ChromaTower.UI
{
    public class HPBar : MonoBehaviour
    {
        [SerializeField] RectTransform innerBarRT;
        [SerializeField] Image innerBarImage;

        public void SetHealthProgress(float hpNormalized)
        {
            innerBarRT.localScale = new Vector3(hpNormalized, innerBarRT.localScale.y, innerBarRT.localScale.z);
        }
        public void UpdateColor(Color color)
        {
            innerBarImage.color = color;
        }
    }
}