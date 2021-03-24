using UnityEngine;

namespace RectangleTrainer.ChromaTower
{
    using Engine;
    using View;

    public class Bootstrapper : MonoBehaviour
    {
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

        void Awake()
        {
            if(towerRenderer == null)
            {
                throw new System.Exception("Tower Renderer cannot be null. Assign a game object with ChromaTowerVisual behaviour");
            }

            if (ballTracker == null)
            {
                throw new System.Exception("Ball Tracker cannot be null. Attach ball tracker behaviour to a game object containing the main camera and assign it to this field");
            }

            if(playerBallPF == null)
            {
                throw new System.Exception("Player Ball prefab cannot be null. Create a prefab with Player Ball behaviour and assign to this field.");
            }

            IPlayerHealth playerHealth = new PlayerHPStaticRate(maxHP: playerMaxHP,
                                                                damageRate: damageRate,
                                                                regenRate: regenRate);

            IPlayerState playerState = new PlayerState(playerHealth);
            IScoreKeeper scoreKeeper = new PlayerPrefScoreKeeper();
            IDifficulty difficulty = new Difficulty(playerState, maxSlots: colorCount, introPlatforms: introPlatformCount);

            ChromaTower tower = new ChromaTower(scoreKeeper, playerState, difficulty);

            towerRenderer.SetTower(tower);
            towerRenderer.SetBall(playerBallPF);
            ballTracker.SetPlayerTransform(towerRenderer.playerBall.transform);

            tower.OnDamage += ballTracker.AddShake;
        }
    }
}