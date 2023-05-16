using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TEST.Architectures.Generics.Entities
{
    public class DTOMapping
    {
        public string ModelName { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string DataType { get; set; }
        public string ElementType { get; set; }
        public bool IsNullable { get; set; }
    }
}
