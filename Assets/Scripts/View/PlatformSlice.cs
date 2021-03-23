using UnityEngine;

namespace RectangleTrainer.ChromaTower.View
{
    class PlatformSlice: MonoBehaviour
    {
        public int colorId;
        public Platform ParentPlatform
        {
            get
            {
                if(transform.parent != null)
                    return transform.parent.GetComponent<Platform>();

                return null;
            }
        }
    }
}