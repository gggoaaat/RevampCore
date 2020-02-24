namespace Revamp.IO.Structs.Models
{
    public class RevampCoreSettings : IRevampCoreSettings
    {
        public string DbConnect { get; set; }
        public string SystemDBName { get; set; }
        public string Platform { get; set; }
        public bool EnableEventLogging { get; set; }
        public bool IsCacEnabled { get; set; }
        public string AssetsUrl { get; set; }
        public LogoStruct Logo { get; set; }
        public bool ShowDisclaimer { get; set; }
    }

    
    public class LogoStruct
    {
        public string Path { get; set; }
        public cssStruct css { get; set; }
    }

    public class cssStruct
    {
        public string width { get; set; }
        public string margin { get; set; }
    }
}
