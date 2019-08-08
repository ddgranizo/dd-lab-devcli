namespace DDCli.Interfaces
{
    public interface IRegistryService
    {
        string GetValue(string key);
        void SetValue(string key, string value);
    }
}