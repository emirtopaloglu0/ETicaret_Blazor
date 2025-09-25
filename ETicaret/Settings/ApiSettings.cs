namespace ETicaret_UI.Settings
{
    public class ApiSettings
    {
        public string BaseUrl { get; set; } = "https://localhost:7093/api/";
        public string userId { get; set; }
        public string mail { get; set; }


        public string Login => $"{BaseUrl}Auth/login";
        public string Register => $"{BaseUrl}Auth/register";
        public string Products => $"{BaseUrl}products";
        public string GetCurrentUser => $"{BaseUrl}User/me";
        public string CheckByMail => $"{BaseUrl}User/CheckByMail/{mail}";
    }
}
