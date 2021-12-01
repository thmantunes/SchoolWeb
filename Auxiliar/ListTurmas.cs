using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace School.Auxiliar
{
    public class ListTurmas
    {
        public ListTurmas(int turmaId, string nome, string professor, string disciplinaNome, int vagas)
        {
            TurmaId = turmaId;
            Nome = nome;
            Professor = professor;
            DisciplinaNome = disciplinaNome;
            Vagas = vagas;
        }

        public int TurmaId;
        public string Nome;
        public string Professor;
        public string DisciplinaNome;
        public int Vagas;
            
    }
}
