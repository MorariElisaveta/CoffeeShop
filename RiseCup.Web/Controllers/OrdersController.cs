using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using RiseCup.Database;
using RiseCup.Domain.Models;
using System.Data.Entity;
using RiseCup.Web.Attributes;

namespace RiseCup.Web.Controllers
{
    [SimpleAuthorize("Moderator", "Administrator")]
    public class OrdersController : Controller
    {
        private RiseCupContext db = new RiseCupContext();

        // GET: Orders
        public ActionResult Index()
        {
            var orders = db.Orders.Include(o => o.User).ToList();
            return View(orders);
        }

        // GET: Orders/Create
        public ActionResult Create()
        {
            ViewBag.UserId = new SelectList(db.Users, "UserId", "Username");
            return View();
        }

        // POST: Orders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrderId,OrderDate,UserId,TotalAmount,Status")] Order order)
        {
            if (ModelState.IsValid)
            {
                order.OrderId = Guid.NewGuid().ToString();
                db.Orders.Add(order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserId = new SelectList(db.Users, "UserId", "Username", order.UserId);
            return View(order);
        }

        // GET: Orders/Edit/5
        // GET: Orders/Edit/5
        [SimpleAuthorize("Administrator")]
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.Users, "UserId", "Username", order.UserId);
            return View(order);
        }

// POST: Orders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SimpleAuthorize("Administrator")]
        public ActionResult Edit([Bind(Include = "OrderId,OrderDate,UserId,TotalAmount,Status")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.Users, "UserId", "Username", order.UserId);
            return View(order);
        }


        // GET: Orders/Delete/5
        [SimpleAuthorize("Administrator")]
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        
        public ActionResult DeleteConfirmed(string id)
        {
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
            db.SaveChanges();
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
