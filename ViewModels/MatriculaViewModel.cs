using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace School.ViewModels
{
    public class MatriculaViewModel
    {
        public int MatriculaId;
        public int TurmaId;
        [Display(Name = "Turma")]
        public string TurmaNome;
        [Display(Name = "Disciplina")]
        public string DisciplinaCodigo;
        [Display(Name="")]
        public string DisciplinaNome;
        [Display (Name="Horários")]
        public string Horarios;

        public string Status;
    }
}
