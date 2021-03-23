using System.Collections.Generic;
using UnityEngine;

namespace RectangleTrainer.ChromaTower.View
{
    class Platform : MonoBehaviour
    {
        private List<PlatformSlice> slices = new List<PlatformSlice>();

        public void InsertSlice(PlatformSlice slice)
        {
            slices.Add(slice);
        }

        public int PickRandomTarget()
        {
            int picked = Random.Range(0, slices.Count);
            return slices[picked].colorId;
        }

        public void Dissolve()
        {
            Destroy(gameObject);
            //TODO: Animate
        }

        public void Explode()
        {
            Destroy(gameObject);
        }
    }
}
