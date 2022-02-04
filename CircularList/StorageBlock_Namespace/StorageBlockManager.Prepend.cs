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
        ConcurrentQueue<Type> PrependQueue = new ConcurrentQueue<Type>();
        public async Task Prepend(Type value)
        {
            PrependQueue.Enqueue(value);
            lock (PrependLockObject)
            {
                if (WorkPrependQueue_Task == null || WorkPrependQueue_Task.IsCompleted)
                {
                    WorkPrependQueue_Task = WorkAppendQueue();
                }
            }
        }
        public async Task PrependRange(Type[] valueArray)
        {
            lock (PrependLockObject)
            {
                foreach (Type value in valueArray)
                {
                    PrependQueue.Enqueue(value);
                }
                if (WorkPrependQueue_Task == null || WorkPrependQueue_Task.IsCompleted)
                {
                    WorkPrependQueue_Task = WorkAppendQueue();
                }
            }
        }
        public object PrependLockObject = new object();
        public Task WorkPrependQueue_Task;
        private async Task WorkPrependQueue()
        {
            while (PrependQueue.Any())
            {
                Type element;
                bool success = PrependQueue.TryDequeue(out element);
                if (!success) continue;
                // prepend Data
                FirstBlock.Data[FirstBlock.FirstFreeElement] = element;
                FirstBlock.FirstFreeElement--;
                FirstBlock.Length++;
                Length++;
                // Block is full, move on to the next block and create a new one
                if (FirstBlock.FirstFreeElement < 0)
                {
                    if (AddBlockTask != null)
                    {
                        await AddBlockTask;
                    }
                    FirstBlock = FirstBlock.PreviousBlock;
                    AddBlockTask = AddBlock(wasLastBlockNotFirstBlock: false);
                }
            }
        }
    }
}
