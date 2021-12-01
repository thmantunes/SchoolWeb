using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using School.Models;
using School.Context;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace School.DBSeeder
{
    public class DBSeeder
    {

        // Carrega dados nas tabelas (seed)
        private class Pessoa
        {
            public Pessoa(string Nome, string Role)
            {
                this.Nome = Nome;
                this.Role = Role;
            }
            public string Nome;
            public string Role;
        };

        public static async Task GenerateSeedsAsync(IServiceProvider serviceProvider)
        {
            AppDbContext _context = serviceProvider.GetRequiredService<AppDbContext>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            Random rnd = new Random();

            string[] rolesNames = { "Coordenador", "Aluno", "Professor" };
            IdentityResult result;

            foreach (var namesRole in rolesNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(namesRole);
                if (!roleExist)
                {
                    result = await roleManager.CreateAsync(new IdentityRole(namesRole));
                }
            }

            if (await userManager.FindByNameAsync("Coordenador") == null)
            {
                var user = new ApplicationUser() { UserName = "Coordenador", Email = "coordenador@gmail.com" };
                result = await userManager.CreateAsync(user, "Coordenador#1");
                result = await userManager.AddToRoleAsync(user, "Coordenador");
            }

            // Carrega Pessoas
            List<Pessoa> pessoas = new List<Pessoa>()
            { new ("Aldo Silva", "Aluno"),
              new ("Alberto Ribeiro", "Aluno"),
              new ("Carlos Rodrigues","Aluno"),
              new ( "Rosa Santos","Aluno"),
              new ("Thomas Antunes","Aluno"),
              new ("Runa Felina","Aluno")
            };

            List<Pessoa> professores = new List<Pessoa>()
            { new ("Beto Pascoal","Professor"),
              new ("Maria Antonieta","Professor"),
              new ("João Pedro","Professor")
            };


            pessoas.AddRange(professores);
            foreach (Pessoa pessoa in pessoas)
            {
                if (userManager.FindByNameAsync(pessoa.Nome).Result == null)
                {
                    var user = new ApplicationUser() { UserName = pessoa.Nome, Email = pessoa.Nome.ToLower() + "@gmail.com" };
                    var password = pessoa.Nome.Replace(" ", "") + "#1";
                    await userManager.CreateAsync(user, password);
                    await userManager.AddToRoleAsync(user, pessoa.Role);
                }
            }

            // Carrega Disciplinas
            if (_context.Disciplinas.Count() == 0)
            {

                List<Disciplina> Disciplinas = new List<Disciplina>();

                Disciplinas.Add(new Disciplina("1152I", 04, "Humanismo e Culura Religiosa", null));
                Disciplinas.Add(new Disciplina("1501A", 04, "Ética e Cidadania", null));
                Disciplinas.Add(new Disciplina("24135", 02, "Legislação em Informatica", null));
                Disciplinas.Add(new Disciplina("2540L", 04, "Empreendimentos Empresariais", "254PF"));
                Disciplinas.Add(new Disciplina("254AU", 04, "Governança Estratégica de TI", "4636Z"));
                Disciplinas.Add(new Disciplina("254PF", 04, "Fundamentos Aplicados da Administração", null));
                Disciplinas.Add(new Disciplina("254PG", 02, "Comportamento Organizacional", "2540L"));
                Disciplinas.Add(new Disciplina("4115L", 02, "Matemática Discreta", null));
                Disciplinas.Add(new Disciplina("4611C", 06, "Fundamentos de programação", null));
                Disciplinas.Add(new Disciplina("4611D", 02, "Laboratório de Banco de Dados", null));
                Disciplinas.Add(new Disciplina("4611F", 04, "Programação Orientada a Objetos", "4611C"));
                Disciplinas.Add(new Disciplina("4611H", 02, "Lógica para Computação", "4611D"));
                Disciplinas.Add(new Disciplina("4636B", 04, "Arquitetura Organizacional Aplicada SI", "4636Z"));
                Disciplinas.Add(new Disciplina("4636C", 04, "Auditoria e Segurança de SI", null));
                Disciplinas.Add(new Disciplina("4636D", 02, "Avaliação de Desempenho de Sistemas", "95304"));
                Disciplinas.Add(new Disciplina("4636F", 02, "Engenharia de Software", null));
                Disciplinas.Add(new Disciplina("4636H", 04, "Fundamentos de Desenvolvimento de Software", "4611F"));
                Disciplinas.Add(new Disciplina("4636K", 02, "Tópicos Avançados em Gestão de dados", "4636R"));
                Disciplinas.Add(new Disciplina("4636L", 04, "Gerência de Projetos de TI", null));
                Disciplinas.Add(new Disciplina("4636M", 04, "Gerência de Redes de Computadores", "4636k"));
                Disciplinas.Add(new Disciplina("4636N", 04, "Infraestrutura de TI", "4636M"));
                Disciplinas.Add(new Disciplina("4636R", 04, "Inteligência de Negócio", "4611D"));
                Disciplinas.Add(new Disciplina("4636T", 04, "Interação Humano-Computador", "4636F"));
                Disciplinas.Add(new Disciplina("4636X", 04, "Modelagem de Negócio", null));
                Disciplinas.Add(new Disciplina("4636Z", 04, "Planejamento e Gestão Estratégica de TI", null));
                Disciplinas.Add(new Disciplina("4636G", 04, "Fundamentos de Redes de Computadores", "4637N"));
                Disciplinas.Add(new Disciplina("4636U", 04, "Introdução a Sistemas de Informação", null));
                Disciplinas.Add(new Disciplina("4637A", 02, "Prática Profissional", null));
                Disciplinas.Add(new Disciplina("4637B", 04, "Programação de Software Aplicado", "4636H"));
                Disciplinas.Add(new Disciplina("4637C", 04, "Projeto e Desenvolvimento de Software", "4636F"));
                Disciplinas.Add(new Disciplina("4637D", 04, "Qualidade de Processo", "4637C"));
                Disciplinas.Add(new Disciplina("4637E", 02, "Qualidade de Produto", "4611C"));
                Disciplinas.Add(new Disciplina("4637F", 04, "Sistemas de Informação Integrados", null));
                Disciplinas.Add(new Disciplina("4637G", 04, "Lógica para Computação", "4637N"));
                Disciplinas.Add(new Disciplina("4637J", 02, "Teoria da Computação", "4611E"));
                Disciplinas.Add(new Disciplina("4637L", 04, "Trabalho de conclusão 1", null));
                Disciplinas.Add(new Disciplina("4637M", 04, "Trabalho de conclusão 2", "4637L"));
                Disciplinas.Add(new Disciplina("4637N", 02, "Arquitetura de Computadores", "4637N"));
                Disciplinas.Add(new Disciplina("4637P", 02, "Fundamentos de Computação", null));
                Disciplinas.Add(new Disciplina("4647D", 04, "Sistemas Operacionais", "4637N"));
                Disciplinas.Add(new Disciplina("46506", 04, "Engenharia de Requisito", "4636F"));
                Disciplinas.Add(new Disciplina("46520", 04, "Modelagem e Projeto de BD", "4611D"));
                Disciplinas.Add(new Disciplina("95300", 04, "Calculo A", null));
                Disciplinas.Add(new Disciplina("95304", 04, "Probabilidade e Estatítisca", null));
                Disciplinas.Add(new Disciplina("9611E", 04, "Lógica para Computação", null));

                _context.Disciplinas.AddRange(Disciplinas);

                _context.SaveChanges();

            }


            // SEED - Horarios 02.08:00 - 06.21:00
            //
            if (_context.Horarios.Count() == 0)
            {
                List<string> Dia = new List<string>() { "","Dom","Seg","Ter", "Qua", "Qui", "Sex", "Sab"};

                List<Horario> horarios = new List<Horario>();
                for (int d = 2; d <= 6; d++)
                {
                    for (int h = 8; h <= 21; h++) 
                    {
                        horarios.Add(new Horario(d * 10000 + h * 100, Dia[d]+" "+ h.ToString() + ":00" ));
                    }
                }
                _context.Horarios.AddRange(horarios);
                _context.SaveChanges();
            }


            // SEED - Turmas
            //
            if (_context.Turmas.Count() == 0)
            {

                var disciplinas = from d in _context.Disciplinas
                                  select d.DisciplinaId;

                var prof = (from R in _context.Roles
                            where R.Name.Equals("Professor")
                            join UR in _context.UserRoles on R.Id equals UR.RoleId
                            join U in _context.Users on UR.UserId equals U.Id
                            select U.Id).ToList();



                List<Turma> turmas = new List<Turma>();

                // lista de disciplinas
                foreach (var tid in disciplinas)
                {
                    Disciplina disciplina = _context.Disciplinas.FindAsync(tid).Result;

                    // busca um professor aleatoriamente
                    int i = rnd.Next(0, prof.Count());
                    //var pId = prof[i]; //.ElementAt(i).FirstOrDefault();
                    ApplicationUser professor = _context.Users.FindAsync(prof[i]).Result;

                    Turma turma = new Turma("T-" + tid, 20, disciplina, professor);
                    turmas.Add(turma);
                }
                _context.Turmas.AddRange(turmas);

                _context.SaveChanges();


                // Horario Turmas
                var horarios = (from hs in _context.Horarios select hs.Id).ToList();

                List<HorarioTurma> horariosTurmas = new List<HorarioTurma>();
                foreach (Turma turma in turmas)
                {
                    int na = turma.Disciplina.Creditos / 2; // numero de aulas
                    for (int d = 1; d <= na; d++)
                    {
                        HorarioTurma HoraTurma = new HorarioTurma();
                        do
                        {
                            int horarioId = rnd.Next(0, horarios.Count()); // pega um dia/hora
                            HoraTurma.HorarioId = horarios[horarioId];
                            HoraTurma.TurmaId = turma.TurmaId;
                        } while (horariosTurmas.Contains(HoraTurma));
                        horariosTurmas.Add(HoraTurma);
                    }
                }
                _context.HorariosTurmas.AddRange(horariosTurmas);
                _context.SaveChanges();


            }

            // SEED - Historicos
            //
            if (_context.Historicos.Count() == 0)
            {
                var Alunos = from r in _context.Roles
                             where r.Name == "Aluno"
                             join ur in _context.UserRoles on r.Id equals ur.RoleId
                             join u in _context.Users on ur.UserId equals u.Id
                             let aluno = u
                             select new { aluno };

                var disciplinas = (from d in _context.Disciplinas
                                   where d.Dependencia == null
                                   select d).ToList();

                List<Historico> historico = new List<Historico>();
                List<string> conceito = new List<string>() { "A", "B", "C" };
                foreach (var al in Alunos)
                {
                    int d = rnd.Next(0, 8);  // nro disciplinas cursadas
                    for (int i = 1; i <= d; i++)
                    {
                        Historico hist = new Historico();
                        hist.Aluno = al.aluno;
                        hist.Disciplina = disciplinas[rnd.Next(0, disciplinas.Count())];    // busca disciplina
                        hist.Conceito = conceito[rnd.Next(0, 3)];                           // gera conceito
                        hist.DataConclusao = DateTime.Today.AddDays(-180 * rnd.Next(1, 7)); // data conclusão

                        var x = historico.Find(y => y.Aluno.Id == al.aluno.Id && y.Disciplina.DisciplinaId == y.Disciplina.DisciplinaId);
                        if (x==null)
                        {
                            historico.Add(hist);
                        }
                    }
                }
                _context.Historicos.AddRange(historico);
                _context.SaveChanges();
            }
        }
    }
}
