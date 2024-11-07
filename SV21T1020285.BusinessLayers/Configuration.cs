namespace SV21T1020285.BusinessLayers 
{
    public static class Configuration 
    {
        private static string connectionString = "";
        
        public static void Initialize(string connectionString) 
        {
            Configuration.connectionString = connectionString;
        }

        public static string ConnectionString 
        {
            get {
                return connectionString;
            }
        }
    }
}
