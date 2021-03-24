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
        [Space]
        [Header("Gameplay Objects")]
        [SerializeField] ChromaTowerRenderer towerRenderer;
        [SerializeField] PlayerBall playerBallPF;
        [SerializeField] BallTracker ballTracker;
        [Space]
        [Header("Health Options")]
        [SerializeField] int playerMaxHP = 100;
        [SerializeField, Range(0, 1)] float damageRate = 0.32f;
        [SerializeField, Range(0, 1)] float regenRate = 0.03f;
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
            difficulty = new Difficulty(playerState, maxSlots: colorCount, introPlatforms: introPlatformCount);

            tower = new ChromaTower(scoreKeeper, playerState, difficulty);

            towerRenderer.SetTower(tower);
            towerRenderer.SetBall(playerBallPF);
            ballTracker.Initialize(towerRenderer.playerBall.transform);
            ballTracker.SubscribeToEngineEvents(tower);
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

            if (towerRenderer == null)
                throw new System.Exception("Tower Renderer cannot be null. Assign a game object with ChromaTowerVisual behaviour");

            if (ballTracker == null)
                throw new System.Exception("Ball Tracker cannot be null. Attach ball tracker behaviour to a game object containing the main camera and assign it to this field");

            if (playerBallPF == null)
                throw new System.Exception("Player Ball prefab cannot be null. Create a prefab with Player Ball behaviour and assign to this field");
        }
    }
}