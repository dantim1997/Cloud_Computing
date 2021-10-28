using System;

namespace Domain
{
    public class User
    {
        public string id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Adress { get; set; }
        public string ZipCode { get; set; }

        //Mortgage info
        public double Income { get; set; }
        public string FileId { get; set; }
    }
}
