using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using fortune_teller_mvc.Models;

namespace fortune_teller_mvc.Controllers
{
    public class CustomersController : Controller
    {
        private FortuneTellerMVCEntities db = new FortuneTellerMVCEntities();

        // GET: Customers
        public ActionResult Index()
        {
            var customers = db.Customers.Include(c => c.BirthMonth).Include(c => c.Color);
            return View(customers.ToList());
        }

        // GET: Customers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            
            if (customer.Age % 2 == 0)
            {
                ViewBag.RetireIn = 30;
            }
            else
            {
                ViewBag.RetireIn = 50;
            }

            if (customer.NumberOfSiblings == 0)
            {
                ViewBag.Location = "Melbourne, Australia";
            }
            else if (customer.NumberOfSiblings == 1)
            {
                ViewBag.Location = "Tokyo, Japan";
            }
            else if (customer.NumberOfSiblings == 2)
            {
                ViewBag.Location = "New York, New York";
            }
            else if (customer.NumberOfSiblings == 3)
            {
                ViewBag.Location = "Paris, France";
            }
            else
            {
                ViewBag.Location = "Los Angeles, California";
            }

            switch (customer.FavoriteColorID)
            {
                case 1:
                    ViewBag.Vehicle = "Ferrari LaFerrari";
                    break;
                case 2:
                    ViewBag.Vehicle = "Lamborghini Aventador";
                    break;
                case 3:
                    ViewBag.Vehicle = "Porsche 911 Turbo S";
                    break;
                case 4:
                    ViewBag.Vehicle = "Nissan GT-R";
                    break;
                case 5:
                    ViewBag.Vehicle = "Chevrolet Corvette ZR1";
                    break;
                case 6:
                    ViewBag.Vehicle = "Audi R8";
                    break;
                case 7:
                    ViewBag.Vehicle = "Mercedes Benz SLS AMG";
                    break;
            }

            int charNum = -1;

            for (int x = 0; x <= 2 && charNum < 0; x++)
            {
                //Inner loops check FirstName...
                for (int y = 0; y < customer.FirstName.Length; y++)
                {
                    if (customer.BirthMonth.MonthName.Substring(x, 1) == customer.FirstName.ToLower().Substring(y, 1))
                    {
                        charNum = x;
                    }
                }

                //...and LastName for matches with the BirthMonth characters.
                for (int y = 0; y < customer.LastName.Length; y++)
                {
                    if (customer.BirthMonth.MonthName.Substring(x, 1) == customer.LastName.ToLower().Substring(y, 1))
                    {
                        charNum = x;
                    }
                }
            }

            if (charNum == -1)
            {
                ViewBag.BankAccount = "$1,000,000";
            }
            else if (charNum == 0)
            {
                ViewBag.BankAccount = "$2,000,000";
            }
            else if (charNum == 1)
            {
                ViewBag.BankAccount = "$3,000,000";
            }
            else if (charNum == 2)
            {
                ViewBag.BankAccount = "$4,000,000";
            }

            return View(customer);
        }

        // GET: Customers/Create
        public ActionResult Create()
        {
            ViewBag.BirthMonthID = new SelectList(db.BirthMonths, "BirthMonthID", "MonthName");
            ViewBag.FavoriteColorID = new SelectList(db.Colors, "FavoriteColorID", "ColorName");
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CustomerID,FirstName,LastName,Age,BirthMonthID,FavoriteColorID,NumberOfSiblings")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Customers.Add(customer);
                db.SaveChanges();
                return RedirectToAction("Details", new { id = customer.CustomerID });
            }

            ViewBag.BirthMonthID = new SelectList(db.BirthMonths, "BirthMonthID", "MonthName", customer.BirthMonthID);
            ViewBag.FavoriteColorID = new SelectList(db.Colors, "FavoriteColorID", "ColorName", customer.FavoriteColorID);
            return View(customer);
        }

        // GET: Customers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            ViewBag.BirthMonthID = new SelectList(db.BirthMonths, "BirthMonthID", "MonthName", customer.BirthMonthID);
            ViewBag.FavoriteColorID = new SelectList(db.Colors, "FavoriteColorID", "ColorName", customer.FavoriteColorID);
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CustomerID,FirstName,LastName,Age,BirthMonthID,FavoriteColorID,NumberOfSiblings")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BirthMonthID = new SelectList(db.BirthMonths, "BirthMonthID", "MonthName", customer.BirthMonthID);
            ViewBag.FavoriteColorID = new SelectList(db.Colors, "FavoriteColorID", "ColorName", customer.FavoriteColorID);
            return View(customer);
        }

        // GET: Customers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Customer customer = db.Customers.Find(id);
            db.Customers.Remove(customer);
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
