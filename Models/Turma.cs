using School.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace School.Models
{
    public class Turma
    {
        public Turma()
        {
            //Alunos = new HashSet<ApplicationUser>();

        }

        public Turma(string Nome, int Vagas, Disciplina Disciplina, ApplicationUser Professor)
        {
            this.Nome = Nome;
            this.Vagas = Vagas;
            this.Disciplina = Disciplina;
            this.Professor = Professor;
           
        }
        [Key]
        public int TurmaId { get; set; }
        [Required(ErrorMessage = "Nome é obrigatório.")]
        public string Nome { get; set; }
        public int Vagas { get; set; }
        public virtual Disciplina Disciplina {get; set;}
        public virtual ApplicationUser Professor { get; set; }

        // public int HorarioTurmaId { get; set; }

        //[InverseProperty("HorarioTurma")]
        public virtual ICollection<HorarioTurma> HorariosTurma { get; set; }
        public virtual ICollection<Matricula> Matriculas { get; set; }

    }
}
