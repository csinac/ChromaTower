using System;

namespace RectangleTrainer.ChromaTower.Engine
{
    public class ChromaTower
    {
        public GameState GameState { get; private set; }
        private IScoreKeeper scoreKeeper;
        private IPlayerState playerState;

        public event Action OnGameOver;
        public event Action OnPause;
        public event Action OnNewGame;

        public ChromaTower(IScoreKeeper scoreKeeper, IPlayerState playerState)
        {
            GameState = GameState.Idle;

            if(scoreKeeper == null || playerState == null)
            {
                throw new Exception("Score Keeper and Player State cannot be null.");
            }

            this.scoreKeeper = scoreKeeper;
            this.playerState = playerState;
        }

        public void NewGame()
        {
            playerState.Reset();
            scoreKeeper.ResetScore();

            OnNewGame?.Invoke();
        }

        public void HitCheck(int ballColorId, int sliceColorId)
        {
            if(ballColorId == sliceColorId)
            {
                playerState.Regen();
                scoreKeeper.IncrementCurrentScore(playerState.Combo);
            }
            else
            {
                playerState.TakeDamage();
                if(playerState.IsDead)
                {
                    GameState = GameState.GameOver;
                    OnGameOver?.Invoke();
                }
            }
        }
    }
}
