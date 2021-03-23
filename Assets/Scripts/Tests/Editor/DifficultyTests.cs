using NUnit.Framework;

namespace RectangleTrainer.ChromaTower.UnitTests
{
    using Engine;

    class DifficultyTests
    {
        [Test]
        public void Difficulty_MaxColorsLessThanOne_ShouldThrowException()
        {
            PlayerState player = new PlayerState(new PlayerHPStaticRate());
            Assert.Throws<System.Exception>(() => new Difficulty(player, maxSlots: 0));
        }

        [Test]
        public void Difficulty_NullPlayer_ShouldThrowException()
        {
            Assert.Throws<System.Exception>(() => new Difficulty(null));
        }

        [Test]
        public void NextColor_DyingPlayer_ShouldGetSingleColorChance()
        {
            PlayerHPStaticRate health = new PlayerHPStaticRate(damageRate: 0.33f);
            PlayerState player = new PlayerState(health);

            player.TakeDamage(); // hp normalized 0.67
            player.TakeDamage(); // hp normalized 0.34
            player.TakeDamage(); // hp normalized 0.01

            Difficulty diff = new Difficulty(player);
            int testCount = 99;

            int[] control = new int[testCount];
            int[] colors = new int[testCount];

            colors[0] = diff.NextSlot();
            control[0] = colors[0];
            
            for(int i = 1; i < testCount; i++)
            {
                control[i] = control[0];
                colors[i] = diff.NextSlot();
            }

            CollectionAssert.AreEqual(control, colors);
        }
    }
}
