using LeaveManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LeaveManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //validate that user is logged in
            if(Session["username"].ToString() == null)
            {
                return View("~/Views/Account/Login.cshtml");
            }
            else
            {
                return View();
            }

            
        }

        
    }
}