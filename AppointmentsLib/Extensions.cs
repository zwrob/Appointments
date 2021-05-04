
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;
using AppointmentsLib.Entities;

namespace AppointmentsLib
{
    public static class Extensions
    {
        public static T ToObject<T>(this string jsonString)
        {
            JsonSerializer serializer = new JsonSerializer();
            using JsonReader reader = new JsonTextReader(new StringReader(jsonString));
            return (T)serializer.Deserialize(reader, typeof(T));

        }

        public static string ToJson<T>(this T obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        // zamienia czas hh:mm na ilośc minut, np :  9*60 + 30
        public static int TimeStringToMinutes(this string timeString) 
        {
            var timeElements = Regex.Replace(timeString, @"\s+", "").Split(":")
                .Select(x => int.Parse(x)).ToArray();

            if (((timeElements == null) ? 0 : timeElements.Length) != 2 ) { 
                throw new ArgumentException($"To nie jest format czasu : {timeString}");
            }


            return timeElements[0]*60 + timeElements[1];
        }

        // zamienia minuty na czas hh:mm , np 570 -> 09:30
        public static string MinutesToTimeString(this int minutes)
        {
            int hrs = minutes / 60;
            int min = minutes - hrs * 60;
            return $"{hrs:00}:{min:00}";
        }

        // zamienia listę dostępnych minut na liste przedziałów czasowych
        public static List<ActiveHours> ToActiveHoursList(this List<int> minutesList)
        {
            List<ActiveHours> activeHours = new List<ActiveHours>();

            if (minutesList != null && minutesList.Count > 0)
            {
                int previousMinute = -1;
                for (int i = 0; i < minutesList.Count; i++)
                {
                    int minute = minutesList[i];

                    if (minute - previousMinute > 1)
                    {
                        // koncic starego przedziału i początek nowego
                        if (activeHours.Count > 0 ) { activeHours.Last().End = previousMinute.MinutesToTimeString(); }

                        activeHours.Add(new ActiveHours()
                        {
                            Start = minute.MinutesToTimeString() // konwerdja na np 09:00
                        });    
                       
                    }

                    previousMinute = minute;
                }

                // jeszcze ustawienie end dla ostatniego elementu
                if (activeHours.Count > 0) { activeHours.Last().End = minutesList.Last().MinutesToTimeString(); }
            }


            return activeHours;
        }

        public static string ToGroupString(this List<ActiveHours> hours)
        {
            var formatedHours = hours.Select(x => $"[\"{x.Start}\",\"{x.End}\"]").ToList();
            return  $"[{ string.Join(',', formatedHours)}]";
        }
    }
}
