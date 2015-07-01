namespace DateAccess.Services.ContactService.Call.Scripts.Visitors
{
    /// <summary>
    /// node visitor for different behaviour
    /// </summary>
    public interface IVisitor
    {
        bool Visit<T>(T node);
    }
}
