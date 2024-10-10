using Model;

namespace Repository
{
    public enum ExitCode
    {
        Success,
        Constraint,
        Error
    }
    public interface IPersonDA
    {
        List<PersonDB> GetAll();
        PersonDB GetById(int id);
        PersonDB Add(PersonDB obj);
        ExitCode Update(PersonDB obj);
        ExitCode DeleteById(int id);
    }
}