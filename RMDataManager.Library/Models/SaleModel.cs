using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMDataManager.Library.Models
{
    public class SaleModel
    {
        public int Id { get; }

        public string CashierId { get; set; }

        public DateTime SaleDate { get; set; } = DateTime.UtcNow;

        public decimal SubTotal { get; set; }

        public decimal Total { get; set; }

        public decimal Tax { get; set; }
    }
}
