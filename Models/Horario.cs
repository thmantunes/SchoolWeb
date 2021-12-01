using School.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace School.Models
{
    public class Horario
    {   public Horario(int intDiaHora, string DiaHora)
        {
            this.IntDiaHora = intDiaHora;
            this.DiaHora = DiaHora;
        }

        [Key]
        public int Id { get; set; }
        public int IntDiaHora { get; set; }
        public string DiaHora { get; set; }

        public virtual ICollection<HorarioTurma> HorarioTurma { get; set; }
    }
}
