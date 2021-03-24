namespace RectangleTrainer.ChromaTower.Engine
{
    public interface IDifficulty
    {
        int MaxSlots { get; }
        int NextSlot();
        void UpdateSingleColorStatus(int platformCount);
        bool NearDeath { get; }
    }
}
