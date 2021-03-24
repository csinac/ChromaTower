using UnityEngine;
using RectangleTrainer.WheelOfPseudoFortune.Renderer;

namespace RectangleTrainer.ChromaTower.View
{
    public class PlatformMaker: MonoBehaviour
    {
        [Header("Slice Build Parameters")]
        [SerializeField] Material sliceMaterial;
        [SerializeField] float sliceSeparation = 0.1f;
        [Header("Slice Animation Parameters")]
        [SerializeField] float sliceThicknessAnimationLimit = 0.2f;
        [SerializeField] float sliceDissolveTime = 0.5f;
        [SerializeField] Vector2 sliceDissolveTimeRange = new Vector2(0f, 0.25f);
        [SerializeField] AnimationCurve sliceDissolvePattern = AnimationCurve.EaseInOut(0, 1, 1, 0);
        [Space]
        [Header("Slice Panic Parameters")]
        [SerializeField] float slicePanicEscapeSpeed = 0.1f;
        [SerializeField] float slicePanicSpinSpeed = 10;

        private float arc;

        public GameObject GeneratePlatform(Engine.IDifficulty diff, AColorServer colorServer)
        {
            GameObject platformGO = new GameObject();
            platformGO.name = "Platform";
            Platform platform = platformGO.AddComponent<Platform>();

            arc = 360f / diff.MaxSlots;

            float lastYRot = Random.Range(0f, 360f);
            for (int i = 0; i < diff.MaxSlots; i++)
            {
                int colorId = diff.NextSlot();

                Material mat = MakeMaterial(colorId, diff.MaxSlots, colorServer);
                GameObject slice = MakeSlice(mat, platform.transform, lastYRot);
                PlatformSlice pSlice = InitializeSlice(slice, colorId);

                platform.InsertSlice(pSlice);
                lastYRot += arc;
            }

            return platformGO;
        }

        private Material MakeMaterial(int colorId, int maxSlots, AColorServer colorServer)
        {
            Material mat = Instantiate(sliceMaterial);
            mat.SetColor("_Color", colorServer.GetColor(colorId, maxSlots));

            return mat;
        }

        private GameObject MakeSlice(Material mat, Transform parent, float yRot)
        {
            GameObject slice = WorldSliceMaker.GenerateGameObject(sliceSeparation, arc, mat, radius: 2);
            slice.name = "Disk Slice";
            slice.transform.SetParent(parent);
            slice.transform.localEulerAngles = new Vector3(90, yRot, 0);

            return slice;
        }

        private PlatformSlice InitializeSlice(GameObject slice, int colorId)
        {
            PlatformSlice pSlice = slice.AddComponent<PlatformSlice>();
            pSlice.colorId = colorId;
            pSlice.maxZ = sliceThicknessAnimationLimit;
            pSlice.dissolveTime = sliceDissolveTime;
            pSlice.dissolveTimeRange = sliceDissolveTimeRange;
            pSlice.dissolvePattern = sliceDissolvePattern;
            pSlice.panicEscapeSpeed = slicePanicEscapeSpeed;
            pSlice.panicSpinSpeed = slicePanicSpinSpeed;

            return pSlice;
        }
    }
}