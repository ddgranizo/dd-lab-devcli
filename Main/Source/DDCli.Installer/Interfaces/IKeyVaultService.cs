namespace DDCli.Installer.Interfaces
{
    public interface IKeyVaultService
    {
        string GetValueSecretFromKeyVault(string keyVaultName, string secretName);
    }
}