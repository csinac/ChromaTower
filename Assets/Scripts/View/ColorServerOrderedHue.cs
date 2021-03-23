using UnityEngine;

namespace RectangleTrainer.ChromaTower.View
{
    public class ColorServerOrderedHue: AColorServer
    {
        [SerializeField] private float brightness;
        [SerializeField] private float saturation;
        [SerializeField] private float minHueDifference;

        public ColorServerOrderedHue(float saturation, float brightness, float minHueDifference)
        {
            this.saturation = saturation;
            this.brightness = brightness;
            this.minHueDifference = minHueDifference;
        }

        override public Color GetColor(int index, int range)
        {
            return Color.HSVToRGB(1f * index / range, saturation, brightness);
        }

        override public Color MakeColor()
        {
            return Color.HSVToRGB(Random.Range(0f, 1f), saturation, brightness);
        }
    }
}
