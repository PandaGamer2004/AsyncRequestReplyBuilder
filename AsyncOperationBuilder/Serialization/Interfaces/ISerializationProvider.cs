namespace AsyncOperationBuilder.Serialization.Interfaces
{
    public interface ISerializationProvider<TInputStream, TSerializationStream>
    {
        TSerializationStream Serialize(TInputStream stream);
    }
}