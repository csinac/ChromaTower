using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RectangleTrainer.ChromaTower.UI
{
    using Engine;
    using View;

    public class InGamePanel : PanelBase
    {
        [SerializeField] HPBar hpBar;
        [SerializeField] Text multiplier;
        [SerializeField] Text score;

        ChromaTower tower;
        public void Initialize(ChromaTowerRenderer towerRenderer)
        {
            tower = towerRenderer.tower;
            towerRenderer.OnBallColorUpdate += UpdateHPBarColor;

            tower.OnNewGame += () => { Fade(PanelVisibility.Visible); };
            tower.OnGameOver += () => { Fade(PanelVisibility.Hidden); };
            tower.OnHit += UpdateUI;
            tower.OnNewGame += UpdateUI;
        }

        private void UpdateHPBarColor(Color color)
        {
            hpBar.UpdateColor(color);
        }

        public void UpdateUI()
        {
            multiplier.text = $"x{tower.playerState.Combo}";
            score.text = tower.scoreKeeper.CurrentScore.ToString();
            hpBar.SetHealthProgress(tower.playerState.HPNormalized);
        }
    }
}