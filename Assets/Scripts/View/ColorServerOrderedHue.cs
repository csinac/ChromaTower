using UnityEngine;

namespace RectangleTrainer.ChromaTower.View
{
    public class ColorServerOrderedHue: AColorServer
    {
        [SerializeField] private float brightness;
        [SerializeField] private float saturation;

        private Color lastColor;

        override public Color GetColor(int index, int range)
        {
            lastColor = Color.HSVToRGB(1f * index / range, saturation, brightness);
            return lastColor;
        }

        public override Color LastColor()
        {
            return lastColor;
        }

        override public Color MakeColor()
        {
            return Color.HSVToRGB(Random.Range(0f, 1f), saturation, brightness);
        }
    }
}
