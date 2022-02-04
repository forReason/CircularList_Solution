using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CircularList.StorageBlock_Namespace
{
    internal partial class StorageBlockManager<Type>
    {
        public StorageBlockManager()
        {
            // initialize last and first open Block
            StorageBlock<Type> root = new StorageBlock<Type>();
            LastBlock = root;
            FirstBlock = root;
            root.FirstFreeElement = root.Data.Length / 2;
            root.LastFreeElement = root.Data.Length / 2 +1;
            // Create Spare Block for appending
            StorageBlock<Type> spareLastBlock = new StorageBlock<Type>();
            LastBlock.NextBlock = spareLastBlock;
            spareLastBlock.PreviousBlock = LastBlock;
            // Create Spare Block for prepending
            StorageBlock<Type> spareFirstBlock = new StorageBlock<Type>();
            FirstBlock.PreviousBlock = spareFirstBlock;
            spareFirstBlock.NextBlock = FirstBlock;
            // Link spare Blocks
            spareFirstBlock.PreviousBlock = spareLastBlock;
            spareLastBlock.NextBlock = spareFirstBlock;
            // Initialize indexes
            this.StartIndex = FirstBlock.Data.Length / 2;

        }
        // used for appending Data
        StorageBlock<Type> LastBlock;
        // used for prepending Data
        StorageBlock<Type> FirstBlock;
        //RootBlock.NextBlock
        public int Length = 0;
        int StartIndex = 0;
        Task AddBlockTask;
        async Task AddBlock(bool wasLastBlockNotFirstBlock)
        {
            // create new spare Block to insert
            StorageBlock<Type> newSpareBlock = new StorageBlock<Type>();
            // determine where to link the block
            if (wasLastBlockNotFirstBlock) // appending
            {
                // the Last block has been taken
                // insert new spare block between last block and last spare block
                StorageBlock<Type> lastSpareBlock = LastBlock.NextBlock;
                // link last spare block (prepending) to  newly created spare block
                lastSpareBlock.PreviousBlock = newSpareBlock;
                newSpareBlock.NextBlock = lastSpareBlock;
                // link current last block to new spare block
                LastBlock.NextBlock = newSpareBlock;
                newSpareBlock.PreviousBlock = LastBlock;
                { }
            }
            else // prepending
            {
                // the first Block has been taken
                StorageBlock<Type> lastSpareBlock = FirstBlock.PreviousBlock;
                // insert new spare block between first block and last spare block
                lastSpareBlock.NextBlock = newSpareBlock;
                newSpareBlock.PreviousBlock = lastSpareBlock;

                FirstBlock.PreviousBlock = newSpareBlock;
                newSpareBlock.NextBlock = FirstBlock;
            }
        }
        internal Type[] StorageBlockToArray(StorageBlock<Type> block, int startIndex = 0, int endIndex = -1)
        {
            if (endIndex == -1) endIndex = block.Length;
            Type[] result = new Type[endIndex-StartIndex-1];
            int index = 0;
            for (int i = 0; i < block.MarkedForDeletion.Length; i++)
            {
                if (block.MarkedForDeletion[i]) continue;
                if (index >= startIndex) result[index] = block.Data[i];
                index++;
                if (index > endIndex) return result;
            }
            return result;
        }
    }
}
