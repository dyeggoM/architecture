﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TEST.Architectures.Generics.Entities
{
    [Table("CACHE_Entity1")]
    public class DbModel : BaseEntity
    {
        [MaxLength(50)]
        [Display(Name = "Nombre", Description = "string")]
        public string Name { get; set; }

    }

}
