using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class User
    {
        public Guid id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Adress { get; set; }

        //Mortgage info
        public double Income { get; set; }
    }
}
