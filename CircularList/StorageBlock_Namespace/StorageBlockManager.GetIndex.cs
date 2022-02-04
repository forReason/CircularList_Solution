using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CircularList.StorageBlock_Namespace
{
    internal partial class StorageBlockManager<Type>
    {
        internal BlockIndex<Type> ConvertIndex(int index)
        {
            // do prechecks
            if (index < 0) throw new ArgumentOutOfRangeException("index may not be smaller than 0! index:"+index);
            if (index > Length) throw new ArgumentOutOfRangeException("index is larger than the list length! index:"+index);
            // determine the rough position of the index in percentage
            BlockIndex<Type> returnIndex = new BlockIndex<Type>();
            float roughPosition = (float)index / Length;
            // determine fastest path, from start or from back of the list
            if (roughPosition <= 0.5)
            {
                // start from beginning
                StorageBlock<Type> currentBlock = FirstBlock;
                // move forwards through the CircularList
                while (index >= 0)
                {
                    // calculate how far we are from the seeked index
                    index -= currentBlock.Length;
                    // check if we have reached the target Block
                    if (index > 0)
                    {
                        // if have not reached the target block yet, grab the next block for inspection
                        currentBlock = currentBlock.NextBlock;
                    }
                    else
                    {
                        // this is the right block. Revert calculation!
                        index += currentBlock.Length;
                        // fetch the correct index (there might be holes due to deletion)
                        returnIndex.Index = GetBlockIndex(index, currentBlock);
                        returnIndex.Block = currentBlock;
                        return returnIndex;
                    }
                }
            }
            else
            {
                // start from back
                StorageBlock<Type> currentBlock = LastBlock;
                // move backwards through the CircularList
                while (index >= 0)
                {
                    // calculate how far we are from the seeked index
                    index -= currentBlock.Length;
                    // check if we have reached the target Block
                    if (index > 0)
                    {
                        // if have not reached the target block yet, grab the next block for inspection
                        currentBlock = currentBlock.PreviousBlock;
                    }
                    else
                    {
                        // this is the right block. Revert calculation!
                        index += currentBlock.Length;
                        // fetch the correct index (there might be holes due to deletion)
                        returnIndex.Index = GetBlockIndex(index, currentBlock);
                        returnIndex.Block= currentBlock;
                        return returnIndex;
                    }
                }
            }
            throw new IndexOutOfRangeException("for some unknown reason, the Index could not be resolved!");
        }
        /// <summary>
        /// this frunction is used to convert the correct index of an element in a block. 
        /// It is important, as elements might have been removed/deleted from the block and thus holes exist
        /// </summary>
        /// <param name="index">the nth index which should be converted to the index of the element in block</param>
        /// <param name="block">the block where the index should be found</param>
        /// <returns></returns>
        /// <exception cref="IndexOutOfRangeException"></exception>
        internal int GetBlockIndex(int index, StorageBlock<Type> block)
        {
            if (index > block.Length) throw new IndexOutOfRangeException("was larger than the data array of the Block!");
            // check if the block is full, then just return the index
            if (block.Length == block.MarkedForDeletion.Length) return index;
            // speedchecks did not find reuslt. There are deleted Elements. Look result up manually
            for (int i = block.FirstFreeElement+1; i < block.MarkedForDeletion.Length; i++)
            {
                if (!block.MarkedForDeletion[i])
                {
                    if (index <= 0) return i;
                    index--;
                }
                
            }
            throw new IndexOutOfRangeException("index could not be resolved!");
        }
    }
}
