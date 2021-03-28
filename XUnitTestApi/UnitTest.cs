using ApiHallOfFame;
using ApiHallOfFame.Controllers;
using ApiHallOfFame.Interfaces;
using ApiHallOfFame.Models;
using ApiHallOfFame.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace XUnitTestApi
{
    public class UnitTest
    {
        private Mock<IPersonService> MockRepository { get; set; } = new Mock<IPersonService>();
        private PersonController Controller { get; set; }
        private PersonRepository Repository { get; set; }
        public UnitTest()
        {
            Controller = new PersonController(MockRepository.Object);
            DbContextOptions<HallOfFameContext> options = new DbContextOptionsBuilder<HallOfFameContext>()
                              .UseInMemoryDatabase("Testing").Options;
            Repository = new PersonRepository(new HallOfFameContext(options));
        }

        [Fact]
        public void TestGetPerson()
        {
            var person = GetData().FirstOrDefault();
            MockRepository.Setup(x => x.GetPersonAsync(person.Id)).ReturnsAsync(person);

            var getPerson = Controller.GetPerson(person.Id);

            Assert.Equal(person.Id, getPerson.Result.Value.Id);
        }

        [Fact]
        public void TestGetPersons()
        {
            var persons = GetData();
            MockRepository.Setup(x => x.GetPersonsAsync()).ReturnsAsync(persons);

            var getPersons = Controller.GetPersons();

            Assert.Equal(getPersons.Result.Count(), persons.Count);
        }

        [Fact]
        public async Task TestPostPersonContext()
        {
            var count = Repository.ListAsync().Result.Count();
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
            await Repository.AddAsync(person);
            await Repository.AddAsync(secondPerson);
            var newCount = Repository.ListAsync().Result.Count();
            Assert.NotEqual(count, newCount);
        }

        [Fact]
        public void TestGetPersonsContext()
        {
            Assert.NotEmpty(Repository.ListAsync().Result);
        }

        [Fact]
        public void TestGetPersonContext()
        {
            Assert.NotNull(Repository.FindByIdAsync(1).Result);
        }

        [Fact]
        public async Task TestPutPersonContext()
        {
            Person person = Repository.FindByIdAsync(1).Result;
            string name = person.Name;
            person.Name = "updateTest";
            await Repository.Update(person);
            Assert.NotEqual(person.Name, name);
        }

        [Fact]
        public async Task TestDeletePersonContext()
        {
            var count = Repository.ListAsync().Result.Count();
            var person = Repository.FindByIdAsync(2).Result;
            await Repository.Remove(person);
            Assert.NotEqual(count,count - 1);
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
