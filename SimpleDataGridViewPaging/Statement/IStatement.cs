namespace Code4Bugs.SimpleDataGridViewPaging.Statement
{
    public interface IStatement<out T>
    {
        T Execute();
    }
}