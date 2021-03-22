using UnityEngine;
using RectangleTrainer.ChromaTower.Engine;

public class Bootstrapper : MonoBehaviour
{
    void Awake()
    {
        IPlayerHealth difficulty = new PlayerHPStaticRate(  maxHP: 100,
                                                            damageRate: 0.34f,
                                                            regenRate: 0.1f);

        IPlayerState playerState = new PlayerState(difficulty);
        IScoreKeeper scoreKeeper = new PlayerPrefScoreKeeper();

        ChromaTower tower = new ChromaTower(scoreKeeper, playerState);
    }
}
