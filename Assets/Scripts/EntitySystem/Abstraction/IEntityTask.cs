using Cysharp.Threading.Tasks;

namespace EntitySystem.Abstraction
{
    public interface IEntityTask
    {
        public UniTask<EnemyTaskResult> Execute(EntityBehaviour entity);
    }
}