using Cysharp.Threading.Tasks;
using Items.ItemDataSystem;

namespace Items.Abstraction
{
    public interface IItemEffect
    {
        UniTask OnUse(IItemUser user, UsableItem item);
    }
}