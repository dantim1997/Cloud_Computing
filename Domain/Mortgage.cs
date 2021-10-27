using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class Mortgage
    {
        public Guid id { get; set; }
        public string Email { get; set; }
        public double MortgageAmount { get; set; }
    }
}
