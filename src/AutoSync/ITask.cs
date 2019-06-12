
namespace AutoSync
{
    public interface ITask
    {
        int ID { get; set; }
        string TaskType { get; }
        int Progress { get; }
        string Description { get; }
    }

    public interface ITask<TArg> : ITask
    {
        void Run(TArg arg);
    }
}
