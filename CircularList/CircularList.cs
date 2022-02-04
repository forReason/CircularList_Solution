using CircularList.StorageBlock_Namespace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CircularList
{
    public class CircularList<Type>
    {
        StorageBlockManager<Type> StorageBlockManager = new StorageBlockManager<Type>();
        public async void AppendAsync(Type value)
        {
            StorageBlockManager.Append(value);
        }
        public void Append(Type value)
        {
            StorageBlockManager.Append(value).Wait();
        }
        public async void PrependAsync(Type value)
        {
            StorageBlockManager.Prepend(value);
        }
        public void Prepend(Type value)
        {
            StorageBlockManager.Prepend(value).Wait();
        }
        public Type[] ToArray()
        {
            Type[] taskResult = StorageBlockManager.GetRange(0, StorageBlockManager.Length).Result;
            return taskResult;
        }
        public Type GetElementAt(int index)
        {
            return StorageBlockManager.GetElementAt(index).Result;
        }
    }
}
