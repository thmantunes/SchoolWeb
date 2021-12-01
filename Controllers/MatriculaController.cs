using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using School.Context;
using School.Models;
using School.ViewModels;

namespace School.Controllers
{
    public class MatriculaController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly CultureInfo culture;

        public MatriculaController(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            this.userManager = userManager;
            culture = new CultureInfo("pt-BR");

        }

        // GET: Matricula
        public async Task<IActionResult> Index()
        {
            var Usuario = await userManager.GetUserAsync(this.User);

            List<MatriculaViewModel> MatriculaVM = new List<MatriculaViewModel>();

            if (User.IsInRole("Aluno"))
            {
                var qryMatricula = (from m in _context.Matriculas
                                    where m.Aluno.Id.Equals(Usuario.Id)
                                    let DisciplinaNome = m.Turma.Disciplina.Nome
                                    let DisciplinaCodigo = m.Turma.Disciplina.DisciplinaId
                                    select new
                                    { m.MatriculaId,
                                        m.Turma.TurmaId,
                                        m.Turma.Nome,
                                        m.Turma.HorariosTurma,
                                        m.Status,
                                        DisciplinaCodigo,
                                        DisciplinaNome,
                                    }) ;

                ViewData["NroDisciplinas"] = qryMatricula.Count();
                ViewData["Status"] = qryMatricula.Any(x => x.Status == "Efetivada") ? "Efetivada" : "Aberta";

                foreach (var i in qryMatricula)
                {
                    MatriculaViewModel VM = new MatriculaViewModel();
                    VM.MatriculaId = i.MatriculaId;
                    VM.TurmaId = i.TurmaId;
                    VM.TurmaNome = i.Nome;
                    VM.DisciplinaCodigo = i.DisciplinaCodigo;
                    VM.DisciplinaNome = i.DisciplinaNome;

                    var horarios = (from t in _context.HorariosTurmas
                                   where t.TurmaId == i.TurmaId
                                   join h in _context.Horarios on t.HorarioId equals h.Id
                                   orderby h.IntDiaHora
                                   select h.DiaHora).ToList();


                    VM.Horarios = String.Join(", ", horarios);
                    MatriculaVM.Add(VM);
                }
            }

            return View(MatriculaVM);
        }

        // GET: Matricula/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var matricula = await _context.Matriculas
                .FirstOrDefaultAsync(m => m.MatriculaId == id);
            if (matricula == null)
            {
                return NotFound();
            }

