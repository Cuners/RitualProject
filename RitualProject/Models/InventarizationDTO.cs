using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RitualProject
{
    public class InventarizationDTO
    {
        public int CompositionID { get; set; }
        public string Product {  get; set; }
        public int Quantity { get;set; }
        public int QuantityFact {  get; set; }
        public int Price { get; set; }
        public int Prices {  get; set; }
        public int PriceFact {  get; set; }
    }
}
