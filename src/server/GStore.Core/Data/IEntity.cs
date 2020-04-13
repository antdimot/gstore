namespace GStore.Core.Data
{
    public interface IEntity<T>
    {
        T Id { get; set; }
    }
}
