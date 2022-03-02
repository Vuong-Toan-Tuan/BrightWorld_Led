using BrightWorld_LED.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BrightWorld_LED.Controllers.User
{
    //[Authorize(Roles = RoleString.USER)]
    public class HomeUserController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: HomeUser
        public async Task<ActionResult> Index()
        {
            ViewBag.Brand = await db.Brands.ToListAsync();
            ViewBag.Category = await db.Categories.ToListAsync();
            return View(await db.Products.ToListAsync());
        }

        public ActionResult ShoppingGuide()
        {
            ViewBag.Category = db.Categories.ToList();
            return View();
        }

        public ActionResult Introduce()
        {
            ViewBag.Category = db.Categories.ToList();
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductModel product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.Category = await db.Categories.ToListAsync();
            return View(product);
        }

        [HttpPost]
        public async Task<ActionResult> Details([Bind(Include = "CartId,ProductId,UserId,Quantity,Price")] CartModel cartModel)
        {
            if (ModelState.IsValid)
            {
                cartModel.CartId = Guid.NewGuid();
                db.Carts.Add(cartModel);
                await db.SaveChangesAsync();
                //return RedirectToAction("Index");
                ViewBag.LbMess = "Thêm vào giỏ hàng thành công";
            }
            object id = cartModel.ProductId;
            ProductModel product = await db.Products.FindAsync(cartModel.ProductId);
            ViewBag.Category = await db.Categories.ToListAsync();
            return View(product);
        }

        [Authorize(Roles = RoleString.USER)]
        [HttpGet]
        public ActionResult Cart(string uid)
        {
            var list = (from d in db.Carts
                        where d.UserId == uid
                        select d).ToList();
            ViewBag.Category = db.Categories.ToList();
            return View(list);
        }

        [HttpPost]
        public async Task<ActionResult> Cart([Bind(Include = "OrderId,UserId,FullName,Phone,ShippingAddress,Notes,TotalPayment,Transaction,OrderStatus,CreateDate")] OrderModel orderModel)
        {
            //if (ModelState.IsValid)
            //{
                orderModel.OrderId = Guid.NewGuid();
                db.Orders.Add(orderModel);
                var oid = orderModel.OrderId;
                var list = (from d in db.Carts
                            where d.UserId == orderModel.UserId
                            select d).ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    OrderDetailModel orderdetail = new OrderDetailModel();
                    orderdetail.OrderDetailId = Guid.NewGuid();
                    orderdetail.OrderId = oid;
                    orderdetail.ProductId = list[i].ProductId;
                    orderdetail.Quantity = list[i].Quantity;
                    orderdetail.Price = list[i].Price;

                    db.OrderDetails.Add(orderdetail);
                }
                await db.SaveChangesAsync();
                //return RedirectToAction("Index");
            //}

            var list2 = (from d in db.Carts
                        where d.UserId == orderModel.UserId
                        select d).ToList();
            ViewBag.Category = db.Categories.ToList();
            ViewBag.LbMess = "Đặt hàng thành công";
            return View(list2);
        }
        public ActionResult DeleteProductFromCart(Guid cartId)
        {
            CartModel cartModel = db.Carts.Find(cartId);
            db.Carts.Remove(cartModel);
            db.SaveChangesAsync();
            //ViewBag.Category = db.Categories.ToList();
            return RedirectToAction("Cart", new { uid = cartModel.UserId });
        }

        public async Task<ActionResult> MyOrder(string uid)
        {
            ViewBag.Category = db.Categories.ToList();
            var listmyorder = await db.Orders.Where(x => x.UserId == uid).ToListAsync();
            var list = listmyorder.OrderByDescending(x => x.CreateDate);
            return View(list);
        }
        public async Task<ActionResult> MyOrderDetail(Guid orderId)
        {
            OrderModel orderModel = await db.Orders.FindAsync(orderId);
            var list = await db.OrderDetails.Where(x => x.OrderId == orderId).ToListAsync();
            ViewBag.ListProductOrder = list;
            ViewBag.Category = db.Categories.ToList();
            return View(orderModel);
        }

        public string getNamePro(Guid pid)
        {
            ProductModel a = db.Products.Find(pid);
            return a.ProductName;
        }

        public string getImagePro(Guid pid)
        {
            ProductModel a = db.Products.Find(pid);
            return a.Image;
        }
        public string getcurrenUserId()
        {
            //ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == User.Identity.GetUserId());
            string currentUserId = User.Identity.GetUserId();
            
            //var user = UserManager.FindById(User.Identity.GetUserId());
            
            if(currentUserId == null)
            {
                return null;
            }
            else
            {
                ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);
                return currentUser.Id;
            }
        }
        
        public async Task<ActionResult> Category(Guid? categoryId)
        {
            
            var listProductOfCategory = await db.Products.Where(x => x.CategoryId == categoryId).ToListAsync();
            ViewBag.Category = await db.Categories.ToListAsync();
            ViewBag.CategoryName = await db.Categories.FindAsync(categoryId);
            return View(listProductOfCategory);
        }


        public async Task<ActionResult> Search(string searching)
        {
            ViewBag.textSearch = searching;
            ViewBag.Category = await db.Categories.ToListAsync();
            return View(await db.Products.Where(x => x.ProductName.Contains(searching) || searching == null).ToListAsync());
        }

    }
}