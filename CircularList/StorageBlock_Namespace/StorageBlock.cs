using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CircularList.StorageBlock_Namespace
{
    /// <summary>
    /// a storage block is used in order to store the Circular Lists Elements
    /// </summary>
    /// <typeparam name="Type"></typeparam>
    internal class StorageBlock<Type>
    {
        public Type[] Data = new Type[64];
        public bool[] MarkedForDeletion = new bool[64];
        public int Length = 0;
        public int DeletedElementsCount = 0;
        public int LastFreeElement = 0;
        public int FirstFreeElement = 0;
        public StorageBlock<Type> NextBlock;
        public StorageBlock<Type> PreviousBlock;
        
    }
    struct BlockIndex<Type>
    {
        public StorageBlock<Type> Block;
        public int Index;
    }
}
