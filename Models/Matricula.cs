using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace School.Models
{
    public class Matricula
    {   [Key]
        public int MatriculaId { get; set; }

        public string  AlunoId { get; set; }
        public virtual ApplicationUser Aluno {get; set;}

        public int     TurmaId { get; set; }
        public virtual Turma Turma { get; set; }

        [DataType(DataType.Date)]
        public DateTime Semestre { get; set; }

        public string Status { get; set; }  // Aberta, Efetivada
    }
}
