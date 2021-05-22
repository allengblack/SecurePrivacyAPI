using AutoMapper;
using MongoDB.Bson;
using MongoDB.Driver;
using SecurePrivacy.API.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace SecurePrivacy.API.Services
{
    public class PersonService : IPersonService
    {
        private IMongoCollection<Person> _people;
        private IMapper _mapper;

        public PersonService(IMongoCollection<Person> people, IMapper mapper)
        {
            _people = people;
            _mapper = mapper;
        }

        public async Task<Person> Create(PersonDto dto)
        {
            var person = _mapper.Map<Person>(dto);
            await _people.InsertOneAsync(person);
            return person;
        }

        public async Task<IEnumerable<Person>> GetAll()
        {
            return await _people.Find(p => true).ToListAsync();
        }

        public async Task<Person> Get(string id) => await _people.Find(p => p.Id == id).FirstOrDefaultAsync();

        public async Task<Person> Update(string id, PersonDto dto)
        {
            var dbPerson = await Get(id);
            if (dbPerson == null)
            {
                return null;
            }

            var person = _mapper.Map<Person>(dto);
            person.Id = id;
            await _people.ReplaceOneAsync(p => p.Id == person.Id, person);
            return person;
        }

        public async Task<Person> Remove(string id)
        {
            var dbPerson = await Get(id);
            if (dbPerson == null)
            {
                return null;
            }

            await _people.DeleteOneAsync(p => p.Id == id);
            return dbPerson;
        }

        public async Task<List<PersonGroup>> Stats()
        {
            var result = _people
                .Aggregate()
                .Group(p => p.Category, agg => new PersonGroup { Category = agg.Key, Count = agg.Sum(p => 1) });
            return await result.ToListAsync();
        }
    }

    public interface IPersonService
    {
        Task<Person> Create(PersonDto person);
        Task<Person> Get(string id);
        Task<IEnumerable<Person>> GetAll();
        Task<Person> Remove(string id);
        Task<Person> Update(string id, PersonDto person);
        Task<List<PersonGroup>> Stats();
    }
}
