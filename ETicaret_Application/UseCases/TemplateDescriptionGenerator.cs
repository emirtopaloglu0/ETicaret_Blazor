using ETicaret_Application.DTOs.ProductDTOs;
using ETicaret_Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret_Application.UseCases
{
    public class TemplateDescriptionGenerator
    {
        public string Generate(ProductDTO p)
        {
            // Basit, güvenilir fallback şablonu
            var shortDesc = string.IsNullOrWhiteSpace(p.Description)
                ? ""
                : p.Description.Split('.').FirstOrDefault()?.Trim();

            return $"{p.Name} — {shortDesc} Fiyatı {p.Price:C}. Stokta {p.Stock} adet var. Şimdi keşfedin!";
        }
    }
}
