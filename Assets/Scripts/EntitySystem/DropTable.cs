using System;
using System.Collections.Generic;
using System.Linq;
using Items;
using Sirenix.Serialization;

namespace EntitySystem
{
    [Serializable]
    public class DropTable
    {
        [OdinSerialize]
        private List<ItemDrop> _drops = new();
        
        public List<Item> GetDrops()
        {
            return (from drop in _drops 
                where drop.Chance >= UnityEngine.Random.value 
                select drop.Item)
                .ToList();
        }
    }
}