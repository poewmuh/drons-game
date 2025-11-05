namespace DronsTeam.Events
{
    public readonly struct DroneSpeedChangedEvent
    {
        public readonly float NewSpeed;

        public DroneSpeedChangedEvent(float speed)
        {
            NewSpeed = speed;
        }
    }
}