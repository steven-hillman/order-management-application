using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steven_Hillman_CPRG200_Lab4
{
    public class Order_Details
    {
        // public properties
        public int OrderID { get; set; }

        public int ProductID { get; set; }

        public decimal UnitPrice { get; set; }

        public int Quantity { get; set; }

        public decimal Discount { get; set; }

        public decimal LineTotal { get; set; }
    }
}
