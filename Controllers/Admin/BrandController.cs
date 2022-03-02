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
using System.IO;

namespace BrightWorld_LED.Controllers.Admin
{
    [Authorize(Roles = RoleString.ADMIN)]
    public class BrandController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Brand
        public async Task<ActionResult> Index()
        {
            return View(await db.Brands.ToListAsync());
        }

        // GET: Brand/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BrandModel brandModel = await db.Brands.FindAsync(id);
            if (brandModel == null)
            {
                return HttpNotFound();
            }
            return View(brandModel);
        }

        // GET: Brand/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Brand/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "BrandId,BrandName,BrandImage,Description")] BrandModel brandModel)
        {
            if (ModelState.IsValid)
            {
                string path = Server.MapPath("~/Uploads/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                var result = Request.Files["BrandImage"];
                if (result.ContentLength > 0)
                {

                    result.SaveAs(path + Path.GetFileName(result.FileName));
                    brandModel.BrandImage = $"{Request.Url.Scheme}://{Request.Url.Authority}/Uploads/{result.FileName}";
                }

                brandModel.BrandId = Guid.NewGuid();
                db.Brands.Add(brandModel);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(brandModel);
        }

        // GET: Brand/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BrandModel brandModel = await db.Brands.FindAsync(id);
            if (brandModel == null)
            {
                return HttpNotFound();
            }
            return View(brandModel);
        }

        // POST: Brand/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "BrandId,BrandName,BrandImage,Description")] BrandModel brandModel)
        {
            if (ModelState.IsValid)
            {
                string path = Server.MapPath("~/Uploads/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                var result = Request.Files["BrandImageFile"];
                if (result.ContentLength > 0)
                {

                    result.SaveAs(path + Path.GetFileName(result.FileName));
                    brandModel.BrandImage = $"{Request.Url.Scheme}://{Request.Url.Authority}/Uploads/{result.FileName}";
                }

                db.Entry(brandModel).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(brandModel);
        }

        // GET: Brand/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BrandModel brandModel = await db.Brands.FindAsync(id);
            if (brandModel == null)
            {
                return HttpNotFound();
            }
            return View(brandModel);
        }

        // POST: Brand/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            BrandModel brandModel = await db.Brands.FindAsync(id);
            db.Brands.Remove(brandModel);
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
