using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;


namespace LeaveManagementSystem.Helpers
{
    public class GoogleCalendarAPI
    {
        //clien id 850856402899-im1tqfivs0t4jmtdkpp37fv2o2ddsc6o.apps.googleusercontent.com

        //client secret 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


        public void CreateEvent(string userEmail)
        {
           //we can always encrypt the file, but for simplicity we will leave it as is.
            string jsonFile = "C:\\Users\\kylea\\source\\repos\\LeaveManagementSystem\\LeaveManagementSystem\\Helpers\\direct-landing-367407-3e73e4e3c9b1.json";
            string calendarId =  ConfigurationManager.AppSettings["google_calendar_id"];

            string[] Scopes = { CalendarService.Scope.Calendar };

            ServiceAccountCredential credential;

            using(var stream = new FileStream(jsonFile,FileMode.Open,FileAccess.Read))
            {
                var confg = Google.Apis.Json.NewtonsoftJsonSerializer.Instance.Deserialize<JsonCredentialParameters>(stream);
                credential = new ServiceAccountCredential(
                   new ServiceAccountCredential.Initializer(confg.ClientEmail)
                   {
                       Scopes = Scopes
                   }.FromPrivateKey(confg.PrivateKey));
            }

            var service = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "Calendar API Sample",
            });

            var calendar = service.Calendars.Get(calendarId).Execute();

            // Define parameters of request.
            /*EventsResource.ListRequest listRequest = service.Events.List(calendarId);
            listRequest.TimeMin = DateTime.Now;
            listRequest.ShowDeleted = false;
            listRequest.SingleEvents = true;
            listRequest.MaxResults = 10;
            listRequest.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;*/



             List<Event> e =
             new List<Event>() {
                new Event(){
                    Id = "eventid" + 1,
                    Summary = "Leave Request",
                    Location = "Office HQ",
                    Description = "Review and update leave request",
                    Start = new EventDateTime()
                    {
                        DateTime = new DateTime(2022, 11, 10, 15, 30, 0),
                        TimeZone = "Africa/Johannesburg",
                    },
                    End = new EventDateTime()
                    {
                        DateTime = new DateTime(2022, 11, 10, 16, 00, 0),
                        TimeZone = "Africa/Johannesburg",
                    },
                     Recurrence = new List<string> { "RRULE:FREQ=DAILY;COUNT=1" },
                    Attendees = new List<EventAttendee>
                    {
                       // new EventAttendee() { Email = "test45@gmail.com"/*userEmail*/ },
                        //new EventAttendee() { Email = "kyleand71@gmail.com"}
                    }
                }
             };

            var myevent = e.Find(x => x.Id == "eventid" + 1);

            var InsertRequest = service.Events.Insert(myevent, calendarId);

            try
            {
                InsertRequest.Execute();
            }
            catch (Exception ex)
            {

                service.Events.Update(myevent, calendarId, myevent.Id).Execute();
            }



        }

        
    }
}