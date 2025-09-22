using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret_Application.Interfaces
{
    public interface IAuthService
    {
        Task<string> LoginAsync(string email, string password);
        Task RegisterAsync(string email, string FirstName, string LastName, string password, string role = "customer");
    }
}
