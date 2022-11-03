using LeaveManagementSystem.Helpers;
using LeaveManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace LeaveManagementSystem.DataAccess
{
    public class SubmitRequest
    {
        //getting con string from .config file
        string cnn = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;

        public string Submit(LeaveRequest leaveRequest, int userId, string username)
        {
            using(SqlConnection conn = new SqlConnection(cnn))
            {
                conn.Open();
                double availableLeaveDays =0;

                //get amount of days
                double totalDays =  (leaveRequest.EndDate - leaveRequest.StartDate).TotalDays;
                    
                        
                SqlCommand getAllLeaveDays = new SqlCommand("select AvailableLeaveDays from Employees where FirstName = '"+leaveRequest.FirstName+"' and LastName = '"+leaveRequest.LastName+"' ", conn);
                SqlDataReader sdr = getAllLeaveDays.ExecuteReader();

                while (sdr.Read())
                {
                    availableLeaveDays = double.Parse(sdr["AvailableLeaveDays"].ToString());
                }
                sdr.Close();

                //check for valid date range
               if(totalDays < availableLeaveDays && availableLeaveDays != 0)
                {
                    //employee qualifies for leave.
                    double remainingDays = availableLeaveDays - totalDays;

                    //submit and insert request
                    SqlCommand submit = new SqlCommand("insert into LeaveRequests " +
                        "(FirstName, " +
                        "LastName, " +
                        "StartDate, " +
                        "EndDate, " +
                        "LeaveType, " +
                        "Reason," +
                        "TotalLeaveDays, " +
                        "RemainingLeaveDays, " +
                        "RequestStatus," +
                        "employee_id ) " +
                        "OUTPUT Inserted.ID " +
                        "values (" +
                        "'"+leaveRequest.FirstName+"'," +
                        "'"+leaveRequest.LastName+"'," +
                        "'"+leaveRequest.StartDate+"'," +
                        "'"+leaveRequest.EndDate+"'," +
                        "'"+leaveRequest.LeaveType+"'," +
                        "'"+leaveRequest.Reason+"'," +
                        ""+(int)totalDays+"," +
                        ""+(int)remainingDays+"," +
                        "'Requested and pending'," +
                        ""+userId+")", conn);

                   
                    //submit.ExecuteNonQuery();

                    int id = (int)submit.ExecuteScalar();

                    //update remaining leave days in emp table
                    UpdateRemainingLeaveDays(userId, (int)remainingDays);

                    if(id != 0)
                    {
                        //on successful save, create google calendar event.
                        GoogleCalendarAPI calendar_api = new GoogleCalendarAPI();
                        calendar_api.CreateEvent(username);
                    }

                    return id.ToString();
                }
                else
                {
                    return "failed,total leave is more than available days";
                }
            }



        }

        public void UpdateRemainingLeaveDays(int userId, int days)
        {
            using (SqlConnection conn = new SqlConnection(cnn))
            {
                conn.Open();
                
                SqlCommand getAllLeaveDays = new SqlCommand("update Employees set AvailableLeaveDays = '"+days+"' where Id = '" + userId + "'", conn);
                SqlDataReader sdr = getAllLeaveDays.ExecuteReader();

                
                sdr.Close();

            }



        }


    }
}