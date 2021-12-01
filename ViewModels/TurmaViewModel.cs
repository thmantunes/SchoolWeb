using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace School.ViewModels
{
    public class TurmaViewModel
    {
        public int TurmaId;
        [Display(Name = "Turma")]
        public string NomeTurma;

        [Display(Name = "Professor")]
        public string NomeProfessor;

        [Display(Name = "Disciplina")]
        public string Disciplina;

        [Display(Name = "Vagas")]
        public int NroVagas;

        [Display(Name = "Disponiveis")]
        public int NroVagasDisponiveis;

        [Display(Name = "Horários")]
        public string HorariosTurma;

        public Boolean Conflito;

    }
}
