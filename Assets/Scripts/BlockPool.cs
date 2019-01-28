using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockPool {
    Block originalBlock;
    List<Block> allBlock;

    public BlockPool(Block originalBlock, int size)
    {
        this.originalBlock = originalBlock;
        originalBlock.enabled = false;
        originalBlock.gameObject.active = false;

        allBlock = new List<Block>();
        for (int i = 0; i < size; i++)
        {
            Block block = CreateBlock();
            block.Init();
            allBlock.Add(block);
        }
    }
    private Block CreateBlock()
    {
        Block block = GameObject.Instantiate<Block>(originalBlock, Vector3.zero, Quaternion.identity);
        block.SetInactive();
        block.enabled = false;
        block.gameObject.active = false;
        return block;
    }
    public Block GetBlock()
    {
        Block newBlock = null;
        for (int i = 0; i < allBlock.Count; i++)
        {
            if (!allBlock[i].IsActive())
            {
                newBlock = allBlock[i];
                break;
            }
        }
        if (newBlock == null)
        {
            newBlock = CreateBlock();
        }
        newBlock.OnBlockListener.RemoveAllListeners();
        newBlock.OnGroundListener.RemoveAllListeners();
        
        return newBlock;
    }



    public void ReturnBlock(Block aBlock)
    {
        aBlock.SetInactive();
        ///aBlock.enabled = false;
    }

}
