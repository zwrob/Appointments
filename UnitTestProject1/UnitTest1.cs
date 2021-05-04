using AppointmentsLib;
using AppointmentsLib.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using static AppointmentsLib.DataAdapter;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        readonly string testFilesPath = @"../../../Data/";

        [TestMethod]
        public void TestImportCalendarOk()
        {
            string[] calendarFiles = new string[] { "testCalendar1.json", "testCalendar4.json" };

            foreach (string calendarFile in calendarFiles)
            {
                string jsonCalendar = File.ReadAllText(Path.Combine(testFilesPath, calendarFile));

                jsonCalendar = jsonCalendar.Replace("\n", "").Replace("\r", "");
                jsonCalendar = Regex.Replace(jsonCalendar, @"\s+", "");

                var calendar = ImportCalendarFromFile(Path.Combine(testFilesPath, calendarFile));

                Assert.IsNotNull(calendar);

                string testJson = calendar.ToJson<Calendar>();

                Assert.AreEqual(jsonCalendar, testJson);
            }

        }


        [TestMethod]
        public void TestImportCalendarError()
        {
            string[] calendarFiles = new string[] { "testCalendarErr.json" };

            foreach (string calendarFile in calendarFiles)
            {
                string jsonCalendar = File.ReadAllText(Path.Combine(testFilesPath, calendarFile));

                jsonCalendar = jsonCalendar.Replace("\n", "").Replace("\r", "");
                jsonCalendar = Regex.Replace(jsonCalendar, @"\s+", "");

                var calendar = ImportCalendarFromFile(Path.Combine(testFilesPath, calendarFile));

                Assert.IsNotNull(calendar);

                string testJson = calendar.ToJson<Calendar>();

                Assert.AreEqual(jsonCalendar, testJson);
            }

        }

        [TestMethod]
        public void TestFindSharedTime1()
        {

            Scheduler scheduler = new Scheduler();

            List<Calendar> calendars = new List<Calendar>() {
                ImportCalendarFromFile(Path.Combine(testFilesPath, "testCalendar1.json")),
                ImportCalendarFromFile(Path.Combine(testFilesPath, "testCalendar2.json"))
            };

            var sharedTimes = scheduler.FindSharedMinutes(calendars).ToActiveHoursList();

            string result = File.ReadAllText(Path.Combine(testFilesPath, "resultTestFindSharedTime1.txt"));
            Assert.AreEqual(result, sharedTimes.ToGroupString());


        }


        [TestMethod]
        public void TestFindSharedTime2()
        {

            Scheduler scheduler = new Scheduler();

            List<Calendar> calendars = new List<Calendar>() {
                ImportCalendarFromFile(Path.Combine(testFilesPath, "testCalendar3.json")),
                ImportCalendarFromFile(Path.Combine(testFilesPath, "testCalendar4.json"))
            };

            var sharedTimes = scheduler.FindSharedMinutes(calendars).ToActiveHoursList();

            string result = File.ReadAllText(Path.Combine(testFilesPath, "resultTestFindSharedTime2.txt"));
            Assert.AreEqual(result, sharedTimes.ToGroupString());


        }
    }
}
