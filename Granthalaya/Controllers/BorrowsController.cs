using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Granthalaya.Models;

namespace Granthalaya.Controllers
{
    public class BorrowsController : Controller
    {
        private GranthalayaEntities db = new GranthalayaEntities();

        // GET: Borrows
        public ActionResult Index()
        {
            var borrows = db.Borrows.Include(b => b.Book).Include(b => b.User);
            return View(borrows.ToList());
        }

        // GET: Borrows/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Borrow borrow = db.Borrows.Find(id);
            if (borrow == null)
            {
                return HttpNotFound();
            }
            return View(borrow);
        }

        // GET: Borrows/Create
        public ActionResult Create(String id)
        {
            ViewBag.Isbn = new SelectList(db.Books, "Isbn", "Ttile");
            ViewBag.Uid = new SelectList(db.Users, "Uid", "Password");
            Borrow borrow = new Borrow();
            borrow.Uid = Session["UserId"].ToString();
            borrow.Isbn = id;
            borrow.Bdate = DateTime.Now;
            return Create(borrow);
        }

        // POST: Borrows/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Bid,Uid,Isbn,Bdate,Rdate")] Borrow borrow)
        {
            if (ModelState.IsValid)
            {
                db.Borrows.Add(borrow);
                Book book = db.Books.Where(b => b.Isbn == borrow.Isbn).FirstOrDefault();
                if(book.AvailableBooks!=0)
                    book.AvailableBooks--;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Isbn = new SelectList(db.Books, "Isbn", "Ttile", borrow.Isbn);
            ViewBag.Uid = new SelectList(db.Users, "Uid", "Password", borrow.Uid);
            return View(borrow);
        }

        // GET: Borrows/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Borrow borrow = db.Borrows.Find(id);
            if (borrow == null)
            {
                return HttpNotFound();
            }
            ViewBag.Isbn = new SelectList(db.Books, "Isbn", "Ttile", borrow.Isbn);
            ViewBag.Uid = new SelectList(db.Users, "Uid", "Password", borrow.Uid);
            return View(borrow);
        }

        // POST: Borrows/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Bid,Uid,Isbn,Bdate,Rdate")] Borrow borrow)
        {
            if (ModelState.IsValid)
            {
                db.Entry(borrow).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Isbn = new SelectList(db.Books, "Isbn", "Ttile", borrow.Isbn);
            ViewBag.Uid = new SelectList(db.Users, "Uid", "Password", borrow.Uid);
            return View(borrow);
        }

        // GET: Borrows/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Borrow borrow = db.Borrows.Find(id);
            if (borrow == null)
            {
                return HttpNotFound();
            }
            return View(borrow);
        }

        // POST: Borrows/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Borrow borrow = db.Borrows.Find(id);
            db.Borrows.Remove(borrow);
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
