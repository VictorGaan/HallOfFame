using Microsoft.VisualStudio.TestTools.UnitTesting;
using BackendHallOfFame.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
using ApiHallOfFame.Models;
using Microsoft.AspNetCore.Mvc;
using Castle.Core.Logging;

namespace BackendHallOfFame.Controllers.Tests
{
    [TestClass()]
    public class PeopleControllerTests
    {
        [TestMethod()]
        public void GetPeopleTest()
        {
            var mock = new Mock<IPerson>();
            mock.Setup(x => x.GetPersons()).Returns(TestData());
            var controller = new PeopleController(mock.Object, null);

            var result = controller.Index() as ViewResult;
            var model = Xunit.Assert.IsAssignableFrom<IEnumerable<Person>>(result.ViewData.Model);
            Xunit.Assert.Equal(2, model.Count());
        }
        public List<Person> TestData()
        {
            List<Person> people = new List<Person>()
            {
                new Person{Id=1, DisplayName="1",Name="1"},
                new Person{Id=2 ,DisplayName="2",Name="2"},
            };
            return people;
        }
    }
}