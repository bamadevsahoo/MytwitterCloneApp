using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TwitterClone;
using TwitterClone.Filters;
using TwitterClone.Models;

namespace TwitterClone.Controllers
{
    [CustomExceptionHelperFilter]
    public class ProfileController : Controller
    {
        private TwitterCloneDBEntities1 db = new TwitterCloneDBEntities1();
     
        // GET: People/Details/5
        [Authorize]
        [OutputCache(Duration =0,NoStore =true)]
        public ActionResult Details()
        {
            if (User.Identity.Name == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.People.Find(User.Identity.Name);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }


        // GET: People/Edit/5
        [Authorize]
        [OutputCache(Duration = 0, NoStore = true)]
        public ActionResult Edit()
        {
            if (User.Identity.Name == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.People.Find(User.Identity.Name);
            if (person == null)
            {
                return HttpNotFound();
            }
            EditProfile editprof = new EditProfile();
            editprof.fullName = person.fullName;
            editprof.emailID = person.email;
            editprof.active = person.active;
            return View(editprof);
        }

        // POST: People/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [OutputCache(Duration = 0, NoStore = true)]
        public ActionResult Edit(EditProfile editProf)
        {
            if (ModelState.IsValid)
            {
                Person persondetails = db.People.Find(User.Identity.Name);
                if (persondetails != null)
                {
                    persondetails.fullName = editProf.fullName;
                    persondetails.email = editProf.emailID;
                    persondetails.active = editProf.active;


                    db.Entry(persondetails).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Details");
                }
            }
            return View(editProf);
        }
        [Authorize]
        [OutputCache(Duration = 0, NoStore = true)]
        public ActionResult UpdatePassword()
        {
            if (User.Identity.Name == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.People.Find(User.Identity.Name);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [OutputCache(Duration = 0, NoStore = true)]
        public ActionResult UpdatePassword(UpdatePassword updpass)
        {
            if (ModelState.IsValid)
            {
                Person person = db.People.Find(User.Identity.Name);
                if (Helper.EncodePasswordMd5(updpass.oldpassWord) == person.password)
                {
                    if (Helper.EncodePasswordMd5(updpass.NewpassWord) == Helper.EncodePasswordMd5(updpass.ConfirmpassWord))
                    {
                        person.password = Helper.EncodePasswordMd5(updpass.ConfirmpassWord);
                        db.Entry(person).State = EntityState.Modified;
                        db.SaveChanges();
                        ViewBag.PasswordUpdateSuccess = true;
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "New password not matching with confirm password..!");
                        return View();

                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid old password..!");                   
                    return View();
                }
               
            }
            return View();
        }

        // GET: People/Delete/5
        [Authorize]
        [OutputCache(Duration = 0, NoStore = true)]
        public ActionResult Delete()
        {
            if (User.Identity.Name == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.People.Find(User.Identity.Name);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }

        // POST: People/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        [OutputCache(Duration = 0, NoStore = true)]
        public ActionResult DeleteConfirmed()
        {
            var twt = db.TWEETs.Where(x => x.user_id == User.Identity.Name);
            db.TWEETs.RemoveRange(twt);
            db.SaveChanges();
            Person person = db.People.Find(User.Identity.Name);
            db.People.Remove(person);
            db.SaveChanges();
            System.Web.Security.FormsAuthentication.SignOut();
            return RedirectToAction("login", "SignIn");
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
