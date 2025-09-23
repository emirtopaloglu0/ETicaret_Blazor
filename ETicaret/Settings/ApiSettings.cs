namespace ETicaret_UI.Settings
{
    public class ApiSettings
    {
        public string BaseUrl { get; set; } = "https://localhost:7093/api/";

        public string Login => $"{BaseUrl}Auth/login";
        public string Register => $"{BaseUrl}Auth/register";
        public string Products => $"{BaseUrl}products";
        public string GetCurrentUser => $"{BaseUrl}User/me";
    }
}
