using System;
using System.Collections.Generic;
using UnityEngine;

namespace RectangleTrainer.ChromaTower.View
{
    using Engine;

    [DisallowMultipleComponent]
    [RequireComponent(typeof(DragRotator))]
    public class ChromaTowerRenderer : MonoBehaviour
    {
        public ChromaTower tower { get; private set; }

        [SerializeField] PlatformMaker platformMaker;
        [SerializeField] AColorServer colorServer;
        [Space]
        [SerializeField] Vector3 playerStartPos = new Vector3(0, 1, -1);
        [SerializeField] float diskYStart = 0;
        [SerializeField] float diskSeparation = 2;

        public Action<Color> OnBallColorUpdate;

        private float diskY;
        public PlayerBall playerBall { get; private set; }
        public bool ready { get; private set; }
        private DragRotator rotator;
        private List<Platform> platformList;

        void Start()
        {
            NullChecks();
            Initialize();
        }

        public void StartGame()
        {
            if (ready)
                return;

            BuildPlatforms();
            UpdateBallColor();
            playerBall.ResetBall();
        }

        private void NullChecks()
        {
            if (platformMaker == null || colorServer == null)
                throw new System.Exception("Platform Maker or Color Maker cannot be null");
        }

        private void Initialize()
        {
            Application.targetFrameRate = 60;
            platformList = new List<Platform>();
            rotator = GetComponent<DragRotator>();
            rotator.AttachTower(this);
        }

        public void SetBall(PlayerBall playerBallPF)
        {
            playerBall = Instantiate(playerBallPF);
            playerBall.AttachTower(this);
            playerBall.Initialize(playerStartPos);
        }

        private void BuildPlatforms()
        {
            diskY = diskYStart;
            for (int i = 0; i < 5; i++)
            {
                tower.difficulty.UpdateSingleColorStatus(i);
                PushPlatform();
            }

            ready = true;
        }

        private void PushPlatform()
        {
            GameObject platform = platformMaker.GeneratePlatform(tower.difficulty, colorServer);
            platform.transform.SetParent(transform);
            platform.transform.localPosition = new Vector3(0, diskY, 0);
            diskY -= diskSeparation;
            platformList.Add(platform.GetComponent<Platform>());
        }

        public void SetTower(ChromaTower tower)
        {
            this.tower = tower;
            this.tower.OnNewGame += StartGame;
        }

        public void OnPlayerCollision(GameObject collided)
        {
            if (tower.GameState != GameState.InProgress)
                return;

            if (collided == null)
                return;

            PlatformSlice slice = collided.GetComponent<PlatformSlice>();
            if (slice == null)
                return;

            HitResult hitResult = tower.HitCheck(playerBall.colorId, slice.colorId);

            platformList.Remove(slice.ParentPlatform);

            if(hitResult.playerDead)
            {
                slice.ParentPlatform.Dissolve(panic: true);
                DestroyAllPlatforms();
            }
            else
            {
                if (hitResult.successfulHit)
                    slice.ParentPlatform.Dissolve();
                else
                    slice.ParentPlatform.Dissolve(panic: true);

                PushPlatform();
                UpdateBallColor();
            }
        }

        private void UpdateBallColor()
        {
            if (platformList.Count == 0)
                return;

            int pickedColor = platformList[0].PickRandomTarget();
            playerBall.UpdateColor(colorServer.GetColor(pickedColor, tower.difficulty.MaxSlots), pickedColor);

            OnBallColorUpdate?.Invoke(colorServer.LastColor());
        }

        private void DestroyAllPlatforms()
        {
            foreach(Platform platform in platformList)
            {
                platform.Dissolve();
            }

            platformList.Clear();
            ready = false;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(playerStartPos, 0.1f);

            for(int i = 0; i < 3; i++)
            {
                Gizmos.DrawWireCube(new Vector3(0, diskY - diskSeparation * i, 0), new Vector3(4, 0.1f, 4));
            }
        }
    }
}
