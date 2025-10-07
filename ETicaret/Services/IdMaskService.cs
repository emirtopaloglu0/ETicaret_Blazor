using Microsoft.AspNetCore.DataProtection;


namespace ETicaret_UI.Services
{
    public class IdMaskService
    {
        private readonly IDataProtector _protector;

        public IdMaskService(IDataProtectionProvider provider)
        {
            _protector = provider.CreateProtector("IdMasking");
        }

        public string Encode(int id) => _protector.Protect(id.ToString());
        public int Decode(string value) => int.Parse(_protector.Unprotect(value));

    }
}
