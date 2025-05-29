using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MyMvcApp.Models;

namespace MyMvcApp.Controllers;

public class UserController : Controller
{
    public static System.Collections.Generic.List<User> userlist = new System.Collections.Generic.List<User>();

        // GET: User
        public ActionResult Index()
        {
            return View(userlist);
        }

        // GET: User/Details/5
        public ActionResult Details(int id)
        {
            var user = userlist.FirstOrDefault(u => u.Id == id);
            if (user == null)
                return NotFound();
            return View(user);
        }

        // GET: User/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        [HttpPost]
        public ActionResult Create(User user)
        {
            if (!ModelState.IsValid)
                return View(user);
            userlist.Add(user);
            return RedirectToAction(nameof(Index));
        }

        // GET: User/Edit/5
        public ActionResult Edit(int id)
        {
            var user = userlist.FirstOrDefault(u => u.Id == id);
            if (user == null)
                return NotFound();
            return View(user);
        }

        // POST: User/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, User user)
        {
            if (!ModelState.IsValid)
                return View(user);
            var existing = userlist.FirstOrDefault(u => u.Id == id);
            if (existing == null)
                return NotFound();
            existing.Name = user.Name;
            // Update other fields as needed
            return RedirectToAction(nameof(Index));
        }

        // GET: User/Delete/5
        public ActionResult Delete(int id)
        {
            var user = userlist.FirstOrDefault(u => u.Id == id);
            if (user == null)
                return NotFound();
            return View(user);
        }

        // POST: User/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            var user = userlist.FirstOrDefault(u => u.Id == id);
            if (user == null)
                return NotFound();
            userlist.Remove(user);
            return RedirectToAction(nameof(Index));
        }

        // GET: User/Search
        public ActionResult Search(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return View(new List<User>());
            var results = userlist.FindAll(u =>
                (!string.IsNullOrEmpty(u.Name) && u.Name.Contains(query, System.StringComparison.OrdinalIgnoreCase)) ||
                (!string.IsNullOrEmpty(u.Email) && u.Email.Contains(query, System.StringComparison.OrdinalIgnoreCase))
            );
            return View(results);
        }
}
