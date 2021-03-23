using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApiHallOfFame.Models
{
    public class Person
    {
        public long Id { get; set; }
        [Required(ErrorMessage = "Enter your name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Enter your display name")]
        public string DisplayName { get; set; }
        public virtual ICollection<Skill> Skills { get; set; }
    }
}
