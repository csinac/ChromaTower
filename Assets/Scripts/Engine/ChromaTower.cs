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
        public event Action OnPause;
        public event Action OnNewGame;

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

        public bool HitCheck(int ballColorId, int targetColorId)
        {
            if(ballColorId == targetColorId)
            {
                playerState.Regen();
                scoreKeeper.IncrementCurrentScore(playerState.Combo);
                return true;
            }
            else
            {
                playerState.TakeDamage();
                if(playerState.IsDead)
                {
                    GameState = GameState.GameOver;
                    OnGameOver?.Invoke();
                }
                return false;
            }
        }
    }
}
