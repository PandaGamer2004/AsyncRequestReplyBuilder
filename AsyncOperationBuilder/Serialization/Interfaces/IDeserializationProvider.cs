namespace AsyncOperationBuilder.Serialization.Interfaces
{
    public interface IDeserializationProvider<TInput, out TResult>
    {
        TResult Deserialize(TInput inputStream);
    }
}