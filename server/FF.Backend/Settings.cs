namespace FF.Backend
{
    public class Settings: ISettings
    {
        public string ConnectionString { get; set; }
        public ICorsSettings Cors { get; set; } = new CorsSettings();
    }

    public class CorsSettings: ICorsSettings
    {
        public string[] AllowedOrigins { get; set; }
        public string[] AllowedMethods { get; set; }

    }
}
