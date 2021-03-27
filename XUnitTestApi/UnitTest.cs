using ApiHallOfFame;
using ApiHallOfFame.Controllers;
using ApiHallOfFame.Models;
using Interfaces.ApiHallOfFame;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Repositories.ApiHallOfFame;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Xunit;

namespace XUnitTestApi
{
    public class UnitTest
    {
        private Mock<IRepositoryManager> MockRepository { get; set; } = new Mock<IRepositoryManager>();
        private Mock<ILogger<PersonController>> MockLogger { get; set; } = new Mock<ILogger<PersonController>>();
        private PersonController Controller { get; set; }

        private PersonRepository Repository { get; set; }
        public UnitTest()
        {
            Controller = new PersonController(MockRepository.Object, MockLogger.Object);
            DbContextOptions<HallOfFameContext> options = new DbContextOptionsBuilder<HallOfFameContext>()
                              .UseInMemoryDatabase("Testing").Options;
            Repository = new PersonRepository(new HallOfFameContext(options));
        }

        [Fact]
        public void TestGetPerson()
        {
            var person = GetData().FirstOrDefault();
            MockRepository.Setup(x => x.Person.GetPerson(person.Id)).ReturnsAsync(person);

            var getPerson = Controller.GetPerson(person.Id);

            Assert.Equal(person.Id, getPerson.Result.Value.Id);
        }

        [Fact]
        public void TestGetPersons()
        {
            var persons = GetData();
            MockRepository.Setup(x => x.Person.GetPersons()).ReturnsAsync(persons);

            var getPersons = Controller.GetPersons();

            Assert.Equal(getPersons.Result.Count(), persons.Count);
        }

        [Fact]
        public void TestDeletePerson()
        {
            var person = GetData().FirstOrDefault();
            MockRepository.Setup(x => x.Person.GetPerson(person.Id)).ReturnsAsync(person);
            MockRepository.Setup(x => x.Person.DeletePerson(person));

            var deletePerson = Controller.DeletePerson(person.Id).Result;

            Assert.Equal(HttpStatusCode.OK, (HttpStatusCode)deletePerson.GetType().GetProperty("StatusCode").GetValue(deletePerson, null));
        }


        [Fact]
        public void TestPostPerson()
        {
            var persons = GetData();
            Person person = new Person()
            {
                Id = 3,
                Name = "newPerson",
                DisplayName = "newPerson",
            };
            MockRepository.Setup(x => x.Person.CreatePerson(person));

            var postPerson = Controller.PostPerson(person).Result;

            Assert.Equal(HttpStatusCode.Created, (HttpStatusCode)postPerson.GetType().GetProperty("StatusCode").GetValue(postPerson, null));
        }

        [Fact]
        public void TestPutPerson()
        {
            var person = GetData().FirstOrDefault();
            person.Name = "updatePerson";
            person.DisplayName = "updatePerson";
            MockRepository.Setup(x => x.Person.UpdatePerson(person));

            var putPerson = Controller.PutPerson(person.Id, person).Result;

            Assert.Equal(HttpStatusCode.OK, (HttpStatusCode)putPerson.GetType().GetProperty("StatusCode").GetValue(putPerson, null));
        }


        [Fact]
        public void TestPutPersonWithChangeId()
        {
            var person = GetData().FirstOrDefault();
            person.Id = 100;
            person.Name = "updatePerson";
            person.DisplayName = "updatePerson";
            MockRepository.Setup(x => x.Person.UpdatePerson(person));

            var putPerson = Controller.PutPerson(1, person).Result;

            Assert.Equal(HttpStatusCode.BadRequest, (HttpStatusCode)putPerson.GetType().GetProperty("StatusCode").GetValue(putPerson, null));
        }

        [Fact]
        public void TestDeletePersonWithoutRealId()
        {
            long id = 101;
            MockRepository.Setup(x => x.Person.GetPerson(id));

            var deletePerson = Controller.DeletePerson(id).Result;

            Assert.Equal(HttpStatusCode.NotFound, (HttpStatusCode)deletePerson.GetType().GetProperty("StatusCode").GetValue(deletePerson, null));
        }
        [Fact]
        public void TestPostPersonContext()
        {
            var count = Repository.GetPersons().Result.Count();
            var person = new Person()
            {
                Name = "test",
                DisplayName = "test"
            };
            var secondPerson = new Person()
            {
                Name = "secondTest",
                DisplayName = "secondTest"
            };
            Repository.CreatePerson(person);
            Repository.CreatePerson(secondPerson);
            Repository.Save();
            var counta = Repository.GetPersons().Result.Count();

            Assert.NotEqual(count, counta);
        }
        [Fact]
        public void TestGetPersonsContext()
        {
            Assert.NotEmpty(Repository.GetPersons().Result);
        }

        [Fact]
        public void TestGetPersonContext()
        {
            Assert.NotNull(Repository.GetPerson(1).Result);
        }

        [Fact]
        public void TestPutPersonContext()
        {
            Person person = Repository.GetPerson(1).Result;
            string name = person.Name;
            person.Name = "updateTest";
            Repository.UpdatePerson(person);
            Repository.Save();
            Assert.NotEqual(person.Name, name);
        }

        [Fact]
        public void TestDeletePersonContext()
        {
            var count = Repository.GetPersons().Result.Count();
            var person = Repository.GetPerson(2).Result;
            Repository.DeletePerson(person);
            Repository.Save();
            Assert.NotEqual(count - 1, count);
        }
        public List<Person> GetData()
        {
            return new List<Person>()
                {
                    new Person(){Id=1,Name="Test1",DisplayName="Test1" },
                    new Person(){Id=2,Name="Test2",DisplayName="Test2",Skills=new List<Skill>() { new Skill() {Id=1,Level=1,Name="firstSkill",PersonId=2, } } }
                };
        }
    }
}
