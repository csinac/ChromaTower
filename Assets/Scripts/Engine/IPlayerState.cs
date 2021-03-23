namespace RectangleTrainer.ChromaTower.Engine
{
    public interface IPlayerState
    {
        int HP { get; }
        float HPNormalized { get; }
        int Combo { get; }
        bool IsDead { get; }

        void TakeDamage();
        void Regen();
        void Reset();
    }
}
