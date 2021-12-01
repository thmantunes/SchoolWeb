using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace School.ViewModels
{
    public class RelatorioOcupacaoViewModel
    {
        [Display(Name = "Turma")]
        public string NomeTurma;


        [Display(Name = "Disciplina")]
        public string Disciplina;

        [Display(Name = "Vagas")]
        public int NroVagas;

        [Display(Name = "Ocupação")]
        public decimal Ocupacao;

    }
}
