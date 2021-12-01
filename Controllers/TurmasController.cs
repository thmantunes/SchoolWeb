using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using School.Auxiliar;
using School.Context;
using School.Models;
using School.ViewModels;

namespace School.Controllers
{
    public class TurmasController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly CultureInfo culture;

        public TurmasController(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            this.userManager = userManager;
            culture = new CultureInfo("pt-BR");

        }

        // GET: Matricula
        public async Task<IActionResult> Index()
        {
            var Usuario = userManager.GetUserId(User);

            List<ListTurmas> turmas = new List<ListTurmas>();

            if (User.IsInRole("Aluno"))
            {
                 turmas = (from x in (from d in _context.Disciplinas
                                              where !_context.Historicos.Any(dh => (dh.Disciplina.DisciplinaId == d.DisciplinaId) && (dh.Aluno.Id == Usuario)) &&
                                                    !_context.Matriculas.Any(mt => (mt.AlunoId == Usuario) && mt.Turma.Disciplina.DisciplinaId == d.DisciplinaId)
                                              select d)
                                   where x.Dependencia == null ||
                                   _context.Historicos.Any(dt => (dt.Disciplina.DisciplinaId == x.Dependencia) && (dt.Aluno.Id == Usuario))
                                   join t in _context.Turmas on x.DisciplinaId equals t.Disciplina.DisciplinaId
                                   let DisciplinaNome = x.Nome
                                   let Professor = t.Professor.UserName
                                   orderby x.Nome
                                   select new ListTurmas( t.TurmaId, t.Nome, Professor, DisciplinaNome, t.Vagas)).ToList();
            } 
                        else
            {
                 turmas = (from x in (from d in _context.Disciplinas
                                              join t in _context.Turmas on d.DisciplinaId equals t.Disciplina.DisciplinaId
                                              let DisciplinaNome = d.Nome
                                              let Professor = t.Professor.UserName
                                              orderby d.Nome
                                      select new ListTurmas( t.TurmaId, t.Nome, Professor, DisciplinaNome, t.Vagas ))
                            select x).ToList();
            }


            List<TurmaViewModel> TurmaVM = new List<TurmaViewModel>();

            foreach (var i in turmas)
                {
                    TurmaViewModel VM = new TurmaViewModel();
                VM.TurmaId = i.TurmaId;
                VM.NomeTurma = i.Nome;
                VM.NomeProfessor = i.Professor;
                VM.Disciplina = i.DisciplinaNome;
                VM.NroVagas = i.Vagas;
                VM.NroVagasDisponiveis = i.Vagas - _context.Matriculas.Count(m => m.TurmaId == i.TurmaId);

                    var horarios = (from t in _context.HorariosTurmas
                                   where t.TurmaId == i.TurmaId
                                   join h in _context.Horarios on t.HorarioId equals h.Id
                                   orderby h.IntDiaHora
                                   select h.DiaHora).ToList();


                    VM.HorariosTurma = String.Join(", ", horarios);
                    TurmaVM.Add(VM);
                }
            

            return View(TurmaVM);
           // return View(await _context.Matriculas.ToListAsync());
        }
/*
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
*/
/*
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
*/
/*
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
*/
/*
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
*/
/*
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
*/

        // GET: Matricula
        public async Task<IActionResult> Turmas(string id)
        {
            var Usuario = await userManager.GetUserAsync(this.User);

            //if (User.IsInRole("Aluno"))
            //{
                var qryTurmas = (from d in _context.Disciplinas where d.DisciplinaId == id
                                join t in _context.Turmas on d.DisciplinaId equals t.Disciplina.DisciplinaId
                                select new { t.Nome, t.Vagas, t.TurmaId, t.Professor.UserName}).ToList();


            ViewData["Disciplina"] = _context.Disciplinas.FindAsync(id).Result.Nome;

                List<TurmaViewModel> TurmaVM = new List<TurmaViewModel>();
                foreach (var q in qryTurmas)
                {
                    TurmaViewModel VM = new TurmaViewModel();
                    VM.TurmaId = q.TurmaId;
                    VM.NomeTurma = q.Nome;
                    VM.NomeProfessor = q.UserName;
                    VM.NroVagas = q.Vagas;
                    VM.NroVagasDisponiveis = 0;

                var horariosTurma = from x in (from ht in _context.HorariosTurmas
                                               where ht.TurmaId == q.TurmaId
                                               join h in _context.Horarios on ht.HorarioId equals h.Id
                                               orderby h.IntDiaHora
                                               select new { h.DiaHora })
                                    select x.DiaHora;

               VM.HorariosTurma = String.Join(", ", horariosTurma);

                    TurmaVM.Add(VM);
                }

            return View(TurmaVM);
            // return View(await _context.Matriculas.ToListAsync());
            //}
        }

        /*
                // GET: Matricula/EfetuarMatricula/id
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

        // GET: Turma/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var turma = await _context.Turmas.FindAsync(id);
            if (turma == null)
            {
                return NotFound();
            }
            return View(turma);
        }

        // POST: Turma/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TurmaId,Nome,Vagas")] Turma turma)
        {
            if (id != turma.TurmaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(turma);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TurmaExists(turma.TurmaId))
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
            return View(turma);
        }

        // POST: Turma/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var turma = await _context.Turmas.FindAsync(id);
            _context.Turmas.Remove(turma);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
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
            //matricula.MatriculaId =
            _context.Matriculas.Add(matricula);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TurmaExists(int id)
        {
            return _context.Turmas.Any(e => e.TurmaId == id);
        }
    }
}
