using LeaveManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace LeaveManagementSystem.DataAccess
{
    public class EmployeeInfo
    {
        //getting con string from .config file
        string cnn = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;

        public double GetAvailabeLeaveDays(int id)
        {
            double days = 0;

            using (SqlConnection conn = new SqlConnection(cnn))
            {
                conn.Open();
                
                
                SqlCommand getAllLeaveDays = new SqlCommand("select * from Employees where Id = '" + id+ "' ", conn);
                SqlDataReader sdr = getAllLeaveDays.ExecuteReader();

                while (sdr.Read())
                {
                    //i'm only looking for the available leave days now,
                    //but when can grab the entire row, and change the return type of the method.
                    string temp = sdr["AvailableLeaveDays"].ToString();
                    days = double.Parse(temp);


                }

                sdr.Close();
            }

           
            return days;
        }

        public double GetRequestedLeaveDays(int id)
        {
            double days = 0;

            using (SqlConnection conn = new SqlConnection(cnn))
            {
                conn.Open();

                SqlCommand getAllLeaveDays = new SqlCommand("select * from LeaveRequests where Id = '" + id + "' ", conn);
                SqlDataReader sdr = getAllLeaveDays.ExecuteReader();

                while (sdr.Read())
                {
                    
                    string temp = sdr["TotalLeaveDays"].ToString();
                    days = double.Parse(temp);


                }

                sdr.Close();
            }


            return days;
        }

    }
}