namespace Revamp.IO.Structs.Models
{
    public interface IRevampCoreSettings
    {
        string DbConnect { get; set; }
        string SystemDBName { get; set; }
        string Platform { get; set; }
        bool EnableEventLogging { get; set; }
    }
}