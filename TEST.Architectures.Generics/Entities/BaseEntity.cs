using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TEST.Architectures.Generics.Entities
{
    public class BaseEntity
    {
        [Key]
        [Display(Name = "Id", Description = "long")]
        public long Id { get; set; }
    }
}
