using System;
using System.Collections.Generic;
using System.Linq;
using Items;
using Sirenix.Serialization;

namespace EntitySystem.DropTable
{
    [Serializable]
    public class AdvancedDropTable: IDropTable
    {
        [OdinSerialize]
        private List<List<Item>> _guaranteedDrops = new();
        
        [OdinSerialize]
        private List<BasicItemDrop> _chanceDrops = new();
        
        public List<Item> GetDrops()
        {
            var drops = new List<Item>();
            
            foreach (var guaranteedDrop in _guaranteedDrops)
            {
                var randomIndex = UnityEngine.Random.Range(0, guaranteedDrop.Count);
                drops.Add(guaranteedDrop[randomIndex]);
            }
            
            var randomDrops = (from drop in _chanceDrops
                where drop.Chance >= UnityEngine.Random.value
                select drop.Item).ToList();

            drops.AddRange(randomDrops);

            return drops;
        }
    }
}