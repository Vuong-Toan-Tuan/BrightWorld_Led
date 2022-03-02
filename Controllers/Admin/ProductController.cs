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
    public class ProductController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Product
        public async Task<ActionResult> Index()
        {
            var products = db.Products.Include(p => p.Brand).Include(p => p.Category);
            return View(await products.ToListAsync());
        }

        // GET: Product/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductModel productModel = await db.Products.FindAsync(id);
            if (productModel == null)
            {
                return HttpNotFound();
            }
            return View(productModel);
        }

        // GET: Product/Create
        public ActionResult Create()
        {
            ViewBag.BrandId = new SelectList(db.Brands, "BrandId", "BrandName");
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName");
            return View();
        }

        // POST: Product/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ProductId,CategoryId,BrandId,ProductName,Image,SecondaryImage,Price,OldPrice,Quantity,Power,LightColor,Size,Material,Description,CreatedOn")] ProductModel productModel)
        {
            if (ModelState.IsValid)
            {
                //var result = Request.Files["Image"];
                //if(result != null)
                //{
                //    string path = Server.MapPath("~/Uploads/");
                //    if (!Directory.Exists(path))
                //    {
                //        Directory.CreateDirectory(path);
                //    }
                //    result.SaveAs(path + Path.GetFileName(result.FileName));
                //    productModel.Image = $"{Request.Url.Scheme}://{Request.Url.Authority}/Uploads/{result.FileName}";
                //}
                string path = Server.MapPath("~/Uploads/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                var result = Request.Files["ImageFile"];
                if (result.ContentLength > 0)
                {

                    result.SaveAs(path + Path.GetFileName(result.FileName));
                    productModel.Image = $"{Request.Url.Scheme}://{Request.Url.Authority}/Uploads/{result.FileName}";
                }

                var result1 = Request.Files.GetMultiple("SecondaryImagesFiles").Where(x => x.ContentLength > 0).ToList();
                if (result1.Count > 0)
                {
                    var imageList = new List<string>();
                    foreach (var file in result1)
                    {
                        file.SaveAs(path + Path.GetFileName(file.FileName));
                        imageList.Add($"{Request.Url.Scheme}://{Request.Url.Authority}/Uploads/{file.FileName}");
                    }
                    productModel.SecondaryImage = string.Join(",", imageList);
                }


                productModel.ProductId = Guid.NewGuid();
                db.Products.Add(productModel);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.BrandId = new SelectList(db.Brands, "BrandId", "BrandName", productModel.BrandId);
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", productModel.CategoryId);
            return View(productModel);
        }

        // GET: Product/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductModel productModel = await db.Products.FindAsync(id);
            if (productModel == null)
            {
                return HttpNotFound();
            }
            ViewBag.BrandId = new SelectList(db.Brands, "BrandId", "BrandName", productModel.BrandId);
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", productModel.CategoryId);
            return View(productModel);
        }

        // POST: Product/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ProductId,CategoryId,BrandId,ProductName,Image,SecondaryImage,Price,OldPrice,Quantity,Power,LightColor,Size,Material,Description,CreatedOn")] ProductModel productModel)
        {
            if (ModelState.IsValid)
            {
                string path = Server.MapPath("~/Uploads/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                var result = Request.Files["ImageFile"];
                if (result.ContentLength > 0)
                {
                    
                    result.SaveAs(path + Path.GetFileName(result.FileName));
                    productModel.Image = $"{Request.Url.Scheme}://{Request.Url.Authority}/Uploads/{result.FileName}";
                }

                var result1 = Request.Files.GetMultiple("SecondaryImagesFiles").Where(x => x.ContentLength > 0).ToList();
                if (result1.Count > 0)
                {
                    var imageList = new List<string>();
                    foreach (var file in result1)
                    {
                        file.SaveAs(path + Path.GetFileName(file.FileName));
                        imageList.Add($"{Request.Url.Scheme}://{Request.Url.Authority}/Uploads/{file.FileName}");
                    }
                    productModel.SecondaryImage = string.Join(",", imageList);
                }

                db.Entry(productModel).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.BrandId = new SelectList(db.Brands, "BrandId", "BrandName", productModel.BrandId);
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", productModel.CategoryId);
            return View(productModel);
        }

        // GET: Product/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductModel productModel = await db.Products.FindAsync(id);
            if (productModel == null)
            {
                return HttpNotFound();
            }
            return View(productModel);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            ProductModel productModel = await db.Products.FindAsync(id);
            db.Products.Remove(productModel);
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
