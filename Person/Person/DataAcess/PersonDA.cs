using DBContext;
using Model;
using Repository;

namespace DataAccess
{
    public class PersonDA : IPersonDA, IDisposable
    {
        private readonly AppDbContext _db;
        private readonly ILogger<PersonDA> _logger;

        public PersonDA(AppDbContext createDB, ILogger<PersonDA> logDB)
        {
            _db = createDB;
            _logger = logDB;
        }

        public List<PersonDB> FindAll()
        {
            var people = _db.People.ToList();
            return people;
        }

        public PersonDB FindById(int id)
        {
            var person = _db.People.Find(id);
            return person;
        }

        public PersonDB Add(PersonDB obj)
        {
            try
            {
                var id = _db.People.Count() + 1;

                obj.id = id;
                _db.People.Add(obj);
                _db.SaveChanges();

                _logger.LogInformation("+PersonRep : Person {Number} was added to People", obj.id);
                return obj;
            }
            catch (Exception err)
            {
                _logger.LogError(err, "+PersonRep : Error trying to add person to People");
                throw;
            }
        }

        public ExitCode Patch(PersonDB obj)
        {
            try
            {
                _db.People.Update(obj);
                _db.SaveChanges();
                _logger.LogInformation("+PersonRep : Person {Number} was updated at People", obj.id);
                return ExitCode.Success;
            }
            catch (Exception err)
            {
                _logger.LogError(err, "+PersonRep : Error trying to update person to People");
                return ExitCode.Error;
            }
        }

        public ExitCode DeleteById(int id)
        {
            try
            {
                var food = FindById(id);
                _db.People.Remove(food);
                _db.SaveChanges();
                _logger.LogInformation("+PersonRep : Person {Number} was deleted from People", id);
                return ExitCode.Success;
            }
            catch (Exception err)
            {
                _logger.LogError(err, "+PersonRep : Error trying to delete person {Number} from People", id);
                return ExitCode.Error;
            }
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}