using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CircularList.StorageBlock_Namespace
{
    internal partial class StorageBlockManager<Type>
    {
        internal async Task<Type> GetElementAt(int index)
        {
            // determine index of the element
            BlockIndex<Type> blockIndex = ConvertIndex(index);
            return blockIndex.Block.Data[blockIndex.Index];
        }
        internal async Task<Type[]> GetRange(int startIndex, int length)
        {
            Type[] result = new Type[length];
            BlockIndex<Type> startBlockIndex = ConvertIndex(startIndex);
            StorageBlock<Type> currentBlock = startBlockIndex.Block;
            int blockIndex = startBlockIndex.Index;
            // starting index was found. Build array
            for (int b = 0; b < length; b++)
            {
                bool elementMarkedForDeletion = true;
                while (elementMarkedForDeletion)
                {
                    // check if we need to move to the next block
                    if (blockIndex >= currentBlock.MarkedForDeletion.Length)
                    {
                        blockIndex = 0;
                        currentBlock = currentBlock.NextBlock;
                    }
                    // check, if we should copy the value
                    if (!currentBlock.MarkedForDeletion[blockIndex])
                    {
                        result[b] = currentBlock.Data[blockIndex];
                        elementMarkedForDeletion = false;
                    }
                    blockIndex++;
                }
            }
            return result;
        }
    }
}
