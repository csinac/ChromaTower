namespace RectangleTrainer.ChromaTower.Engine
{
    public class Difficulty: IDifficulty
    {
        public int MaxSlots { get; private set; }

        private int lastSlot;
        private IPlayerState player;
        private float almostDeadLimit;
        private bool forceSingleColor = true;
        private int introPlatforms;

        public Difficulty(IPlayerState playerState, int maxSlots = 8, int introPlatforms = 0, float almostDeadLimit = 0.1f)
        {
            if (maxSlots < 1)
                throw new System.Exception("Max Slots should be at least 1");

            if (playerState == null)
                throw new System.Exception("Player cannot be null");

            MaxSlots = maxSlots;
            lastSlot = UnityEngine.Random.Range(0, MaxSlots);
            player = playerState;
            this.almostDeadLimit = almostDeadLimit;
            this.introPlatforms = introPlatforms;
        }

        public void UpdateSingleColorStatus(int platformCount)
        {
            forceSingleColor = platformCount < introPlatforms;
        }

        public bool NearDeath
        {
            get => player.HPNormalized < almostDeadLimit;
        }

        public int NextSlot()
        {
            if(NearDeath || forceSingleColor)
                return lastSlot;

            lastSlot = UnityEngine.Random.Range(0, MaxSlots);
            return lastSlot;
        }
    }
}
