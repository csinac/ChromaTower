using NUnit.Framework;

namespace RectangleTrainer.ChromaTower.UnitTests {
    using Engine;

    public class ChromaTowerTests
    {
        [Test]
        public void ChromaTower_NullScoreKeeper_ShouldThrowException()
        {
            PlayerState player = new PlayerState(new PlayerHPStaticRate());
            Assert.Throws<System.Exception>(() => new ChromaTower(  null,
                                                                    player,
                                                                    new Difficulty(player)));
        }

        [Test]
        public void ChromaTower_NullPlayerState_ShouldThrowException()
        {
            Assert.Throws<System.Exception>(() => new ChromaTower(  new PlayerPrefScoreKeeper(),
                                                                    null,
                                                                    new Difficulty(new PlayerState(new PlayerHPStaticRate()))));
        }

        [Test]
        public void ChromaTower_NullDifficulty_ShouldThrowException()
        {
            Assert.Throws<System.Exception>(() => new ChromaTower(  new PlayerPrefScoreKeeper(),
                                                                    new PlayerState(new PlayerHPStaticRate()),
                                                                    null));
        }

        [Test]
        public void ChromaTower_GameStart_ShouldStartIdle()
        {
            IScoreKeeper scoreKeeper = new PlayerPrefScoreKeeper();
            IPlayerState playerState = new PlayerState(new PlayerHPStaticRate());
            IDifficulty difficulty = new Difficulty(playerState);

            ChromaTower tower = new ChromaTower(scoreKeeper, playerState, difficulty);

            Assert.AreEqual(tower.GameState, GameState.Idle);
        }

        [Test]
        public void ChromaTower_HitCheck_PlayerShouldTakeDamageOnDifferentColor()
        {
            IScoreKeeper scoreKeeper = new PlayerPrefScoreKeeper();
            IPlayerState playerState = new PlayerState(new PlayerHPStaticRate());
            IDifficulty difficulty = new Difficulty(playerState);

            ChromaTower tower = new ChromaTower(scoreKeeper, playerState, difficulty);
            int hpBefore = playerState.HP;

            tower.HitCheck(1, 2);

            int hpAfter = playerState.HP;

            Assert.Greater(hpBefore, hpAfter);
        }

        [Test]
        public void ChromaTower_HitCheck_PlayerShouldRegenHPOnSameColor()
        {
            IScoreKeeper scoreKeeper = new PlayerPrefScoreKeeper();
            IPlayerState playerState = new PlayerState(new PlayerHPStaticRate());
            IDifficulty difficulty = new Difficulty(playerState);

            ChromaTower tower = new ChromaTower(scoreKeeper, playerState, difficulty);

            tower.HitCheck(1, 2); //take damage
            int hpBefore = playerState.HP;

            tower.HitCheck(1, 1); //regen
            int hpAfter = playerState.HP;

            Assert.Less(hpBefore, hpAfter);
        }

        [Test]
        public void ChromaTower_HitCheck_GameShouldEndOnZeroHP()
        {
            IScoreKeeper scoreKeeper = new PlayerPrefScoreKeeper();
            IPlayerState playerState = new PlayerState(new PlayerHPStaticRate(100, 0.5f, 0.5f));
            IDifficulty difficulty = new Difficulty(playerState);

            ChromaTower tower = new ChromaTower(scoreKeeper, playerState, difficulty);
            tower.HitCheck(1, 2); //50% left
            tower.HitCheck(1, 2); //0% left

            Assert.AreEqual(tower.GameState, GameState.GameOver);
        }
    }
}
