namespace RectangleTrainer.ChromaTower.Engine
{
    public class PlayerHPStaticRate : IPlayerHealth
    {
        public float RegenRate { get; private set; }
        public float DamageRate { get; private set; }
        public int MaxHP { get; private set; }

        public PlayerHPStaticRate(int maxHP = 100, float regenRate = 0.25f, float damageRate = 0.25f)
        {
            MaxHP = maxHP;
            RegenRate = regenRate;
            DamageRate = damageRate;
        }
    }
}
