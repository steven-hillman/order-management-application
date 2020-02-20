using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steven_Hillman_CPRG200_Lab4
{
    public class Orders
    {
        // public properties
        public int OrderID  { get; set; }

        public string CustomerID { get; set; }

        public DateTime? OrderDate  { get; set; }

        public DateTime? RequiredDate { get; set; }

        public DateTime? ShippedDate { get; set; }
    }
}
