using System;

namespace Domain
{
    public class User
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Adress { get; set; }

        //Mortgage info
        public double Income { get; set; }
    }
}
