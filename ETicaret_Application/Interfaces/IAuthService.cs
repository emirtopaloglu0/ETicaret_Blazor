using ETicaret_Application.DTOs.UserDTOs;
using ETicaret_Core.Entities;
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
        Task<LoggedUserDto> GetLoggedUser(int id);
        Task<bool> UpdateUserAsync(int id, string email, string FirstName, string LastName, string password, string role = "customer");
        Task ChangeUserRole(int id, string role);
        Task<bool> CheckByMail(string mail);
    }
}
