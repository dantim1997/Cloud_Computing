using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class House
    {
        public string id { get; set; }
        public float Price { get; set; }
        public string Title { get; set; }
        public string ZipCode { get; set; }
        public string Address { get; set; }
        public string PictureUrl { get; set; }
    }
}
