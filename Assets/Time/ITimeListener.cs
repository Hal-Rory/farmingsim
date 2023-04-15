namespace GameTime
{
    public interface ITimeListener
    {
        /// <summary>
        /// Clock update has occurred
        /// </summary>
        /// <param name="timestamp"></param>
        public void ClockUpdate(TimeStruct timestamp);
        public void Register();
        public void Unregister();
    }
}