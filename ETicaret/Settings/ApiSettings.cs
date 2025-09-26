namespace ETicaret_UI.Settings
{
    public class ApiSettings
    {
        public string BaseUrl { get; set; } = "https://localhost:7093/api/";
        public string userId { get; set; }


        #region User - Auth
        public string mail { get; set; }

        public string Login => $"{BaseUrl}Auth/login";
        public string Register => $"{BaseUrl}Auth/register";
        public string GetCurrentUser => $"{BaseUrl}User/me";
        public string CheckByMail => $"{BaseUrl}User/CheckByMail/{mail}";
        public string GetByMail => $"{BaseUrl}User/GetByMail/{mail}";
        public string ChangeRole => $"{BaseUrl}User/changeRole/{userId}";
        #endregion

        #region Shop
        public int shopId { get; set; }

        public string GetShops => $"{BaseUrl}Shop";
        public string UpdateShop => $"{BaseUrl}Shop/{shopId}";
        public string DeleteShop => $"{BaseUrl}Shop/{shopId}";
        public string AddShop => $"{BaseUrl}Shop";
        #endregion

        #region ProductCategory
        public int categoryId { get; set; }

        public string GetCategories => $"{BaseUrl}ProductCategory";
        public string AddCategory => $"{BaseUrl}ProductCategory";
        public string UpdateCategory => $"{BaseUrl}ProductCategory/{categoryId}";
        public string DeleteCategory => $"{BaseUrl}ProductCategory/{categoryId}";


        #endregion

        #region DeliveryCompanies
        public int companyId { get; set; }

        public string GetCompanies => $"{BaseUrl}DeliveryCompanies";
        public string AddCompany => $"{BaseUrl}DeliveryCompanies";
        public string UpdateCompany => $"{BaseUrl}DeliveryCompanies/{companyId}";
        public string DeleteCompany => $"{BaseUrl}DeliveryCompanies/{companyId}";
        #endregion

        public string Products => $"{BaseUrl}products";

    }
}
