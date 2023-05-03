using System;
using System.Collections.Generic;
using System.Linq;
using Items;
using Sirenix.Serialization;

namespace EntitySystem.DropTable
{
    [Serializable]
    public class BasicDropTable: IDropTable
    {
        [OdinSerialize]
        private List<BasicItemDrop> _drops = new();
        
        public List<Item> GetDrops()
        {
            return (from drop in _drops 
                where drop.Chance >= UnityEngine.Random.value 
                select drop.Item)
                .ToList();
        }
    }
}