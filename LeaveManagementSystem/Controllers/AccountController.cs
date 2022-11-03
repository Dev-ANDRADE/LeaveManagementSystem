using LeaveManagementSystem.DataAccess;
using LeaveManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LeaveManagementSystem.Controllers
{
    public class AccountController : Controller
    {
        
        public ActionResult Login()
        {
            Session.Clear();

            return View();
        }

        [HttpPost]
        public ActionResult Login(User user)
        {
            //check data
            if (ModelState.IsValid)
            {
                //check details
                LoginAuth loginAuth = new LoginAuth();

                int id = loginAuth.ValidateLogin(user);

                if (id !=0 )
                {
                    //login success
                    //TempData["LoginStatus"] = "Login success";
                    Session["username"] = user.Username;
                    Session["userType"] = loginAuth.GetUserType(user.Username);
                    Session["userId"] = id;

                    loginAuth.updateLastLogin(user.Username);

                    return View("~/Views/Home/Index.cshtml");

                }
                else
                {
                    //invalid credentials
                    TempData["LoginStatus"] = "Invalid credentials provided, try again";
                    return View("Login");
                }

            }
            else
            {
                //model not valid
                TempData["LoginStatus"] = "Invalid data, try again";
                return View("Login");
            }

           
        }
    }
}