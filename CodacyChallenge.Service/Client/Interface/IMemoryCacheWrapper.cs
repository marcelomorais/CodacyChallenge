namespace CodacyChallenge.Service.Client.Interface
{
    public interface IMemoryCacheWrapper
    {
        void Add(string key, object value); 
        T Get<T>(string key); 
    }
}
