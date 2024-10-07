using Microsoft.EntityFrameworkCore;
using DBContext;
using Repository;
using DataAccess;
using Microsoft.Extensions.Logging.Abstractions;

namespace PeopleTests
{
    public class PeopleAccessObject : IDisposable
    {
        public AppDbContext peopleContext { get; }
        public IPersonDA personRepository { get; }

        public PeopleAccessObject()
        {
            var builder = new DbContextOptionsBuilder<AppDbContext>();
            builder.UseInMemoryDatabase("person");

            peopleContext = new AppDbContext(builder.Options);
            personRepository = new PersonDA(peopleContext, NullLogger<PersonDA>.Instance);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                peopleContext.Database.EnsureDeleted();
                peopleContext?.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}