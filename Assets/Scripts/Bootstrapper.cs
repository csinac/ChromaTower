using UnityEngine;

namespace RectangleTrainer.ChromaTower
{
    using Engine;
    using View;
    using UI;

    public class Bootstrapper : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] IdlePanel idlePanel;
        [SerializeField] InGamePanel inGamePanel;
        [SerializeField] BackgroundRenderer backgroundRenderer;
        [SerializeField] VignetteRenderer vignetteRenderer;
        [Space]
        [Header("Gameplay Objects")]
        [SerializeField] ChromaTowerRenderer towerRenderer;
        [SerializeField] PlayerBall playerBallPF;
        [SerializeField] BallTracker ballTracker;
        [SerializeField] AudioManager audioManager;
        [Space]
        [Header("Health Options")]
        [SerializeField] int playerMaxHP = 100;
        [SerializeField, Range(0, 1)] float damageRate = 0.32f;
        [SerializeField, Range(0, 1)] float regenRate = 0.03f;
        [SerializeField, Range(0, 1)] float nearDeathRatio = 0.15f;
        [Space]
        [Header("Difficulty Options")]
        [SerializeField] int colorCount = 7;
        [SerializeField] int introPlatformCount = 2;

        ChromaTower tower;
        IPlayerHealth playerHealth;
        IPlayerState playerState;
        IScoreKeeper scoreKeeper;
        IDifficulty difficulty;

        void Start()
        {
            try
            {
                NullChecks();
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return;
            }

            PrepareEngine();
            PrepareRenderers();
            PrepareUI();

            towerRenderer.StartGame();
        }

        private void PrepareEngine()
        {
            playerHealth = new PlayerHPStaticRate(maxHP: playerMaxHP,
                                                  damageRate: damageRate,
                                                  regenRate: regenRate);

            playerState = new PlayerState(playerHealth);
            scoreKeeper = new PlayerPrefScoreKeeper();
            difficulty = new Difficulty(playerState: playerState,
                                        maxSlots: colorCount,
                                        introPlatforms: introPlatformCount,
                                        almostDeadLimit: nearDeathRatio);

            tower = new ChromaTower(scoreKeeper, playerState, difficulty);
        }

        private void PrepareRenderers()
        {
            towerRenderer.SetTower(tower);
            towerRenderer.SetBall(playerBallPF);
            ballTracker.Initialize(towerRenderer.playerBall.transform);
            ballTracker.SubscribeToEngineEvents(tower);
            backgroundRenderer.Initialize(towerRenderer);
            vignetteRenderer.Initialize(towerRenderer);
            audioManager.Initialize(tower);
        }

        private void PrepareUI()
        {
            idlePanel.Initialize(tower);
            inGamePanel.Initialize(towerRenderer);
            idlePanel.Fade(PanelBase.PanelVisibility.Visible);
            inGamePanel.Toggle(PanelBase.PanelVisibility.Hidden);
        }

        private void NullChecks()
        {
            if (idlePanel == null)
                throw new System.Exception("Idle Panel cannot be null. Assign a game object with the Idle panel behaviour");

            if (inGamePanel == null)
                throw new System.Exception("In Game Panel cannot be null. Assign a game object with the Idle panel behaviour");

            if (backgroundRenderer == null)
                throw new System.Exception("Background Renderer cannot be null. Assign a UI panel in a world space canvas with the Background Renderer behaviour");

            if (vignetteRenderer == null)
                throw new System.Exception("Vignette Renderer cannot be null. Assign a UI panel in a world space canvas with the Vignette Renderer behaviour");

            if(audioManager == null)
                throw new System.Exception("Audio Manager cannot be null. Assign a game object with Audio Manager behaviour");

            if (towerRenderer == null)
                throw new System.Exception("Tower Renderer cannot be null. Assign a game object with ChromaTowerVisual behaviour");

            if (ballTracker == null)
                throw new System.Exception("Ball Tracker cannot be null. Attach ball tracker behaviour to a game object containing the main camera and assign it to this field");

            if (playerBallPF == null)
                throw new System.Exception("Player Ball prefab cannot be null. Create a prefab with Player Ball behaviour and assign to this field");
        }
    }
}