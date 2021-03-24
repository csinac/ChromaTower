using UnityEngine;

namespace RectangleTrainer.ChromaTower.View
{
    public abstract class AColorServer: MonoBehaviour
    {
        abstract public Color MakeColor();
        abstract public Color GetColor(int index, int range);
        abstract public Color LastColor();
    }
}
