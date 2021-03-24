using System;

namespace RectangleTrainer.ChromaTower.Engine
{
    public class ChromaTower
    {
        public GameState GameState { get; private set; }
        public IScoreKeeper scoreKeeper { get; private set; }
        public IPlayerState playerState { get; private set; }
        public IDifficulty difficulty { get; private set; }

        public event Action OnGameOver;
        public event Action OnNewGame;
        public event Action OnDamage;
        public event Action OnHit;

        public ChromaTower(IScoreKeeper scoreKeeper, IPlayerState playerState, IDifficulty difficulty)
        {
            GameState = GameState.Idle;

            if(scoreKeeper == null || playerState == null || difficulty == null)
            {
                throw new Exception("Score Keeper, Player State or the Difficulty cannot be null.");
            }

            this.scoreKeeper = scoreKeeper;
            this.playerState = playerState;
            this.difficulty = difficulty;
        }

        public void NewGame()
        {
            playerState.Reset();
            scoreKeeper.ResetScore();
            GameState = GameState.InProgress;

            OnNewGame?.Invoke();
        }

        public HitResult HitCheck(int ballColorId, int targetColorId)
        {
            HitResult hitResult;

            if(ballColorId == targetColorId)
            {
                playerState.Regen();
                scoreKeeper.IncrementCurrentScore(playerState.Combo);
                hitResult = new HitResult { successfulHit = true, playerDead = false };
            }
            else
            {
                playerState.TakeDamage();
                OnDamage?.Invoke();

                if(playerState.IsDead)
                {
                    GameState = GameState.GameOver;
                    OnGameOver?.Invoke();

                    hitResult = new HitResult { successfulHit = false, playerDead = true };
                }
                else
                {
                    hitResult = new HitResult { successfulHit = false, playerDead = false };
                }
            }

            OnHit?.Invoke();
            return hitResult;
        }
    }
}
