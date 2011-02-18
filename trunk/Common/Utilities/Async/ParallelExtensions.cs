using System.Threading.Tasks.Schedulers;

namespace System.Threading.Tasks
{
    public static class ParallelExtensions
    {
        public static Task StartNew(this TaskFactory factory, Action action, TaskScheduler scheduler)
        {
            return factory.StartNew(action, CancellationToken.None, TaskCreationOptions.None, scheduler);
        }

        public static Task<TResult> StartNew<TResult>(this TaskFactory factory, Func<TResult> action, TaskScheduler scheduler)
        {
            return factory.StartNew<TResult>(action, CancellationToken.None, TaskCreationOptions.None, scheduler);
        }

        
        public static Task<TResult> StartNewSta<TResult>(this TaskFactory factory, Func<TResult> action)
        {
            return factory.StartNew<TResult>(action, sharedScheduler);
        }

        private static TaskScheduler sharedScheduler = new StaTaskScheduler(1);
    }
}
