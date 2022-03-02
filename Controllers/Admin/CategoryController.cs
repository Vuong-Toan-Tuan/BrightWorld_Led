using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BrightWorld_LED.Models;

namespace BrightWorld_LED.Controllers.Admin
{
    [Authorize(Roles = RoleString.ADMIN)]
    public class CategoryController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Category
        public async Task<ActionResult> Index()
        {
            return View(await db.Categories.ToListAsync());
        }

        // GET: Category/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CategoryModel categoryModel = await db.Categories.FindAsync(id);
            if (categoryModel == null)
            {
                return HttpNotFound();
            }
            return View(categoryModel);
        }

        // GET: Category/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Category/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "CategoryId,CategoryName,CreatedOn")] CategoryModel categoryModel)
        {
            if (ModelState.IsValid)
            {
                categoryModel.CategoryId = Guid.NewGuid();
                db.Categories.Add(categoryModel);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(categoryModel);
        }

        // GET: Category/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CategoryModel categoryModel = await db.Categories.FindAsync(id);
            if (categoryModel == null)
            {
                return HttpNotFound();
            }
            return View(categoryModel);
        }

        // POST: Category/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "CategoryId,CategoryName,CreatedOn")] CategoryModel categoryModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(categoryModel).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(categoryModel);
        }

        // GET: Category/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CategoryModel categoryModel = await db.Categories.FindAsync(id);
            if (categoryModel == null)
            {
                return HttpNotFound();
            }
            return View(categoryModel);
        }

        // POST: Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            CategoryModel categoryModel = await db.Categories.FindAsync(id);
            db.Categories.Remove(categoryModel);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
