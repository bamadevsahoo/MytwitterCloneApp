using System;
using System.Collections.Generic;
using System.Linq;

using System.Web;
using System.Web.Security;
using System.Web.Mvc;
using TwitterClone.Models;
using TwitterClone.Filters;

namespace TwitterClone.Controllers
{
    [CustomExceptionHelperFilter]
    public class SignInController : Controller
    {
        private TwitterCloneDBEntities1 db = new TwitterCloneDBEntities1();
        // GET: SignIn
        public ActionResult login()
        {
            return View();
        }


        // POST: SignIn/login
        [HttpPost]
       
        public ActionResult login(Account accDetails)
        {
            try
            {             
                // TODO: Add login logic here
                if (ModelState.IsValid)
                {
                    Person person = db.People.Find(accDetails.userName);
                    if (person != null)
                    {

                        if (accDetails.userName == person.user_id &&  Helper.EncodePasswordMd5(accDetails.passWord) == person.password) 
                        {
                            FormsAuthentication.SetAuthCookie(person.user_id, false);
                           
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Inalid password");
                            ViewBag.Inalidlogin = "Incorrect credential..!";
                            return View();
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid user name");
                        ViewBag.Inalidlogin = "Invalid user please signup today";
                        return View();
                    }                  
                }
                return View();
            }
            catch(Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message.ToString());
                return View();
            }
        }

        public ActionResult SignUp()
        {
            return View();
        }

        // POST: SignIn/SignUp
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignUp([Bind(Include = "user_id,password,fullName,email,joined,active")]Person personDetails)
        {
            try
            {
                // TODO: Add insert logic here
                if (ModelState.IsValid)
                {
                    try
                    {
                        Person person = db.People.Find(personDetails.user_id);
                        if (person == null)
                        {
                            personDetails.joined = DateTime.Now;
                            personDetails.active = true;
                            personDetails.password = Helper.EncodePasswordMd5(personDetails.password);
                            db.People.Add(personDetails);
                            db.SaveChanges();
                            ViewBag.SignupSuccess = "True";
                            ModelState.Clear();
                            return View();
                        }
                        else
                        {
                            ViewBag.SuccessMessage = "An account already exist with UserName " + personDetails.user_id;
                            return View();
                        }

                    }
                    catch(Exception exec)
                    {
                        ViewBag.errorMessage = "Oops there is an error:- " + exec.Message;
                        return View();
                    }
                                      
                }
                return View();
            }
            catch 
            {
                return View("login");
            }
        }

        [OutputCache (Duration =0, NoStore =true)]
        
        public ActionResult signout()
        {
            FormsAuthentication.SignOut();
            return View("login");

        }


    }
}
