using System.ComponentModel.DataAnnotations;

namespace ETicaret_UI.Auth
{
    public class RegisterViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "E-Posta Adersinizi Lütfen Giriniz"),
            EmailAddress(ErrorMessage = "E-Posta Geçerli Değil.")]
        public string? Email { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Lütfen Adınızı Giriniz")]
        public string? FirstName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Lütfen Soyadınızı Giriniz")]
        public string? LastName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Lütfen Şifrenizi Giriniz")]
        public string? Password { get; set; }

    }
}
