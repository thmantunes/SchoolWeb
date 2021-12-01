using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using School.Context;
using School.Models;

namespace SchoolWeb.Controllers
{
    public class DisciplinasController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> userManager;


        public DisciplinasController(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            this.userManager = userManager;
            
        }

        // GET: Disciplinas
        public async Task<IActionResult> Index()
        {
            var Usuario =  userManager.GetUserId(User);

            List<Disciplina> disciplinas = new List<Disciplina>();

            if (User.Identity.IsAuthenticated && User.IsInRole("Aluno"))
                    {

                // disciplinas sem depencias ou com dependencia concluida

                disciplinas = (from x in (from d in _context.Disciplinas
                                          where !_context.Historicos.Any(dh => (dh.Disciplina.DisciplinaId == d.DisciplinaId) && (dh.Aluno.Id == Usuario)) &&
                                                !_context.Matriculas.Any(mt => (mt.AlunoId == Usuario) && mt.Turma.Disciplina.DisciplinaId == d.DisciplinaId)
                                          select d)
                               where x.Dependencia == null ||
                               _context.Historicos.Any(dt => (dt.Disciplina.DisciplinaId == x.Dependencia) && (dt.Aluno.Id == Usuario))
                               orderby x.Nome
                               select x).ToList();

                ViewData["Status"] = _context.Matriculas.Any(x => x.AlunoId==Usuario && x.Status == "Efetivada") ? "Efetivada" : "Aberta";
            }
            else
            {
                disciplinas = (from d in _context.Disciplinas 
                               orderby d.Nome 
                               select d).ToList();

            }
            return View(disciplinas);
        }

        // GET: Disciplinas/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var disciplina = await _context.Disciplinas
                .FirstOrDefaultAsync(m => m.DisciplinaId == id);
            if (disciplina == null)
            {
                return NotFound();
            }

            return View(disciplina);
        }

        // GET: Disciplinas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Disciplinas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DisciplinaId,Creditos,Nome,Dependencia")] Disciplina disciplina)
        {
            if (ModelState.IsValid)
            {
                _context.Add(disciplina);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(disciplina);
        }

        // GET: Disciplinas/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var disciplina = await _context.Disciplinas.FindAsync(id);
            if (disciplina == null)
            {
                return NotFound();
            }
            return View(disciplina);
        }

        // POST: Disciplinas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("DisciplinaId,Creditos,Nome,Dependencia")] Disciplina disciplina)
        {
            if (id != disciplina.DisciplinaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(disciplina);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DisciplinaExists(disciplina.DisciplinaId))
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
            return View(disciplina);
        }

        // GET: Disciplinas/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var disciplina = await _context.Disciplinas
                .FirstOrDefaultAsync(m => m.DisciplinaId == id);
            if (disciplina == null)
            {
                return NotFound();
            }

            return View(disciplina);
        }

        // POST: Disciplinas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var disciplina = await _context.Disciplinas.FindAsync(id);
            _context.Disciplinas.Remove(disciplina);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DisciplinaExists(string id)
        {
            return _context.Disciplinas.Any(e => e.DisciplinaId == id);
        }
    }
}
