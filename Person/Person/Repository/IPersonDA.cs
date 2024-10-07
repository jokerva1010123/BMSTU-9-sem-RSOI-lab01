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
        List<PersonDB> FindAll();
        PersonDB FindById(int id);
        PersonDB Add(PersonDB obj);
        ExitCode Patch(PersonDB obj);
        ExitCode DeleteById(int id);
    }
}