using DBContext;
using Model;
using Repository;

namespace DataAccess
{
    public class PersonDA : IPersonDA, IDisposable
    {
        private readonly AppDbContext appDB;
        public PersonDA(AppDbContext createDB)
        {
            appDB = createDB;
        }
        public List<PersonDB> GetAll()
        {
            var people = appDB.People.ToList();
            return people;
        }
        public PersonDB GetById(int id)
        {
            var person = appDB.People.Find(id);
            return person;
        }
        public PersonDB Add(PersonDB obj)
        {
            try
            {
                var id = appDB.People.Count() + 1;

                obj.id = id;
                appDB.People.Add(obj);
                appDB.SaveChanges();
                return obj;
            }
            catch (Exception err)
            {
                throw;
            }
        }
        public ExitCode Update(PersonDB obj)
        {
            try
            {
                appDB.People.Update(obj);
                appDB.SaveChanges();
                return ExitCode.Success;
            }
            catch (Exception err)
            {
                return ExitCode.Error;
            }
        }
        public ExitCode DeleteById(int id)
        {
            try
            {
                var food = GetById(id);
                appDB.People.Remove(food);
                appDB.SaveChanges();
                return ExitCode.Success;
            }
            catch (Exception err)
            {
                return ExitCode.Error;
            }
        }

        public void Dispose()
        {
            appDB.Dispose();
        }
    }
}