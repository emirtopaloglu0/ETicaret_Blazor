using ETicaret_Application.DTOs.ProductDTOs;
using ETicaret_Application.Interfaces;
using ETicaret_Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret_Application.UseCases
{
    public class ProductDescriptionUseCase
    {
        private readonly IProductRepository _productRepository;
        private readonly ILLMClient _llmClient;
        private readonly TemplateDescriptionGenerator _templateGenerator;

        public ProductDescriptionUseCase(
            IProductRepository productRepository,
            ILLMClient llmClient,
            TemplateDescriptionGenerator templateGenerator)
        {
            _productRepository = productRepository;
            _llmClient = llmClient;
            _templateGenerator = templateGenerator;
        }

        /// <summary>
        /// Ürün için otomatik açıklama oluşturur, veritabanına yazar ve açıklamayı döner.
        /// </summary>
        public async Task<string> ExecuteGenerateDescriptionAsync(int productId)
        {
            var product = await _productRepository.GetByIdWithCompanyAndShopsAsync(productId);
            if (product == null) throw new KeyNotFoundException("Product not found");

            // Try LLM
            string generated = null;
            try
            {
                var prompt = BuildPrompt(product);
                generated = await _llmClient.GenerateTextAsync(prompt, maxTokens: 150);
            }
            catch
            {
                // LLM çağrısı başarısızsa fallback yapılacak
                generated = null;
            }

            if (string.IsNullOrWhiteSpace(generated))
            {
                generated = _templateGenerator.Generate(product);
            }

            //await _productRepository.UpdateAsync(product);

            return generated;
        }

        private string BuildPrompt(ProductDTO p)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Lütfen Türkçe kısa, pazarlama diliyle ve okunması hoş bir ürün açıklaması yaz. 35-45 kelime arası olsun. Bilgiler:");
            sb.AppendLine($"İsim: {p.Name}");
            sb.AppendLine($"Fiyat: {p.Price:C}");
            //if (!string.IsNullOrWhiteSpace(p.Description))
            //    sb.AppendLine($"Mevcut açıklama: {p.Description}");
            sb.AppendLine($"Stok: {p.Stock}");
            if (!string.IsNullOrWhiteSpace(p.CategoryName))
                sb.AppendLine($"Kategori: {p.CategoryName}");
            if (!string.IsNullOrEmpty(p.CategoryDesc))
                sb.AppendLine($"Kategori Açıklaması: {p.CategoryDesc}");
            //if (!string.IsNullOrEmpty(p.ShopName) && !string.IsNullOrEmpty(p.ShopDesc))
            //    sb.AppendLine($"Satıcının Adı: {p.ShopName} ve Satıcının Açıklaması: {p.ShopDesc}");
            sb.AppendLine("Sade, akıcı ve kullanıcıyı satın almaya yönlendirici bir ton kullan.");
            return sb.ToString();
        }

    }
}




