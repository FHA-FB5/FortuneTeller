using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Gruppenverteilung.Code;

namespace Gruppenverteilung.Models
{
    public class DataInputModel
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]
        [MaxLength(100)]
        public string Vorname { get; set; }
        [Required]
        [Range(1,100)]
        public int Alter { get; set; }
        [Required]
        public Studiengang Studiengang { get; set; }
        [Required]
        public Geschlecht Geschlecht { get; set; }

        public DataInputModel()
        {

        }
    }
}
