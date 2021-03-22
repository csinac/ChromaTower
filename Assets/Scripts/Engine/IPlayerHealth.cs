namespace RectangleTrainer.ChromaTower.Engine
{
    public interface IPlayerHealth
    {
        float RegenRate { get; }
        float DamageRate { get; }
        int MaxHP { get; }

    }
}
