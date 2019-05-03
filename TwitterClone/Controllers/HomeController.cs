using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TwitterClone.Filters;
using TwitterClone.Models;

namespace TwitterClone.Controllers
{
    [CustomExceptionHelperFilter]
    public class HomeController : Controller
    {
        private TwitterCloneDBEntities1 db = new TwitterCloneDBEntities1();
        
        // GET: Home
        [Authorize]
        [OutputCache(Duration =0,NoStore =true)]
        public ActionResult Index()
        {
            Person person = db.People.Find(User.Identity.Name);
            return View(person);
        }
      
       

        [Authorize]
        [OutputCache(Duration = 0, NoStore = true)]
        public JsonResult SearchPerson(string PersionUserId)
        {
            string person_Id = string.Empty;
            var Srhperson = db.People.Find(PersionUserId);
            if (Srhperson != null)
                person_Id = Srhperson.user_id;
            var FollowingChk = db.FOLLOWINGs.Where(x => x.user_id == User.Identity.Name && x.following_id == PersionUserId).Select(x => x.user_id).Count();
            Dictionary<string, string> dt = new Dictionary<string, string>();          
            dt.Add("PersionId", person_Id);
            dt.Add("FollowCheck", FollowingChk.ToString());
            var jsonobject = JsonConvert.SerializeObject(dt);
            return Json(jsonobject, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize]
        [OutputCache(Duration = 0, NoStore = true)]
        public ActionResult PostTwitte(string messagePost)
        {
            TWEET twts = new TWEET();
            twts.message = messagePost;
            twts.Created = DateTime.Now;
            twts.user_id = User.Identity.Name;        
            db.TWEETs.Add(twts);
            db.SaveChanges();
            string message = "SUCCESS";
            return Json(new { Message = message, JsonRequestBehavior.AllowGet });
        }

        [HttpPost]
        [Authorize]
        [OutputCache(Duration = 0, NoStore = true)]
        public ActionResult FollowPerson(string PersionUserId)
        {
            FOLLOWING flw = new FOLLOWING();
            flw.user_id = User.Identity.Name;
            flw.following_id = PersionUserId;
            object message = "";
            var FollowingChk = db.FOLLOWINGs.Where(x => x.user_id == User.Identity.Name && x.following_id == PersionUserId).Select(x => x.user_id).Count();
            if (FollowingChk == 0)
            {
                db.FOLLOWINGs.Add(flw);
                message = "SUCCESS";
            }
            else
            {
                FOLLOWING FOLLOWINGperson = db.FOLLOWINGs.Where(x=>x.user_id== flw.user_id && x.following_id== flw.following_id).FirstOrDefault();
                db.FOLLOWINGs.Remove(FOLLOWINGperson);
                message = "DeleteSUCCESS";
            }
            db.SaveChanges();
            var jsonobject = JsonConvert.SerializeObject(message);
            return Json(jsonobject, JsonRequestBehavior.AllowGet );
        }


        [Authorize]
        [OutputCache(Duration = 0, NoStore = true)]
        public JsonResult GetTwitte()
        {        
            var twt = db.TWEETs.OrderByDescending(cr => cr.Created).Select(x => new { user_id = x.user_id, messaage = x.message, created = x.Created }).ToList();                   
            var jsonobject = JsonConvert.SerializeObject(twt);
           return Json(jsonobject, JsonRequestBehavior.AllowGet);
        }

        
        [Authorize]
        [OutputCache(Duration = 0, NoStore = true)]
        public JsonResult GetTwittecount()
        {
            var twt = db.TWEETs.Select(x => x.tweet_id ).Count();

            var jsonobject = JsonConvert.SerializeObject(twt);
            return Json(jsonobject, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [OutputCache(Duration = 0, NoStore = true)]
        public JsonResult GetFollowingFollowercount()
        {
         
            var FollowingCnt = db.FOLLOWINGs.Where(x => x.user_id == User.Identity.Name).Select(x => x.user_id).Count();
            var FollowerCnt = db.FOLLOWINGs.Where(x => x.following_id == User.Identity.Name).Select(x => x.following_id).Count();
            Dictionary<string, string> cnt = new Dictionary<string, string>();
            cnt.Add("FollowingCnt", FollowingCnt.ToString());
            cnt.Add("FollowerCnt", FollowerCnt.ToString());

            var jsonobject = JsonConvert.SerializeObject(cnt);
            return Json(jsonobject, JsonRequestBehavior.AllowGet);
        }


    }
}