using AppointmentsLib.Entities;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;

namespace AppointmentsLib
{
    public static class DataAdapter
    {
        public static Calendar ImportCalendarFromFile(string fileName)
        {
            string jsonCalendar = File.ReadAllText(fileName);

            Calendar calendar = null;


            JObject objg = JObject.Parse(jsonCalendar);

            if (objg.ContainsKey("working_hours"))
            {
                var workingHours = objg["working_hours"].ToObject(typeof(ActiveHours)) as ActiveHours;
                calendar = new Calendar(workingHours);
            }

            if (calendar == null) { throw new ArgumentException("Nie udało się utworzyć kalendarza, niepoprawne dane"); }

            if (objg.ContainsKey("planned_meeting"))
            {
                List<ActiveHours> plannedMeetings = objg["planned_meeting"].ToObject(typeof(List<ActiveHours>)) as List<ActiveHours>;
                if (plannedMeetings != null)
                {
                    foreach (var p in plannedMeetings)
                    {
                        calendar.AddPlannedMeeting(p);
                    }
                }
            }

            return calendar;
            // return jsonCalendar.ToObject<Calendar>(); //można użyć ale wtedy nie ma sprawdzenia poprawosci wstawianych danych
        }

    }
}
