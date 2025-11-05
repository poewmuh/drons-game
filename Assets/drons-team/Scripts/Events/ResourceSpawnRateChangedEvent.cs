namespace DronsTeam.Events
{
    public readonly struct ResourceSpawnRateChangedEvent
    {
        public readonly float NewRate;

        public ResourceSpawnRateChangedEvent(float rate)
        {
            NewRate = rate;
        }
    }
}