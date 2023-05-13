namespace GameTime
{
    public interface ITimeListener
    {
        /// <summary>
        /// Clock update has occurred
        /// </summary>
        /// <param name="tick"></param>
        public void ClockUpdate(int tick);
        public void Register();
        public void Unregister();
    }
}