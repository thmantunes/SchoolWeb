using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace School.Models
{
    public class Disciplina
    {
        public Disciplina()
        {
        }

            public Disciplina (string DisciplinaId, int Creditos, string Nome, string Dependencia)
        {
            this.DisciplinaId = DisciplinaId;
            this.Nome = Nome;
            this.Creditos= Creditos;
            this.Dependencia = Dependencia;
        }

        [Key]
        [StringLength(20)]
        [Display(Name="Código")]
        public string DisciplinaId { get; set; }

        [Display(Name = "Créditos")]
        public int Creditos { get; set; }

        public string Nome { get; set; }

        [Display(Name = "Dependência")]
        public string Dependencia { get; set; }
        //public  ICollection<Disciplina> Precursoras { get; set; }
        public  ICollection<Turma> Turmas { get; set; }
    }
}
