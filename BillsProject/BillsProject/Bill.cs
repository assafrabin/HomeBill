using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillsProject
{
    public class Bill
    {
        public string Name { get; set; }
        public int amount { get; set; }
        public string picturePath { get; set; }
        public bool IsPaid { get; set;}
    }
}
