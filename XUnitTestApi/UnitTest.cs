using ApiHallOfFame;
using ApiHallOfFame.Controllers;
using ApiHallOfFame.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Xunit;

namespace XUnitTestApi
{
    public class UnitTest
    {
        private Mock<IPerson> MockPerson { get; set; } = new Mock<IPerson>();
        private Mock<ILogger<PersonController>> MockLogger { get; set; } = new Mock<ILogger<PersonController>>();
        private PersonController Controller { get; set; }

        private PersonRepository Repository { get; set; }
        private HallOfFameContext Context { get; set; }
        public UnitTest()
        {
            Controller = new PersonController(MockPerson.Object, MockLogger.Object);
            var contextOptions = new DbContextOptionsBuilder<HallOfFameContext>()
              .UseSqlServer(@"Server=DESKTOP-OB3VG27;Database=HallOfFame;Trusted_Connection=True;MultipleActiveResultSets=true")
              .Options;
            Context = new HallOfFameContext(contextOptions);
            Repository = new PersonRepository(Context);
        }

        [Fact]
        public void TestGetPerson()
        {
            long id = 1;
            var person = GetData().FirstOrDefault();
            MockPerson.Setup(x => x.GetPerson(person.Id)).ReturnsAsync(person);

            var getPerson = Controller.GetPerson(id);

            Assert.Equal(id, getPerson.Result.Value.Id);
        }

        [Fact]
        public void TestGetPersons()
        {
            var persons = GetData();
            MockPerson.Setup(x => x.GetPersons()).ReturnsAsync(persons);

            var getPersons = Controller.GetPersons();

            Assert.Equal(getPersons.Result.Count(), persons.Count);
        }

        [Fact]
        public void TestDeletePerson()
        {
            var persons = GetData();
            var person = persons.FirstOrDefault();
            MockPerson.Setup(x => x.GetPerson(person.Id)).ReturnsAsync(person);
            MockPerson.Setup(x => x.DeletePerson(person));

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
            MockPerson.Setup(x => x.InsertPerson(person));

            var postPerson = Controller.PostPerson(person).Result;

            Assert.Equal(HttpStatusCode.Created, (HttpStatusCode)postPerson.GetType().GetProperty("StatusCode").GetValue(postPerson, null));
        }

        [Fact]
        public void TestPutPerson()
        {
            var person = GetData().FirstOrDefault();
            person.Name = "updatePerson";
            person.DisplayName = "updatePerson";
            MockPerson.Setup(x => x.UpdatePerson(person));

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
            MockPerson.Setup(x => x.UpdatePerson(person));

            var putPerson = Controller.PutPerson(1, person).Result;

            Assert.Equal(HttpStatusCode.BadRequest, (HttpStatusCode)putPerson.GetType().GetProperty("StatusCode").GetValue(putPerson, null));
        }

        [Fact]
        public void TestDeletePersonWithoutRealId()
        {
            var id = 15;

            var deletePerson = Controller.DeletePerson(id).Result;

            Assert.Equal(HttpStatusCode.NotFound, (HttpStatusCode)deletePerson.GetType().GetProperty("StatusCode").GetValue(deletePerson, null));
        }

        [Fact]
        public void TestGetPersonsContext()
        {
            var persons = Repository.GetPersons().Result.ToList();
            Assert.NotEmpty(persons);
        }

        [Fact]
        public void TestGetPersonContext()
        {
            var person = Repository.GetPerson(1).Result;
            Assert.NotNull(person);
        }

        [Fact]
        public void TestPutPersonContext()
        {
            var person = Repository.GetPerson(1).Result;
            Repository.UpdatePerson(person);
            Assert.Equal(EntityState.Modified,Context.Entry(person).State);
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
            Repository.InsertPerson(person);
            Context.SaveChanges();
            Assert.NotEqual(count, count+1);
        }
        [Fact]
        public void TestDeletePersonContext()
        {
            var count = Repository.GetPersons().Result.Count();
            var lastPerson = Repository.GetPersons().Result.LastOrDefault();
            Repository.DeletePerson(lastPerson);
            Context.SaveChanges();
            Assert.NotEqual(count, count - 1);
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
