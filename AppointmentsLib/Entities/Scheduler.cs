using System;
using System.Collections.Generic;
using System.Linq;

namespace AppointmentsLib.Entities
{
    public class Scheduler
    {
     
        #region use_free_minutes_range

        public List<int> FindOverlappedMinutes(List<Calendar> calendars)
        {
     
            if (calendars == null || calendars.Count == 0 ) { throw new ArgumentException("Brak kalendarzy do sprawdzenia"); }

            List<int> freeMinutes = null;

            for (int i = 0; i < calendars.Count; i++)
            {
               
                Calendar calendar = calendars[i];

                int minutesStart = calendar.WorkingHours.Start.TimeStringToMinutes();
                int minutesEnd = calendar.WorkingHours.End.TimeStringToMinutes();

                freeMinutes =  (i == 0) ? Enumerable.Range(minutesStart, minutesEnd - minutesStart + 1).ToList() // inicjacja lisy zakresu wolnych minut
                    : CorrectFreeMinutesByWorkHours(freeMinutes, minutesStart, minutesEnd);   //ograniczenie do godzin pracy
               
                // ograniczenie wolnych minut przez terminy spotkań
                foreach (var planed in calendar.PlannedMeeting)
                {
                    minutesStart = planed.Start.TimeStringToMinutes();
                    minutesEnd = planed.End.TimeStringToMinutes();

                    CorrectFreeMinutesByPlannedTime(freeMinutes, minutesStart, minutesEnd);
                }

            }

            return freeMinutes ;
        }


        private List<int> CorrectFreeMinutesByWorkHours(List<int> freeMinutes,int minutesStart, int minutesEnd)
        {
            if (minutesStart > freeMinutes.First())
            {
                freeMinutes.RemoveRange(0, minutesStart - freeMinutes.First());
            }
            if (minutesEnd < freeMinutes.Last())
            {
                int idxMinEnd = freeMinutes.IndexOf(minutesEnd);
                if (idxMinEnd > 0 && freeMinutes[idxMinEnd] - freeMinutes[idxMinEnd] > 1) { idxMinEnd++;  }
                freeMinutes.RemoveRange(idxMinEnd, freeMinutes.Count - idxMinEnd );
            }

            return freeMinutes;
        }

        private void CorrectFreeMinutesByPlannedTime(List<int> freeMinutes, int minutesStart, int minutesEnd)
        {

            var minutesToRemove = freeMinutes.Where(x => x > minutesStart && x < minutesEnd);
            if (minutesToRemove != null && minutesToRemove.Count() > 0)
            {
                int idxMinStart = freeMinutes.IndexOf(minutesStart);
                if (idxMinStart == 0 ||(idxMinStart > 0 && ( freeMinutes[idxMinStart] - freeMinutes[idxMinStart-1] > 1)))
                { freeMinutes.Remove(minutesStart); }

             
                if (freeMinutes.Last() == minutesEnd) { freeMinutes.Remove(minutesEnd); }

                freeMinutes.RemoveAll(x => minutesToRemove.Any(y => y == x));
            }

        }

        #endregion use_free_minutes_range
    }
}
