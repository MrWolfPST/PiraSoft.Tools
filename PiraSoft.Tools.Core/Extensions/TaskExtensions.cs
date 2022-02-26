namespace System.Threading.Tasks
{
    public static class TaskExtensions
    {
        public static T ExecuteSync<T>(this Task<T> task)
        {
            AutoResetEvent autoResetEvent = new AutoResetEvent(false);
            IAsyncResult asyncResult = task.ContinueWith(r => autoResetEvent.Set());
            autoResetEvent.WaitOne();
            return task.Result;
        }

        public static T ExecuteSync<T>(this Task<T> task, int millisecondsTimeout)
        {
            AutoResetEvent autoResetEvent = new AutoResetEvent(false);
            IAsyncResult asyncResult = task.ContinueWith(r => autoResetEvent.Set());
            autoResetEvent.WaitOne(millisecondsTimeout);
            return task.Result;
        }

        public static void ExecuteSync(this Task task)
        {
            AutoResetEvent autoResetEvent = new AutoResetEvent(false);
            IAsyncResult asyncResult = task.ContinueWith(r => autoResetEvent.Set());
            autoResetEvent.WaitOne();
        }

        public static void ExecuteSync(this Task task, int millisecondsTimeout)
        {
            AutoResetEvent autoResetEvent = new AutoResetEvent(false);
            IAsyncResult asyncResult = task.ContinueWith(r => autoResetEvent.Set());
            autoResetEvent.WaitOne(millisecondsTimeout);
        }
    }
}
