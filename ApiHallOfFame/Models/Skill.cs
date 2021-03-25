using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ApiHallOfFame.Models
{
    public class Skill
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Enter skill name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Enter level skill")]
        [Range(1,10)]
        public byte Level { get; set; }
        public long PersonId { get; set; }
        public Person Person { get; set; }
    }
}
