namespace FF.Backend
{
    public interface ISettings
    {
        string ConnectionString { get; set; }
        public ICorsSettings Cors { get; set; }
    }

    public interface ICorsSettings
    {
        public string[] AllowedOrigins { get; set; }
        public string[] AllowedMethods { get; set; }

    }
}
