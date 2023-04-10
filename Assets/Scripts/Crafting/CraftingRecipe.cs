using System.Collections.Generic;
using Common.Attributes;
using Items;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

namespace Crafting
{
    [ScriptableFactoryElement]
    public class CraftingRecipe: SerializedScriptableObject
    {
        [OdinSerialize]
        private List<Item> _ingredients;
        
        [OdinSerialize]
        private Item _result;
        
        public List<Item> Ingredients => _ingredients;
        public Item Result => _result;
    }
}