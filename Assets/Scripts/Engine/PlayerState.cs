namespace RectangleTrainer.ChromaTower.Engine
{
    public class PlayerState: IPlayerState
    {
        IPlayerHealth health;
        public int HP { get; private set; }
        public float HPNormalized { get => 1f * HP / health.MaxHP; }
        public int Combo { get; private set; }
        public bool IsDead { get => HP == 0; }

        public PlayerState(IPlayerHealth health)
        {
            if(health == null)
            {
                throw new System.Exception("Player Health cannot be null");
            }
            HP = health.MaxHP;
            this.health = health;
            Combo = 0;
        }

        public void Regen()
        {
            Combo++;

            if (HP == health.MaxHP)
                return;

            int regen = (int)((health.MaxHP - HP) * health.RegenRate);
            if (regen < 1) regen = 1;

            HP += regen;
        }

        public void Reset()
        {
            HP = health.MaxHP;
            Combo = 0;
        }

        public void TakeDamage()
        {
            HP -= (int)(health.MaxHP * health.DamageRate);            
            if (HP < 0)
                HP = 0;

            Combo = 0;
        }
    }
}
