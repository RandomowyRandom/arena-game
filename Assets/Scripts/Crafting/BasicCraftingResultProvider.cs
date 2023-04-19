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
            return _result;
        }
    }
}