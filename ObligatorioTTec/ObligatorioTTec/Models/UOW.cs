using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObligatorioTTec.Models
{
    
    internal class UOW
    {
        private readonly ApiService _apiService;
        public UOW()
        {
            _apiService = new ApiService();
        }
        
    }
}
