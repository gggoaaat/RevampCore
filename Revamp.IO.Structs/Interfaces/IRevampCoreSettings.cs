namespace Revamp.IO.Structs.Models
{
    public interface IRevampCoreSettings
    {
        string DbConnect { get; set; }
        string SystemDBName { get; set; }
        string Platform { get; set; }
        bool EnableEventLogging { get; set; }
        bool IsCacEnabled { get; set; }
        string AssetsUrl { get; set; }
        LogoStruct Logo { get; set; }
        bool ShowDisclaimer { get; set; }
    }
}