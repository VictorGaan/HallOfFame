using System;
using System.ComponentModel.DataAnnotations;

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
        public virtual Person Person { get; set; }
    }
}
