using ETicaret_Application.Interfaces;
using ETicaret_Core.Entities;
using ETicaret_Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret_Infrastructure.Data.Repositories
{
    public class EfDeliveryCompanyRepository : IDeliveryCompanyRepository
    {
        private readonly ETicaretDbContext _context;
        public EfDeliveryCompanyRepository(ETicaretDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(ETicaret_Core.Entities.DeliveryCompany deliveryCompany)
        {
            var company = new Entities.DeliveryCompany
            {
                Name = deliveryCompany.Name
            };

            await _context.DeliveryCompanies.AddAsync(company);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCompany(int id)
        {
            var company = _context.DeliveryCompanies.Find(id);
            _context.DeliveryCompanies.Remove(company);
            await _context.SaveChangesAsync();
        }

        public async Task<List<ETicaret_Core.Entities.DeliveryCompany>?> GetDeliveryCompanies()
        {
            var companies = await _context.DeliveryCompanies.ToListAsync();
            List<ETicaret_Core.Entities.DeliveryCompany> deliveryCompanies = new List<ETicaret_Core.Entities.DeliveryCompany>();
            foreach (var item in companies)
            {
                deliveryCompanies.Add(new ETicaret_Core.Entities.DeliveryCompany
                {
                    Id = item.Id,
                    Name = item.Name
                });
            }
            return deliveryCompanies;
        }

        public async Task<ETicaret_Core.Entities.DeliveryCompany?> GetDeliveryCompanyById(int id)
        {
            var company = await _context.DeliveryCompanies.FindAsync(id);
            return new ETicaret_Core.Entities.DeliveryCompany
            {
                Id = company.Id,
                Name = company.Name
            };
        }

        public async Task<ETicaret_Core.Entities.DeliveryCompany> UpdateCompany(int id, string name)
        {
            var company = await _context.DeliveryCompanies.FindAsync(id);
            company.Name = name;
            await _context.SaveChangesAsync();
            return new ETicaret_Core.Entities.DeliveryCompany
            {
                Id = company.Id,
                Name = company.Name
            };
        }
    }
}
