namespace DronsTeam.Events
{
    public readonly struct PathVisualizationToggleEvent
    {
        public readonly bool IsEnabled;

        public PathVisualizationToggleEvent(bool isEnabled)
        {
            IsEnabled = isEnabled;
        }
    }
}