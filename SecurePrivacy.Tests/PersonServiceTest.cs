using AutoMapper;
using Bogus;
using MongoDB.Driver;
using SecurePrivacy.API.Models;
using SecurePrivacy.API.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Person = SecurePrivacy.API.Models.Person;

namespace SecurePrivacy.Tests
{
    public class PersonServiceTest : IDisposable
    {
        private readonly IMongoClient _client;
        private readonly IMongoCollection<Person> _collection;
        private readonly Mapper _mapper;

        public PersonServiceTest()
        {
            _client = new MongoClient("mongodb://localhost:27017/");
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MapperProfile());
            });

            _mapper = (Mapper)mappingConfig.CreateMapper();
            _collection = _client.GetDatabase("test").GetCollection<Person>(nameof(Person));
        }

        public void Dispose()
        {
            _client.DropDatabase("test");
        }

        [Fact]
        public async Task Can_Create_Person()
        {
            var dto = new Faker<PersonDto>()
               .RuleFor(p => p.Name, x => x.Person.FullName)
               .RuleFor(p => p.Age, _ => 30)
               .RuleFor(p => p.Address, x => x.Person.Address.City)
               .Generate();

            var service = new PersonService(_collection, _mapper);
            var result = await service.Create(dto);

            Assert.NotNull(result.Id);
            Assert.Equal(dto.Name, result.Name);
        }

        [Fact]
        public async Task Can_Get_Person()
        {
            var dto = new Faker<PersonDto>()
               .RuleFor(p => p.Name, x => x.Person.FullName)
               .RuleFor(p => p.Age, _ => 30)
               .RuleFor(p => p.Address, x => x.Person.Address.City)
               .Generate();

            var service = new PersonService(_collection, _mapper);
            var person = await service.Create(dto);

            var result = await service.Get(person.Id);

            Assert.Equal(result.Id, person.Id);
            Assert.Equal(result.Name, person.Name);
        }

        [Fact]
        public async Task Can_Get_People()
        {
            var dto = new Faker<PersonDto>()
               .RuleFor(p => p.Name, x => x.Person.FullName)
               .RuleFor(p => p.Age, _ => 30)
               .RuleFor(p => p.Address, x => x.Person.Address.City)
               .Generate(2);

            var service = new PersonService(_collection, _mapper);
             await service.Create(dto[0]);
             await service.Create(dto[1]);

            var result = await service.GetAll();

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task Can_Remove_People()
        {
            var dto = new Faker<PersonDto>()
               .RuleFor(p => p.Name, x => x.Person.FullName)
               .RuleFor(p => p.Age, _ => 30)
               .RuleFor(p => p.Address, x => x.Person.Address.City)
               .Generate();

            var service = new PersonService(_collection, _mapper);
            var person = await service.Create(dto);
            var check = await service.Get(person.Id);
            Assert.NotNull(check.Id);
            Assert.Equal(dto.Name, check.Name);

            await service.Remove(person.Id);

            var result = await service.GetAll();
            Assert.Empty(result);
        }

        [Fact]
        public async Task Can_Update_People()
        {
            var dto = new Faker<PersonDto>()
               .RuleFor(p => p.Name, x => x.Person.FullName)
               .RuleFor(p => p.Age, _ => 30)
               .RuleFor(p => p.Address, x => x.Person.Address.City)
               .Generate();

            var dto2 = new Faker<PersonDto>()
               .RuleFor(p => p.Name, x => x.Person.FullName)
               .RuleFor(p => p.Age, _ => 50)
               .RuleFor(p => p.Address, x => x.Person.Address.City)
               .Generate();

            var service = new PersonService(_collection, _mapper);
            var person = await service.Create(dto);

            await service.Update(person.Id, dto2);

            var result = await service.Get(person.Id);

            Assert.Equal(result.Id, person.Id);
            Assert.Equal(result.Name, dto2.Name);
        }
    }
}
