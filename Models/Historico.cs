using School.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace School.Models
{
    public class Historico
    {
        public Historico()
        {


        }
        public int HistoricoId { get; set; }
        public virtual ApplicationUser Aluno { get; set; }
        public virtual Disciplina Disciplina { get; set; }
        public DateTime DataConclusao { get; set; }
        public string Conceito { get; set; }

    }
}
