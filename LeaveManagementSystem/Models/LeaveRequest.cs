using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LeaveManagementSystem.Models
{
    public class LeaveRequest
    {
        [Display(Name = "Request Id")]
        public int Id{ get; set; }

        [Display(Name = "First name")]
        [Required (ErrorMessage = "First name is required")]
        public string FirstName { get; set; }

        [Display(Name = "Lastname")]
        [Required(ErrorMessage = "Lastname is required")]
        public string LastName { get; set; }

        [Display(Name = "When do you want to take off?")]
        [Required(ErrorMessage = "Start date is required")]
        public DateTime StartDate { get; set; }


        [Display(Name = "When you will be back?")]
        [Required(ErrorMessage = "End date is required")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Leave type")]
        [Required(ErrorMessage = "Leave type is required")]
        public string LeaveType { get; set; }

        [Display(Name = "Total days off")]
        public string TotalDays { get; set; }

        [Display(Name = "Your remaining leave days")]
        public string RemainingLeaveDays { get; set; }

        [Display(Name = "Status of your request")]
        public string RequestStatus { get; set; }

        [Required(ErrorMessage = "Reason is required")]
        [MaxLength(500)]
        public string Reason { get; set; }

    }
}