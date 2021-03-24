using UnityEngine;
using UnityEngine.UI;

namespace RectangleTrainer.ChromaTower.UI
{
    using Engine;

    public class IdlePanel : PanelBase
    {
        [SerializeField] Text highscore;
        [SerializeField] NewGameButton newGameButton;

        private ChromaTower tower;

        void Start()
        {
            if(highscore == null)
            {
                Debug.LogWarning("Assign a game object with Text Behaviour to display the highscore");
            }

            newGameButton.onClick.AddListener(() =>
            {
                tower.NewGame();
            });
        }

        public void Initialize(ChromaTower tower)
        {
            this.tower = tower;
            this.tower.OnNewGame += () =>
            { 
                Fade(PanelVisibility.Hidden); 
            };
            this.tower.OnGameOver += () => 
            {
                UpdateHighscore();
                newGameButton.SwitchToReplayLabel();
                Fade(PanelVisibility.Visible);
            };
            UpdateHighscore();
        }

        private void UpdateHighscore()
        {
            if(highscore)
                highscore.text = tower.scoreKeeper.HighestScore.ToString();
        }
    }

}
