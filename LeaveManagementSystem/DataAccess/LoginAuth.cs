using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using LeaveManagementSystem.Models;
using LeaveManagementSystem.Helpers;
using Microsoft.Ajax.Utilities;

namespace LeaveManagementSystem.DataAccess
{
    public class LoginAuth
    {

        //getting con string from .config file
        string cnn = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;


        //return user id 
        public int ValidateLogin(User user)
        {
            int id = 0;
            //a user will always have an employee record,
            //since you cannot access the system without being an employee

            //initialising sql connection with string in config file
            using(SqlConnection conn = new SqlConnection(cnn))
            {
                conn.Open();

                //hash password
                string pwdHash = HashGenerator.generateMD5hash(user.Password);

                //building query, lookup user in database
                SqlCommand cmd = new SqlCommand("select * from Users where Username = '"+user.Username+"' and PasswordHash = '"+pwdHash+"' ", conn);

                //run query
                SqlDataReader sdr = cmd.ExecuteReader();

                //check query results
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        //user exists, get id
                        id = (int)sdr[0];
                        
                    }
                   
                    
                }


                sdr.Close();
            }
            
            return id;
        }

        public void updateLastLogin(string username)
        {
            if (!string.IsNullOrWhiteSpace(username))
            {

                //initialising sql connection with string in config file
                using (SqlConnection conn = new SqlConnection(cnn))
                {
                    conn.Open();

                    //building query, update user last login
                    SqlCommand cmd = new SqlCommand("update Users set LastLogin = '" + DateTime.Now + "' where Username = '"+username+"' ", conn); 

                    //run query
                    SqlDataReader sdr = cmd.ExecuteReader();

                    sdr.Close();

                }

            }
        }

        public string GetUserType(string username)
        {
            if (!string.IsNullOrWhiteSpace(username))
            {

                //initialising sql connection with string in config file
                using (SqlConnection conn = new SqlConnection(cnn))
                {
                    conn.Open();

                    //building query, update user last login
                    SqlCommand cmd = new SqlCommand("select UserType from Users where Username = '" + username + "' ", conn);

                    //run query
                    SqlDataReader sdr = cmd.ExecuteReader();

                    while (sdr.Read())
                    {
                        return sdr["UserType"].ToString();
                    }

                    sdr.Close();
                   
                    

                }

            }
            return string.Empty;
        }

    }
}