using UnityEngine;
using RectangleTrainer.WheelOfPseudoFortune.Renderer;

namespace RectangleTrainer.ChromaTower.View
{
    public class PlatformMaker: MonoBehaviour
    {
        [SerializeField] Material sliceMaterial;
        public GameObject GeneratePlatform(Engine.IDifficulty diff, AColorServer colorServer)
        {
            GameObject platformGO = new GameObject();
            platformGO.name = "Platform";
            Platform platform = platformGO.AddComponent<Platform>();

            float arc = 360f / diff.MaxSlots;

            float lastYRot = 0;
            for (int i = 0; i < diff.MaxSlots; i++)
            {
                int colorId = diff.NextSlot();
                Material mat = Instantiate(sliceMaterial);
                mat.SetColor("_Color", colorServer.GetColor(colorId, diff.MaxSlots));

                GameObject slice = WorldSliceMaker.GenerateGameObject(arc, mat, radius: 2);
                slice.name = "Disk Slice";
                slice.transform.SetParent(platform.transform);
                slice.transform.localEulerAngles = new Vector3(90, lastYRot, 0);

                PlatformSlice pSlice = slice.AddComponent<PlatformSlice>();
                pSlice.colorId = colorId;
                platform.InsertSlice(pSlice);

                lastYRot += arc;
            }

            return platformGO;
        }
    }
}