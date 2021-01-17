using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace SimpleJiraProject.UnitTests
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void Validate_RightEmail_ReturnTrue()
        {
            GeneralValidation val = new GeneralValidation();
            string email = "right@email.com";
            Assert.IsTrue(val.IsValidEmail(email));
        }

        [TestMethod]
        public void Validate_WrongEmail_ReturnFalse()
        {
            GeneralValidation val = new GeneralValidation();
            string email = "wrong";
            Assert.IsFalse(val.IsValidEmail(email));
        }

        [TestMethod]
        public void Validate_WrongPassword_ReturnFalse()
        {
            GeneralValidation val = new GeneralValidation();
            string pwd = "wrong";
            Assert.IsFalse(val.IsValidEmail(pwd));
        }

        [TestMethod]
        public void Validate_RightPassword_ReturnTrue()
        {
            GeneralValidation val = new GeneralValidation();
            string pwd = "rightpassword";
            Assert.IsFalse(val.IsValidEmail(pwd));
        }

        [TestMethod]
        public void Validate_RightName_ReturnTrue()
        {
            GeneralValidation val = new GeneralValidation();
            string name = "name";
            Assert.IsFalse(val.IsValidEmail(name));
        }

        [TestMethod]
        public void Validate_WrongName_ReturnFale()
        {
            GeneralValidation val = new GeneralValidation();
            Assert.IsFalse(val.IsValidEmail(null));
        }

        [TestMethod]
        public void Validate_WrongDescription_ReturnFale()
        {
            GeneralValidation val = new GeneralValidation();
            string desc = string.Join("", Enumerable.Repeat(0, 256).Select(n => (char)new Random().Next(127)));
            Assert.IsFalse(val.IsValidDescription(desc));
        }

        [TestMethod]
        public void Validate_RightDescription_ReturnTrue()
        {
            GeneralValidation val = new GeneralValidation();
            string desc = "Description";
            Assert.IsTrue(val.IsValidDescription(desc));
        }

        [TestMethod]
        public void Validate_RightDate_ReturnTrue()
        {
            GeneralValidation val = new GeneralValidation();
            DateTime date1 = DateTime.Now;
            DateTime date2 = DateTime.Now.AddDays(2);
            Assert.IsTrue(val.IsValidDate(date1, date2));
        }

        [TestMethod]
        public void Validate_WrongDate_ReturnFalse()
        {
            GeneralValidation val = new GeneralValidation();
            DateTime date1 = DateTime.Now;
            DateTime date2 = DateTime.Now.AddDays(2);
            Assert.IsFalse(val.IsValidDate(date2, date1));
        }

        [TestMethod]
        public void Validate_WrongPoint_ReturnFalse()
        {
            GeneralValidation val = new GeneralValidation();
            int pt = 0;
            Assert.IsFalse(val.IsValidPoint(pt));
            pt = 100;
            Assert.IsFalse(val.IsValidPoint(pt));
        }

        [TestMethod]
        public void Validate_RightPoint_ReturnTrue()
        {
            GeneralValidation val = new GeneralValidation();
            int pt = 20;
            Assert.IsTrue(val.IsValidPoint(pt));
        }

    }
}
