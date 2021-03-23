using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ApiHallOfFame;
using ApiHallOfFame.Models;
using Microsoft.Extensions.Logging;

namespace BackendHallOfFame.Controllers
{
    public class PeopleController : Controller
    {
        private readonly IPerson context;
        private readonly ILogger<PeopleController> logger;

        public PeopleController(IPerson context, ILogger<PeopleController> logger)
        {
            this.context = context;
            this.logger = logger;
        }


        // GET: People
        public IActionResult Index()
        {
            return View(context.GetPersons());
        }

        // GET: People/Details/5
        public IActionResult Details(long? id)
        {
            if (!id.HasValue)
            {
                logger.LogError("Id was null.Not found this person");
                return NotFound();
            }

            var person = context.GetPerson(id.Value);
            if (person == null)
            {
                logger.LogError("Not found this person");
                return NotFound();
            }

            return View(person);
        }

        // GET: People/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: People/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Name,DisplayName")] Person person)
        {
            if (ModelState.IsValid)
            {
                context.InsertPerson(person);
                context.Save();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                logger.LogError("Failed to create");
            }
            return View(person);
        }

        // GET: People/Edit/5
        public IActionResult Edit(long? id)
        {
            if (!id.HasValue)
            {
                logger.LogError("Id was null.Not found this person");
                return NotFound();
            }

            var person = context.GetPerson(id.Value);
            if (person == null)
            {
                logger.LogError("Not found this person");
                return NotFound();
            }
            return View(person);
        }

        // POST: People/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(long id, [Bind("Id,Name,DisplayName")] Person person)
        {
            if (id != person.Id)
            {
                logger.LogError("Not found this person");
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    context.UpdatePerson(person);
                    context.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonExists(person.Id))
                    {
                        logger.LogError("Not found this person");
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(person);
        }

        // GET: People/Delete/5
        public IActionResult Delete(long? id)
        {
            if (!id.HasValue)
            {
                logger.LogError("Id was null.Not found this person");
                return NotFound();
            }

            var person = context.GetPerson(id.Value);
            if (person == null)
            {
                logger.LogError("Not found this person");
                return NotFound();
            }

            return View(person);
        }

        // POST: People/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(long id)
        {
            var person = context.GetPerson(id);
            context.DeletePerson(person);
            context.Save();
            return RedirectToAction(nameof(Index));
        }

        private bool PersonExists(long id)
        {
            return context.GetPersons().Any(e => e.Id == id);
        }
    }
}
