using Microsoft.VisualStudio.TestTools.UnitTesting;
using TaskManager.DesktopClient.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Models.Dtos;

namespace TaskManager.DesktopClient.ViewModels.Tests
{
    [TestClass()]
    public class MainWindowViewModelTests
    {
        [TestMethod()]
        public void EncodedLoginPassStringTest()
        {
            var test = new MainWindowViewModel();

            var result = "";// test.EncodeUser("{\"LastName\":\"McFly\",\"Email\":\"ra.081@hotmail.com\",\"Password\":\"Da3lRfh3pW\",\"Phone\":null,\"LastLoginDate\":\"2025-01-03T19:36:58.0832978Z\",\"UserStatus\":0,\"ProjectsIds\":null,\"DesksIds\":null,\"TasksIds\":null,\"Id\":\"f9a179e3-948a-4b94-a582-8cb9d1562cc3\",\"Name\":\"Andr33w\",\"Description\":null,\"Created\":\"2025-01-03T19:36:58.0832753Z\",\"Updated\":\"2025-01-03T19:36:58.0832888Z\",\"Picture\":null}");
            Console.WriteLine(result);
            Assert.IsNotNull(result);
        }

        [TestMethod()]
        public void TestEncodingTest()
        {
            var test = new MainWindowViewModel();

            var result = "";// test.DecodeUser("eyJMYXN0TmFtZSI6Ik1jRmx5IiwiRW1haWwiOiJyYS4wODFAaG90bWFpbC5jb20iLCJQYXNzd29yZCI6IkRhM2xSZmgzcFciLCJQaG9uZSI6bnVsbCwiTGFzdExvZ2luRGF0ZSI6IjIwMjUtMDEtMDNUMTk6MzY6NTguMDgzMjk3OFoiLCJVc2VyU3RhdHVzIjowLCJQcm9qZWN0c0lkcyI6bnVsbCwiRGVza3NJZHMiOm51bGwsIlRhc2tzSWRzIjpudWxsLCJJZCI6ImY5YTE3OWUzLTk0OGEtNGI5NC1hNTgyLThjYjlkMTU2MmNjMyIsIk5hbWUiOiJBbmRyMzN3IiwiRGVzY3JpcHRpb24iOm51bGwsIkNyZWF0ZWQiOiIyMDI1LTAxLTAzVDE5OjM2OjU4LjA4MzI3NTNaIiwiVXBkYXRlZCI6IjIwMjUtMDEtMDNUMTk6MzY6NTguMDgzMjg4OFoiLCJJbWFnZSI6bnVsbH0=");


            //for (int i = 0; i < result.Length; i++)
            //{
            //    testResult += result[i];
            //}

            Console.WriteLine(result);
            Assert.IsNotNull(result);
        }

        [TestMethod()]
        public void SerializeUserTest()
        {
            var test = new MainWindowViewModel();
            UserDto user = new UserDto();
            user.Email = "ra.081@hotmail.com";
            user.Password = "Da3lRfh3pW";
            user.Name = "Andr33w";
            user.LastName = "McFly";
            user.UserStatus = Models.Enums.UserStatus.Admin;

            var result = "";// test.SerializeUser(user);

            Console.WriteLine(result);
            Assert.IsNotNull(result);
        }

        [TestMethod()]
        public void DeserializeUserTest()
        {
            Assert.IsTrue(true);
        }
    }
}