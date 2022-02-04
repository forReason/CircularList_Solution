using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CircularList.StorageBlock_Namespace
{
    internal partial class StorageBlockManager<Type>
    {
        ConcurrentQueue<Type> AppendQueue = new ConcurrentQueue<Type>();
        public async Task Append(Type value)
        {
            AppendQueue.Enqueue(value);
            lock(AppendLockObject)
            { 
                if (WorkAppendQueue_Task == null || WorkAppendQueue_Task.IsCompleted)
                {
                    WorkAppendQueue_Task = WorkAppendQueue();
                }
            }
        }
        public async Task AppendRange(Type[] valueArray)
        {
            lock (AppendLockObject)
            {
                foreach(Type value in valueArray)
                {
                    AppendQueue.Enqueue(value);
                }
                if (WorkAppendQueue_Task == null || WorkAppendQueue_Task.IsCompleted)
                {
                    WorkAppendQueue_Task = WorkAppendQueue();
                }
            }
        }
        public object AppendLockObject = new object();
        public Task WorkAppendQueue_Task;
        private async Task WorkAppendQueue()
        {
            while (AppendQueue.Any())
            {
                Type element;
                bool success = AppendQueue.TryDequeue(out element);
                if (!success) continue;
                LastBlock.Data[LastBlock.LastFreeElement] = element;
                LastBlock.LastFreeElement++;
                LastBlock.Length++;
                Length++;
                // Block is full, move on to the next block and create a new one
                if (LastBlock.LastFreeElement >= LastBlock.Data.Length)
                {
                    if (AddBlockTask != null)
                    {
                        await AddBlockTask;
                    }
                    LastBlock = LastBlock.NextBlock;
                    AddBlockTask = AddBlock(wasLastBlockNotFirstBlock: true);
                }
            }
        }
    }
}
