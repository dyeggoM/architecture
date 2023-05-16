using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TEST.Architectures.UnitOfWork.Entities
{
    [Table("CACHE_Entity1")]
    public class DbModel : BaseEntity
    {
        [MaxLength(50)]
        public string Name { get; set; }
    }
}
