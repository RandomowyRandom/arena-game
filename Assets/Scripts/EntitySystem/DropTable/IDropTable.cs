using System.Collections.Generic;
using Items;

namespace EntitySystem.DropTable
{
    public interface IDropTable
    {
        public List<Item> GetDrops();
    }
}