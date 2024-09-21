namespace Fina.Core
{
    public static class Configuration
    {
        public const int DefaultPageNumber = 1;
        public const int DefaultPageSize = 25;
        public const int DefaultStatusCode = 200;

        public static string BackendUrl { get; set; } = "https://localhost:7179";
        public static string FrontendUrl { get; set; } = "https://localhost:7260";
    }
}
