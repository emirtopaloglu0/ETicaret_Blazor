using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret_Application.Interfaces
{
    public interface ILLMClient
    {
        Task<string> GenerateTextAsync(string prompt, int maxTokens = 150);
    }
}
