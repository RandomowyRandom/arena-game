namespace BuffSystem.Abstraction
{
    public interface IBuffEffect
    {
        public void OnApply(IBuffHandler buffHandler);
        public void OnRemove(IBuffHandler buffHandler);
        public void OnTick(IBuffHandler buffHandler);
    }
}