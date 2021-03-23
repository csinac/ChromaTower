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

        void Awake()
        {
            if(towerRenderer == null)
            {
                throw new System.Exception("Tower Renderer cannot be null. Assign a game object with ChromaTowerVisual behaviour");
            }

            if (ballTracker == null)
            {
                throw new System.Exception("Ball Tracker cannot be null. Attach ball tracker behaviour to the main camera and assign it to this field");
            }

            if(playerBallPF == null)
            {
                throw new System.Exception("Player Ball prefab cannot be null. Create a prefab with Player Ball behaviour and assign to this field.");
            }

            IPlayerHealth playerHealth = new PlayerHPStaticRate(maxHP: 100,
                                                                damageRate: 0.34f,
                                                                regenRate: 0.1f);

            IPlayerState playerState = new PlayerState(playerHealth);
            IScoreKeeper scoreKeeper = new PlayerPrefScoreKeeper();
            IDifficulty difficulty = new Difficulty(playerState, maxSlots: 5, introPlatforms: 2);

            ChromaTower tower = new ChromaTower(scoreKeeper, playerState, difficulty);

            towerRenderer.SetTower(tower);
            towerRenderer.SetBall(playerBallPF);
            ballTracker.SetPlayerTransform(towerRenderer.playerBall.transform);
        }
    }
}