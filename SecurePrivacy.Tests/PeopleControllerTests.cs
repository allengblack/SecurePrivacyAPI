using Bogus;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SecurePrivacy.API.Controllers;
using SecurePrivacy.API.Models;
using SecurePrivacy.API.Services;
using System;
using System.Threading.Tasks;
using Xunit;
using Person = SecurePrivacy.API.Models.Person;

namespace SecurePrivacy.Tests
{
    public class PeopleControllerTests
    {
        [Fact]
        public async Task Can_Create_Person()
        {
            var person = new Faker<Person>()
                .RuleFor(p => p.Name, x => x.Person.FullName)
                .RuleFor(p => p.Age, _ => 30)
                .RuleFor(p => p.Address, x => x.Person.Address.City)
                .Generate();

            var service = new Mock<IPersonService>();
            service.Setup(service => service.Create(It.IsAny<PersonDto>()))
                .ReturnsAsync(person);

            var controller = new PeopleController(service.Object);

            var result = await controller.Create(new PersonDto { Name = person.Name, Address = person.Address, Age = person.Age });

            Assert.Equal(result.Age, person.Age);
            Assert.Equal(result.Name, person.Name);
        }

        [Fact]
        public async Task Can_Get_Person()
        {
            var id = Guid.NewGuid();
            var person = new Faker<Person>()
                .RuleFor(p => p.Name, x => x.Person.FullName)
                .RuleFor(p => p.Age, _ => 30)
                .RuleFor(p => p.Address, x => x.Person.Address.City)
                .RuleFor(p => p.Id, _ => id.ToString())
                .Generate();

            var service = new Mock<IPersonService>();
            service.Setup(service => service.Get(It.IsAny<string>()))
                .ReturnsAsync(person);

            var controller = new PeopleController(service.Object);
            var actionResult = await controller.GetById(id.ToString());
            var result = actionResult.Value;

            Assert.Equal(result.Id, person.Id);
        }

        [Fact]
        public async Task Can_Get_AllPeople()
        {
            var id = Guid.NewGuid();
            var people = new Faker<Person>()
                .RuleFor(p => p.Name, x => x.Person.FullName)
                .RuleFor(p => p.Age, _ => 30)
                .RuleFor(p => p.Address, x => x.Person.Address.City)
                .RuleFor(p => p.Id, _ => id.ToString())
                .Generate(10);

            var service = new Mock<IPersonService>();
            service.Setup(service => service.GetAll())
                .ReturnsAsync(people);

            var controller = new PeopleController(service.Object);
            var result = await controller.Get();

            Assert.Equal(10, result.Count);
        }

        [Fact]
        public async Task Can_Delete_Person()
        {
            var id = Guid.NewGuid();
            var person = new Faker<Person>()
                .RuleFor(p => p.Name, x => x.Person.FullName)
                .RuleFor(p => p.Age, _ => 30)
                .RuleFor(p => p.Address, x => x.Person.Address.City)
                .RuleFor(p => p.Id, _ => id.ToString())
                .Generate();

            var service = new Mock<IPersonService>();

            service.Setup(service => service.Get(It.IsAny<string>()))
                .ReturnsAsync(person);

            service.Setup(service => service.Remove(It.IsAny<string>())).ReturnsAsync(person);

            var controller = new PeopleController(service.Object);
            var actionResult = await controller.Delete(id.ToString());

            Assert.NotNull(actionResult);
        }

        [Fact]
        public async Task Can_Update_Person()
        {
            var person = new Faker<Person>()
                .RuleFor(p => p.Name, x => x.Person.FullName)
                .RuleFor(p => p.Age, _ => 30)
                .RuleFor(p => p.Address, x => x.Person.Address.City)
                .RuleFor(p => p.Id, x => x.Random.Guid().ToString())
                .Generate();

            var dto = new Faker<PersonDto>()
                .RuleFor(p => p.Name, x => x.Person.FullName)
                .RuleFor(p => p.Age, _ => 30)
                .RuleFor(p => p.Address, x => x.Person.Address.City)
                .Generate();

            var service = new Mock<IPersonService>();

            service.Setup(service => service.Get(It.IsAny<string>()))
                .ReturnsAsync(person);

            service.Setup(service => service.Update(It.IsAny<string>(), dto))
                .ReturnsAsync(new Person 
                {
                    Address = dto.Address,
                    Name = dto.Name,
                    Age = dto.Age,
                    Id = person.Id
                });

            var controller = new PeopleController(service.Object);
            var actionResult = await controller.Update(person.Id, dto);
            var result = actionResult.Value;

            Assert.Equal(person.Id, result.Id);
        }

        [Fact]
        public async Task Can_Get_Stats()
        { 
            var stats = new Faker<PersonGroup>()
                .RuleFor(p => p.Category, x => x.Random.Word())
                .RuleFor(p => p.Count, x => x.Random.Number(0, 10))
                .Generate(5);

            var service = new Mock<IPersonService>();
            service.Setup(service => service.Stats())
                .ReturnsAsync(stats);

            var controller = new PeopleController(service.Object);
            var result = await controller.GetStats();

            Assert.Equal(5, result.Count);
        }
    }
}
