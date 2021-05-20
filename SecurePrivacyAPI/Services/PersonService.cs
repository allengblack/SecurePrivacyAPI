using AutoMapper;
using MongoDB.Driver;
using SecurePrivacyAPI.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;

namespace SecurePrivacyAPI.Services
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

        public async Task Remove(string id) => await _people.DeleteOneAsync(p => p.Id == id);
    }

    public interface IPersonService
    {
        Task<Person> Create(PersonDto person);
        Task<Person> Get(string id);
        Task<IEnumerable<Person>> GetAll();
        Task Remove(string id);
        Task<Person> Update(string id, PersonDto person);
    }
}
