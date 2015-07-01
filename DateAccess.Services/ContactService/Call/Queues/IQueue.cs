namespace DateAccess.Services.ContactService.Call.Queues
{
    public interface IQueue<out T>
    {
        T GetQueue();
    }
}
