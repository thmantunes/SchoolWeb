using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using School.Context;
using School.Models;
using School.ViewModels;

namespace SchoolWeb.Controllers
{

    public class RelatoriosController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> userManager;


        public RelatoriosController(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            this.userManager = userManager;

        }


        // GET: RelatoriosController
        // Percentual de ocupação de cada uma das disciplinas do curso, por turmas.
        public ActionResult RelatorioOcupacao()
        {
            var ocupacaoTurma = from x in (
                           from m in _context.Matriculas
                           join t in _context.Turmas on m.TurmaId equals t.TurmaId
                           join d in _context.Disciplinas on t.Disciplina.DisciplinaId equals d.DisciplinaId
                           group t by new { t.TurmaId, t.Disciplina.DisciplinaId } into gp
                           select new { gp.Key.TurmaId, gp.Key.DisciplinaId, ocupacao = gp.Count() })
                                join t in _context.Turmas on x.TurmaId equals t.TurmaId
                                join d in _context.Disciplinas on t.Disciplina.DisciplinaId equals d.DisciplinaId
                                let lotacao = t.Vagas / (decimal)x.ocupacao
                                let disciplinaNome = d.Nome
                                select new { t.Nome, disciplinaNome, t.Vagas, lotacao };


            List<RelatorioOcupacaoViewModel> VMOcupacao = new List<RelatorioOcupacaoViewModel>();
            foreach (var x in ocupacaoTurma)
            { RelatorioOcupacaoViewModel VM = new RelatorioOcupacaoViewModel();
                VM.NomeTurma = x.Nome;
                VM.Disciplina = x.disciplinaNome;
                VM.NroVagas = x.Vagas;
                VM.Ocupacao = x.lotacao;
                VMOcupacao.Add(VM);
            }
            return View(VMOcupacao);
        }

        // GET: RelatoriosController
        // Alunos matriculados em uma determinada disciplina
        public ActionResult RelatorioAlunos(string disciplina)
        {
            ViewBag.Disciplinas = from d in _context.Disciplinas
                                  orderby d.Nome
                                  select d.Nome;

            var alunos = (from d in _context.Disciplinas where d.Nome == disciplina
                         join t in _context.Turmas on d.DisciplinaId equals t.Disciplina.DisciplinaId
                         join m in _context.Matriculas on t.TurmaId equals m.TurmaId
                         join u in _context.Users on m.AlunoId equals u.Id
                         orderby u.UserName
                         select new { u.UserName }).Distinct();

            List<RelatorioAlunosViewModel> VMAlunos = new List<RelatorioAlunosViewModel>();
            foreach (var a in alunos)
            {
                RelatorioAlunosViewModel VM = new RelatorioAlunosViewModel();
                VM.NomeAluno = a.UserName;
                VMAlunos.Add(VM);


            }
            return View(VMAlunos);
        }

        // GET: RelatoriosController
        // Total de alunos matriculados e o número médio de créditos matriculados
        public ActionResult RelatorioEstatisticas()
        {
            var creditos = from m in _context.Matriculas
                           join t in _context.Turmas on m.TurmaId equals t.TurmaId
                           join d in _context.Disciplinas on t.Disciplina.DisciplinaId equals d.DisciplinaId
                           select d.Creditos;


            int TotalMatriculados = _context.Matriculas.GroupBy(x => x.AlunoId).Count();
            ViewData["TotalMatriculados"] = TotalMatriculados;
            ViewData["MediaCreditos"] = creditos.Sum()/TotalMatriculados;
            return View();
        }

    }
}
