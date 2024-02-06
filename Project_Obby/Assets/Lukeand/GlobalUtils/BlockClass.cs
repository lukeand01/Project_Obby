using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockClass
{
    #region BLOCKS

    //get blocks to stop movement.
    public Dictionary<string, BlockType> blockNary = new Dictionary<string, BlockType>();

    public void AddBlock(string id, BlockType block)
    {
        if (blockNary.ContainsKey(id))
        {
            //if we already have that key then we dont.
            return;
        }

        

        blockNary.Add(id, block);
    }

    


    public bool HasBlock(BlockType block)
    {
        return blockNary.ContainsValue(block);
    }

    public bool HasBlockID(string id)
    {
        return blockNary.ContainsKey(id);
    }

    public void RemoveBlock(string id)
    {
        if (blockNary.ContainsKey(id))
        {
            blockNary.Remove(id);
        }



    }

    public void ClearBlock()
    {
        blockNary.Clear();
    }

    public enum BlockType
    {
        Complete,        
        Partial,
        Movement,
        MouseSkill,
        KeySkill,
        Silence


    }

    #endregion
}
