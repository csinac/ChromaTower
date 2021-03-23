namespace RectangleTrainer.ChromaTower.Engine
{
    public class Difficulty: IDifficulty
    {
        public int MaxSlots { get; private set; }
        private int lastSlot;
        private IPlayerState player;
        private float almostDeadLimit;

        public Difficulty(IPlayerState playerState, int maxSlots = 8, float almostDeadLimit = 0.1f)
        {
            if (maxSlots < 1)
                throw new System.Exception("Max Slots should be at least 1");

            if (playerState == null)
                throw new System.Exception("Player cannot be null");

            MaxSlots = maxSlots;
            player = playerState;
            this.almostDeadLimit = almostDeadLimit;
        }

        public int NextSlot()
        {
            if(player.HPNormalized < almostDeadLimit)
                return lastSlot;

            lastSlot = UnityEngine.Random.Range(0, MaxSlots);
            return lastSlot;
        }
    }
}
