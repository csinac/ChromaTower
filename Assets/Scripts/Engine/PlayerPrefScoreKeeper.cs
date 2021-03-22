namespace RectangleTrainer.ChromaTower.Engine
{
    public class PlayerPrefScoreKeeper : IScoreKeeper
    {
        private string scoreKey = "CT_Highscore";
        public int CurrentScore { get; private set; }

        public int HighestScore
        {
            get => UnityEngine.PlayerPrefs.GetInt(scoreKey, 0);
        }

        public void ResetScore()
        {
            CurrentScore = 0;
        }

        public void IncrementCurrentScore(int score)
        {
            CurrentScore += score;
            SetHighScore(CurrentScore);
        }

        public void SetHighScore(int score)
        {
            if(score > HighestScore)
                UnityEngine.PlayerPrefs.SetInt(scoreKey, score);
        }
    }
}