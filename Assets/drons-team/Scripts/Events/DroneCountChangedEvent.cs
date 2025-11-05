namespace DronsTeam.Events
{
    public readonly struct DroneCountChangedEvent
    {
        public readonly int NewCount;

        public DroneCountChangedEvent(int newCount)
        {
            NewCount = newCount;
        }
    }
}