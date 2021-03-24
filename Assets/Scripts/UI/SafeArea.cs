using UnityEngine;
using UnityEngine.UI;

namespace RectangleTrainer.ChromaTower.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class SafeArea : MonoBehaviour
    {
        [SerializeField] RectTransform canvasRT;
        private RectTransform rt;
        void Start()
        {
            rt = GetComponent<RectTransform>();
            if (canvasRT)
                SetupSafeArea();
            else
                Debug.LogWarning("Assign the container canvas to canvasRT field in order to calculate the safe area.");
        }

        private void SetupSafeArea()
        {
            float canvasScale = canvasRT.localScale.x;

            float offsetMinX = Screen.safeArea.x / canvasScale;
            float offsetMinY = Screen.safeArea.y / canvasScale;

            float offsetMaxX = (Screen.safeArea.xMax - Screen.width) / canvasScale;
            float offsetMaxY = (Screen.safeArea.yMax - Screen.height) / canvasScale;

            rt.offsetMin = new Vector2(offsetMinX, offsetMinY);
            rt.offsetMax = new Vector2(offsetMaxX, offsetMaxY);
        }
        // Update is called once per frame
        void Update()
        {

        }
    }
}
