using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TEST.Architectures.UnitOfWork.Entities
{
    public class BaseEntity
    {
        [Key]
        public long Id { get; set; }
    }
}
