namespace ETicaret_UI.Settings
{
    public class ApiSettings
    {
        public string BaseUrl { get; set; } = "https://localhost:7093/api/";
        public string userId { get; set; }


        #region User - Auth
        public string mail { get; set; }
        public string password { get; set; }

        public string Login => $"{BaseUrl}Auth/login";
        public string Register => $"{BaseUrl}Auth/register";
        public string GetCurrentUser => $"{BaseUrl}User/me";
        public string CheckByMail => $"{BaseUrl}User/CheckByMail/{mail}";
        public string GetByMail => $"{BaseUrl}User/GetByMail/{mail}";
        public string ChangeRole => $"{BaseUrl}User/changeRole/{userId}";
        public string UpdateUser => $"{BaseUrl}User/me";
        public string IsPasswordRight => $"{BaseUrl}User/IsPasswordRight/{password}/{userId}";
        #endregion

        #region Shop / ShopUser
        public int shopId { get; set; }

        public string GetShops => $"{BaseUrl}Shop";
        public string GetShopById => $"{BaseUrl}Shop/{shopId}";
        public string UpdateShop => $"{BaseUrl}Shop/{shopId}";
        public string DeleteShop => $"{BaseUrl}Shop/{shopId}";
        public string AddShop => $"{BaseUrl}Shop";

        public string GetShopByUserId => $"{BaseUrl}Shop/GetShopUserById/{userId}";
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
        public string GetCompanyByUserId => $"{BaseUrl}DeliveryCompanies/GetCompanyByUser/{userId}";
        public string GetCompanyById => $"{BaseUrl}DeliveryCompanies/{companyId}";
        #endregion

        #region Products
        public int productId { get; set; }
        public string Products => $"{BaseUrl}Product";
        public string GetProductsByShopId => $"{BaseUrl}Product/byShop/{shopId}";
        public string AddProduct => $"{BaseUrl}Product";
        public string UpdateProduct => $"{BaseUrl}Product/{productId}";
        public string DeleteProduct => $"{BaseUrl}Product/{productId}";

        #endregion

        #region Orders
        public int orderId { get; set; }

        public string CompleteOrder => $"{BaseUrl}Orders";
        public string GetOrdersByUser => $"{BaseUrl}Orders/byUser";
        public string GetOrderWithItems => $"{BaseUrl}Orders/withItems/{orderId}";
        public string UpdateOrderStatus => $"{BaseUrl}Orders/updateCargo/{orderId}";
        public string GetOrdersByCompanyId => $"{BaseUrl}Orders/GetByCompanyId/{companyId}";
        #endregion

    }
}
