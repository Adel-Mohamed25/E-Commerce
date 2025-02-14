namespace API.Routing
{
    public static class Router
    {
        private const string Root = "api";
        private const string Version = "V1";

        private const string BaseRoute = $"{Root}/{Version}";

        public static string GetAll = $"{BaseRoute}/GetAll";

        public static string GetById = $"{BaseRoute}/GetById";

        public static string Post = $"{BaseRoute}/Post";

        public static string Put = $"{BaseRoute}/Put";

        public static string Delete = $"{BaseRoute}/Delete";
    }

}