            return View(matricula);
        }

        // GET: Matricula/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Matricula/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MatriculaId,Semestre")] Matricula matricula)
        {
            if (ModelState.IsValid)
            {
                _context.Add(matricula);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(matricula);
        }

        // GET: Matricula/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var matricula = await _context.Matriculas.FindAsync(id);
            if (matricula == null)
            {
                return NotFound();
            }
            return View(matricula);
        }

        // POST: Matricula/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MatriculaId,Semestre")] Matricula matricula)
        {
            if (id != matricula.MatriculaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(matricula);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MatriculaExists(matricula.MatriculaId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(matricula);
        }



        // GET: Matricula/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var matricula = await _context.Matriculas
                .FirstOrDefaultAsync(m => m.MatriculaId == id);
            if (matricula == null)
            {
                return NotFound();
            }

            var disciplina = from x in (from t in _context.Turmas
                                        where t.TurmaId == matricula.TurmaId
                                        select new { t.Disciplina.Nome })
                             select x.Nome;

            ViewData["Disciplina"] = disciplina.ToList()[0];
            return View(matricula);
        }

        // GET: Matricula
        // Lista Turmas da disciplina escolhida
        public async Task<IActionResult> Turmas(string id)
        {
            var UsuarioId = userManager.GetUserAsync(this.User).Result.Id;

            var qryTurmas = (from d in _context.Disciplinas where d.DisciplinaId == id
                             join t in _context.Turmas on d.DisciplinaId equals t.Disciplina.DisciplinaId
                             select new { t.Nome, t.Vagas, t.TurmaId, t.Professor.UserName}).ToList();

            // busca horarios das turmas matriculadas
            var qyrHorariosTurmasMatriculadas = (from x in (from m in _context.Matriculas
                                         where m.AlunoId == UsuarioId
                                         join t in _context.Turmas on m.TurmaId equals t.TurmaId
                                         join ht in _context.HorariosTurmas on t.TurmaId equals ht.TurmaId
                                         select ht.Horario.DiaHora)
                                                 select x).ToList();

            

            ViewData["Disciplina"] = _context.Disciplinas.FindAsync(id).Result.Nome;

                List<TurmaViewModel> TurmaVM = new List<TurmaViewModel>();
                foreach (var q in qryTurmas)
                {
                    TurmaViewModel VM = new TurmaViewModel();
                    VM.TurmaId = q.TurmaId;
                    VM.NomeTurma = q.Nome;
                    VM.NomeProfessor = q.UserName;
                    VM.NroVagas = q.Vagas;
                    

                    VM.NroVagasDisponiveis = q.Vagas - _context.Matriculas.Count(m => m.TurmaId == q.TurmaId);

                    var horariosTurma = (from x in (from ht in _context.HorariosTurmas
                                               where ht.TurmaId == q.TurmaId
                                               join h in _context.Horarios on ht.HorarioId equals h.Id
                                               orderby h.IntDiaHora
                                               select new { h.DiaHora })
                                        select x.DiaHora).ToList();

                
                    var conflito = qyrHorariosTurmasMatriculadas.Select(h => h).Intersect(horariosTurma);

                    VM.Conflito = (conflito.Count() > 0);
                    VM.HorariosTurma = String.Join(", ", horariosTurma);

                    TurmaVM.Add(VM);
                }

            return View(TurmaVM);
        }


        // GET: Matricula/EfetuarMatricula/id
        /*
        public async Task<IActionResult> EfetuarMatricula(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var matricula = await _context.Matriculas
                .FirstOrDefaultAsync(m => m.MatriculaId == id);
            if (matricula == null)
            {
                return NotFound();
            }

            return View(matricula);
        }
        */

        // POST: Matricula/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var matricula = await _context.Matriculas.FindAsync(id);
            _context.Matriculas.Remove(matricula);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: Emitir Comprovante
        [HttpPost, ActionName("Comprovante")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Comprovante()
        {
            var Usuario = await userManager.GetUserAsync(this.User);
            ViewData["Aluno"] = Usuario.UserName;
            ViewData["Data"] = DateTime.Now.ToString();

            var matricula = from m in _context.Matriculas
                            where m.AlunoId == Usuario.Id
                            join t in _context.Turmas on m.TurmaId equals t.TurmaId
                            join d in _context.Disciplinas on t.Disciplina.DisciplinaId equals d.DisciplinaId
                            let DisciplinaNome = d.Nome
                            orderby d.Nome
                            select new { t.Nome, d.DisciplinaId, DisciplinaNome };

            List < MatriculaViewModel > VMComprovante = new List<MatriculaViewModel>();

            foreach (var mat in matricula)
            {
                MatriculaViewModel VM = new MatriculaViewModel();
                VM.TurmaNome = mat.Nome;
                VM.DisciplinaCodigo = mat.DisciplinaId;
                VM.DisciplinaNome = mat.DisciplinaNome;
                VMComprovante.Add(VM);
            }
            return View(VMComprovante);
        }


        // POST: Matricula/EfetuarMatricula/id
        [HttpPost, ActionName("EfetuarMatricula")]
        [ValidateAntiForgeryToken]
        //         public async Task<IActionResult> Create(ProdutosViewModel model)
        public async Task<IActionResult> EfetuarMatricula(int TurmaId)
        {
            var Usuario = await userManager.GetUserAsync(this.User);
            var turma = await _context.Turmas.FindAsync(TurmaId);
            Matricula matricula = new Matricula();
            //matricula.AlunoId = Usuario.Id;
            matricula.Aluno = Usuario;
            matricula.Turma = turma;
            matricula.Semestre = DateTime.Today;
            matricula.AlunoId = Usuario.Id;
            matricula.Status = "Aberta";
            _context.Matriculas.Add(matricula);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MatriculaExists(int id)
        {
            return _context.Matriculas.Any(e => e.MatriculaId == id);
        }

        // POST: Matricula/EfetuarMatricula/id
        [HttpPost, ActionName("FinalizarMatricula")]
        [ValidateAntiForgeryToken]
        //         public async Task<IActionResult> Create(ProdutosViewModel model)
        public async Task<IActionResult> FinalizarMatricula()
        {
            var Usuario = await userManager.GetUserAsync(this.User);
            (from m in _context.Matriculas
             where m.AlunoId == Usuario.Id
             select m).ToList().ForEach(x => x.Status = "Efetivada");

            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

    }
}
