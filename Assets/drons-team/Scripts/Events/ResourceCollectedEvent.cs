namespace DronsTeam.Events
{
    public readonly struct ResourceCollectedEvent
    {
        public int FractionId { get; }

        public ResourceCollectedEvent(int fractionId)
        {
            FractionId = fractionId;
        }
    }
}
