namespace RectangleTrainer.ChromaTower.Engine
{
    public interface IScoreKeeper
    {
        int HighestScore { get; }
        int CurrentScore { get; }
        void SetHighScore(int score);
        void IncrementCurrentScore(int score);
        void ResetScore();
    }
}
