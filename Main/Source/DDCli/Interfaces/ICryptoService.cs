namespace DDCli.Interfaces
{
    public interface ICryptoService
    {
        string Decrypt(string str);
        string Encrypt(string str);
    }
}