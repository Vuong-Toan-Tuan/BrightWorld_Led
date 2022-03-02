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
    public class OrderModelsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: OrderModels
        public async Task<ActionResult> Index()
        {
            var list = await db.Orders.ToListAsync();
            return View(list.OrderByDescending(x => x.CreateDate));
        }

        // GET: OrderModels/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderModel orderModel = await db.Orders.FindAsync(id);
            if (orderModel == null)
            {
                return HttpNotFound();
            }
            var list = await db.OrderDetails.Where(x => x.OrderId == id).ToListAsync();
            ViewBag.ListProductOrder = list;
            return View(orderModel);
        }



        public ActionResult Chart()
        {
            return View();
        }

        public ActionResult Data()
        {
            //OrderModel orderModel = new OrderModel();
            //var listOrder = new List<OrderModel>();
            var listOrder = db.Orders.ToList();
            return Json(listOrder, JsonRequestBehavior.AllowGet);
        }

        //[Route("api/order")]
        //[HttpGet("findall")]
        //[Products("application/json")]
        //public async Task<ActionResult> findAll()
        //{
        //    var products = new List<OrderModel>
        //    {
        //        new OrderModel { OrderId = Guid.NewGuid(), TotalPayment = 100, CreateDate = DateTime.Parse("05/5/2021") },
        //        new OrderModel { OrderId = Guid.NewGuid(), TotalPayment = 200, CreateDate = DateTime.Parse("06/6/2021") }
        //        //new OrderModel { OrderId = "p03", Name = "Product 3", Price = 80, Quantity = 60 },
        //        //new OrderModel { OrderId = "p04", Name = "Product 4", Price = 290, Quantity = 34 },
        //        //new OrderModel { OrderId = "p05", Name = "Product 5", Price = 200, Quantity = 29 }
        //    };
        //    return products;
        //}
        
        //public List<OrderModel> findAll2()
        //{
        //    return (db.Orders.ToList());
        //}
          

        // GET: OrderModels/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: OrderModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "OrderId,UserId,FullName,Phone,ShippingAddress,Notes,TotalPayment,Transaction,OrderStatus,CreateDate")] OrderModel orderModel)
        {
            if (ModelState.IsValid)
            {
                orderModel.OrderId = Guid.NewGuid();
                db.Orders.Add(orderModel);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(orderModel);
        }

        // GET: OrderModels/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderModel orderModel = await db.Orders.FindAsync(id);
            if (orderModel == null)
            {
                return HttpNotFound();
            }
            return View(orderModel);
        }

        // POST: OrderModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "OrderId,UserId,FullName,Phone,ShippingAddress,Notes,TotalPayment,Transaction,OrderStatus,CreateDate")] OrderModel orderModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orderModel).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(orderModel);
        }

        // GET: OrderModels/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderModel orderModel = await db.Orders.FindAsync(id);
            if (orderModel == null)
            {
                return HttpNotFound();
            }
            return View(orderModel);
        }

        // POST: OrderModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            OrderModel orderModel = await db.Orders.FindAsync(id);
            db.Orders.Remove(orderModel);
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
