using NUnit.Framework;

namespace RectangleTrainer.ChromaTower.UnitTests
{
    using Engine;

    public class PlayerStateTests
    {
        [Test]
        public void PlayerState_NullPlayerHealth_ShouldThrowException()
        {
            Assert.Throws<System.Exception>(() => new PlayerState(null));
        }

        [Test]
        public void Regen_ComboShouldIncrement()
        {
            PlayerState playerState = new PlayerState(new PlayerHPStaticRate());

            playerState.Regen();
            playerState.Regen();
            playerState.Regen();
            playerState.Regen();

            Assert.AreEqual(playerState.Combo, 4);
        }

        [Test]
        public void Regen_ShouldIncreaseHP()
        {
            PlayerState playerState = new PlayerState(new PlayerHPStaticRate());
            playerState.TakeDamage();
            int hp = playerState.HP;

            playerState.Regen();
            int hpAfter = playerState.HP;

            Assert.Greater(hpAfter, hp);
        }

        [Test]
        public void Regen_OnFullHP_HPShouldBeCappedAtMaxHP()
        {
            PlayerHPStaticRate health = new PlayerHPStaticRate();
            PlayerState playerState = new PlayerState(health);
            playerState.Regen();

            Assert.AreEqual(playerState.HP, health.MaxHP);
        }

        [Test]
        public void TakeDamage_ComboShouldReset()
        {
            PlayerState playerState = new PlayerState(new PlayerHPStaticRate());

            playerState.Regen();
            playerState.Regen();
            playerState.Regen();

            playerState.TakeDamage();

            Assert.AreEqual(playerState.Combo, 0);
        }

        [Test]
        public void TakeDamage_ShouldDecreaseHP()
        {
            PlayerHPStaticRate health = new PlayerHPStaticRate();
            PlayerState playerState = new PlayerState(health);
            
            playerState.TakeDamage();

            Assert.Greater(health.MaxHP, playerState.HP);
        }

        [Test]
        public void TakeDamage_OnZeroHP_HPShouldBeCappedAtZero()
        {
            PlayerHPStaticRate health = new PlayerHPStaticRate(damageRate: 1f); //instakill
            PlayerState playerState = new PlayerState(health);

            playerState.TakeDamage(); //already dead
            playerState.TakeDamage();

            Assert.AreEqual(playerState.HP, 0);
        }
    }
}
