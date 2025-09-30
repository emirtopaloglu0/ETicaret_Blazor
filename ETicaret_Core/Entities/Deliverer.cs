using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret_Core.Entities
{
    public class Deliverer
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
    }
}
