namespace Projectvil.Shared.Infrastructures.Constants;

public static class ProjectivConstants
{
    public const string AllowChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+/ абвгдеёжзийклмнопрстуфхцчшщъыьэюяАБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";
    public static class Microservice
    {
        public const string GetWayService = "GetWay";
        
        public const string PetProjectService = "PetProject";
        public const string IdentityService = "Identity";
        public const string AuthService = "Auth";
        public const string NotificationService = "Notification";
    }
    
    public static class Api
    {
        public const string GetWayApi = "GetWayApi";
        
        public const string PetProjectApi = "PetProjectApi";
        public const string IdentityApi = "IdentityApi";
        public const string AuthApi = "AuthApi";
        public const string NotificationApi = "NotificationApi";
    }

    public static class Language
    {
        public const string Ru = "ru-ru";
        public const string En = "en-US";
    }
    
    public static class Role
    {
        public const string User = "User";
        public const string Admin = "Admin";
    }
}