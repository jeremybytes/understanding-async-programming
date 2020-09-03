using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskAwait.Library;
using TaskAwait.Shared;

namespace Concurrent.UI.Web.Controllers
{
    public class PeopleController : Controller
    {
        PersonReader reader = new PersonReader();

        public Task<ViewResult> WithTask()
        {
            ViewData["Title"] = "Using Task";
            ViewData["RequestStart"] = DateTime.Now;

            Task<List<Person>> peopleTask = reader.GetPeopleAsync();

            Task<ViewResult> resultTask = peopleTask.ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    var errors = task.Exception!.Flatten().InnerExceptions;
                    return View("Error", errors);
                }

                List<Person> people = task.Result;
                ViewData["RequestEnd"] = DateTime.Now;
                return View("Index", people);
            });

            return resultTask;
        }

        public async Task<IActionResult> WithAwait()
        {
            ViewData["Title"] = "Using async/await";
            ViewData["RequestStart"] = DateTime.Now;
            try
            {
                var people = await reader.GetPeopleAsync().ConfigureAwait(false);
                return View("Index", people);
            }
            catch (Exception ex)
            {
                var errors = new List<Exception>() { ex };
                return View("Error", errors);
            }
            finally
            {
                ViewData["RequestEnd"] = DateTime.Now;
            }
        }
    }
}
