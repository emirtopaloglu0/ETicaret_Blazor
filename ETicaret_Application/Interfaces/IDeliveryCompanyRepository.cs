using ETicaret_Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret_Application.Interfaces
{
    public interface IDeliveryCompanyRepository
    {
        Task AddAsync(DeliveryCompany deliveryCompany);
        Task<List<DeliveryCompany>> GetDeliveryCompanies();
        Task<DeliveryCompany> GetDeliveryCompanyById(int id);
        Task<DeliveryCompany> UpdateCompany(int id, string name);
        Task DeleteCompany(int id);
    }
}
