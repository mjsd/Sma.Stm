using System.Collections.Generic;

namespace Sma.Stm.Ssc.Contract
{
    public class IdRegistryResponeObject
    {
        public List<Organization> Content { get; set; }
        public bool Last { get; set; }
        public int TotalPages { get; set; }
        public int TotalElements { get; set; }
        public int NumberOfElements { get; set; }
        public bool First { get; set; }
        public object Sort { get; set; }
        public int Size { get; set; }
        public int Number { get; set; }
    }
}
