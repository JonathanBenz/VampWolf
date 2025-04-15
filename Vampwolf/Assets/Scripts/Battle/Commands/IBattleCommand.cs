using Cysharp.Threading.Tasks;

namespace Vampwolf.Battle.Commands
{
    public interface IBattleCommand
    {
        UniTask Execute();
    }
}
