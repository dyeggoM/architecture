using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TEST.Architectures.Generics.Entities
{
    [Table("TestModel")]
    public class TestModel : BaseEntity
    {
        [MaxLength(50)]
        [Display(Name = "Nombre", Description ="string")]
        public string Name { get; set; }

        [Display(Name = "Fecha", Description = "DateTime?")]
        public DateTime? Date { get; set; }

        [NotMapped]
        [Display(Name = "Lista strings", Description = "List<string>")]
        public List<string> List1 { get; set; }

        [NotMapped]
        [Display(Name = "Lista ints", Description = "List<int?>")]
        public List<int?> List2 { get; set; }

        //[NotMapped]
        //[Display(Name = "Tupla", Description = "Tupla<string,int>")]
        //public Tuple<string,int> Tuple { get; set; }

        [NotMapped]
        [Display(Name = "Modelo de prueba", Description = "DbModel")]
        public DbModel DbModel { get; set; }
    }
}
