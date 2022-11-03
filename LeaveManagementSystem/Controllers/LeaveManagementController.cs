using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Web;
using System.Web.Mvc;
using LeaveManagementSystem.DataAccess;
using LeaveManagementSystem.Models;
using LeaveManagementSystem.ViewModels;

namespace LeaveManagementSystem.Controllers
{
    public class LeaveManagementController : Controller
    {
       
        public ActionResult Requests()
        {
            //validate that user is logged in
            if (Session["username"].ToString() == null)
            {
                return View("~/Views/Account/Login.cshtml");
            }
            else
            {
                return View();
            }

            
        }


        public ActionResult Create()
        {
            //validate that user is logged in
            if (Session["username"].ToString() == null)
            {
                return View("~/Views/Account/Login.cshtml");
            }
            else
            {
                EmployeeInfo info = new EmployeeInfo();
                TempData["availableLeaveDays"] = info.GetAvailabeLeaveDays((int)Session["userId"]);

                return View();
               
            }
            

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Submit(LeaveRequest request)
        {
            //validate that user is logged in
            if (Session["username"].ToString() == null)
            {
                return View("~/Views/Account/Login.cshtml");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    //check that start date is before end date
                    if(request.EndDate < request.StartDate)
                    {
                        TempData["RequestStatus"] = "Invalid date range, start date must be before end date.";
                        return View("Create");
                    }
                    else
                    {
                        //save and submit request
                        SubmitRequest submission = new SubmitRequest();
                        string result = submission.Submit(request, (int)Session["userId"], Session["username"].ToString());

                        if (result.Contains("failed"))
                        {
                            TempData["RequestStatus"] = result;
                        }
                        else
                        {
                            //get request id and redirect to success page
                            EmployeeInfo info = new EmployeeInfo();
                            double temp = info.GetRequestedLeaveDays((int.Parse(result)));
                            TempData["requestedLeaveDays"] = temp.ToString();


                            return View("~/Views/Home/Success.cshtml");
                        }

                        return View("Create");
                    }
                    
                }
                else
                {
                    TempData["RequestStatus"] = "Invalid data, please try again";
                    return View("Create");
                }


                
            }


        }

        public ActionResult ViewMyRequests(int id)
        {
            //validate that user is logged in
            if (Session["username"].ToString() == null)
            {
                return View("~/Views/Account/Login.cshtml");
            }
            else
            {
                //get requests
                GetRequests getRequests = new GetRequests();
                var request = getRequests.GetForUser(id);

                if (request.Count == 0)
                {
                    TempData["count"] = "0";
                }
                else
                {
                    TempData["count"] = request.Count.ToString();
                }
                    

                return View(request);

            }

               
        }

        public ActionResult List()
        {
            //validate that user is logged in
            if (Session["username"].ToString() == null)
            {
                return View("~/Views/Account/Login.cshtml");
            }
            else if(Session["userType"].ToString() != "Admin")
            {
                return View("~/Views/Home/Index.cshtml");
            }
            else
            {

                //get requests
                GetRequests getRequests = new GetRequests();
                var request = getRequests.GetAll();

                if (request.Count == 0)
                {
                    TempData["count"] = "0";
                }
                else
                {
                    TempData["count"] = request.Count.ToString();
                }
                    

                return View(request);

            }


        }

        public ActionResult Success()
        {
            //validate that user is logged in
            if (Session["username"].ToString() == null)
            {
                return View("~/Views/Account/Login.cshtml");
            }
            else
            {
                EmployeeInfo info = new EmployeeInfo();
                TempData["requestedDays"] = info.GetRequestedLeaveDays((int)Session["userId"]);
                return View("~/Views/Home/Success.cshtml");
            }

        }
    }
}