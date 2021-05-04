using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppointmentsLib.Entities
{
    public class Calendar
    {
        [JsonProperty("working_hours")]
        public ActiveHours WorkingHours { get; private set; }

        [JsonProperty("planned_meeting")]
        public List<ActiveHours> PlannedMeeting { get; private set; }

        public Calendar(ActiveHours workingHours)
        {
            this.WorkingHours = workingHours;
            this.PlannedMeeting = new List<ActiveHours>();
        }

        public bool AddPlannedMeeting(ActiveHours meeting)
        {
            // sprawdzenie czy dodawany meeting mieści się w przedziale godzin pracy
            if (meeting.Start.TimeStringToMinutes() < this.WorkingHours.Start.TimeStringToMinutes() ||
                  meeting.End.TimeStringToMinutes() > this.WorkingHours.End.TimeStringToMinutes())
            {
                throw new Exception("Spotkanie przekracza czas pracy");
            }

            this.PlannedMeeting.Add(meeting);
            return true;
        }

        public bool RemovePlannedMeeting(ActiveHours meeting)
        {
            return this.PlannedMeeting.Remove(meeting);
        }
    }
}
