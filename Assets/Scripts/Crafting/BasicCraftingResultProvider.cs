using System;
using Items;
using Sirenix.Serialization;

namespace Crafting
{
    [Serializable]
    public class BasicCraftingResultProvider: ICraftingResultProvider
    {
        [OdinSerialize]
        private Item _result;
        
        public Item GetResult()
        {
            _result = new(_result);
            
            return _result;
        }
        
        public void SetResult(Item result)
        {
            _result = result;
        }
    }
}