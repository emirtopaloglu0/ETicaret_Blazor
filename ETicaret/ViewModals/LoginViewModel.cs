using System.ComponentModel.DataAnnotations;

namespace ETicaret_UI.ViewModals
{
    public class LoginViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "E-Posta Adersinizi Lütfen Giriniz"),
            EmailAddress(ErrorMessage = "E-Posta Geçerli Değil.")]
        public string? Email { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Lütfen Şifrenizi Giriniz")]
        public string? Password { get; set; }
    }
}
