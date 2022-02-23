using System;

namespace MarketAnalyzer.Core.Model
{
    public class Item : Entity
    {
        public string Name { get; set; }
        public DateTime RegistrationDate { get; set; }
    }
}
