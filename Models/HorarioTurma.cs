using School.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace School.Models
{
    public class HorarioTurma
    {  
        public int HorarioId { get; set; }
        public int TurmaId { get; set; }
        
        public virtual Horario Horario { get; set; }

        public virtual Turma Turma { get; set; }


    }
}
