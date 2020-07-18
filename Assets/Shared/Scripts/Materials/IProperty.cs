namespace Things
{
    public interface IProperty<T>
    {
        T Value { get; set; }
        void Invalidate();
    }
}