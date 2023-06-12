namespace ObsvSystem
{
    public struct EventInfo<Arg>
    {
        public readonly int eventId;
        public readonly Arg arg;

        public EventInfo(int eventId, Arg arg)
        {
            this.eventId = eventId;
            this.arg = arg;
        }
    }
}