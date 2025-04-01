namespace pv311_web_api.BLL
{
    public static class Settings
    {
        // paths
        public static string FilesRootPath = string.Empty;
        public const string ImagesPath = "images";
        public const string ManufacturesPath = "manufactures";
        public const string CarsPath = "cars";
        public const string UsersPath = "users";
        // roles
        public const string AdminRole = "admin";
        public const string UserRole = "user";
        // pagination
        public const int PageSize = 3;
    }
}