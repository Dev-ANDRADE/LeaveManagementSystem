using LeaveManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace LeaveManagementSystem.DataAccess
{
    public class GetRequests
    {
        //getting con string from .config file
        string cnn = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;

        public List<LeaveRequest> GetAll()
        {
            List<LeaveRequest> list = new List<LeaveRequest>();

            using (SqlConnection conn = new SqlConnection(cnn))
            {
                conn.Open();
                

                //here we could use the user's id, but since the application is small, we won't.
                SqlCommand getAllLeaveDays = new SqlCommand("select * from LeaveRequests", conn);
                SqlDataReader sdr = getAllLeaveDays.ExecuteReader();

                while (sdr.Read())
                {
                    //we could use an ORM here, but for the sake of time and simplicity I will manually map the objects
                    LeaveRequest request = new LeaveRequest();

                    //mapping from sql to c# object
                    request.FirstName = sdr["FirstName"].ToString();
                    request.LastName = sdr["LastName"].ToString();
                    request.StartDate = (DateTime)sdr["StartDate"];
                    request.EndDate = (DateTime)sdr["EndDate"];
                    request.LeaveType = sdr["LeaveType"].ToString();
                    request.Reason = sdr["Reason"].ToString();
                    request.TotalDays = sdr["TotalLeaveDays"].ToString();
                    request.RemainingLeaveDays = sdr["RemainingLeaveDays"].ToString();
                    request.RequestStatus = sdr["RequestStatus"].ToString();

                    //adding obj to list
                    list.Add(request);
                }

                
               
            }

            return list;
        }

        public List<LeaveRequest> GetForUser(int id)
        {

            List<LeaveRequest> list = new List<LeaveRequest>();

            using (SqlConnection conn = new SqlConnection(cnn))
            {
                conn.Open();

                SqlCommand getAllLeaveDays = new SqlCommand("select * from LeaveRequests where employee_id = '"+id+"' ", conn);
                SqlDataReader sdr = getAllLeaveDays.ExecuteReader();

                while (sdr.Read())
                {
                    //we could use an ORM here, but for the sake of time and simplicity I will manually map the objects
                    LeaveRequest request = new LeaveRequest();

                    //mapping from sql to c# object
                    request.FirstName = sdr["FirstName"].ToString();
                    request.LastName = sdr["LastName"].ToString();
                    request.StartDate = (DateTime)sdr["StartDate"];
                    request.EndDate = (DateTime)sdr["EndDate"];
                    request.LeaveType = sdr["LeaveType"].ToString();
                    request.Reason = sdr["Reason"].ToString();
                    request.TotalDays = sdr["TotalLeaveDays"].ToString();
                    request.RemainingLeaveDays = sdr["RemainingLeaveDays"].ToString();
                    request.RequestStatus = sdr["RequestStatus"].ToString();

                    //adding obj to list
                    list.Add(request);
                }



            }

            return list;
        }

    }
}