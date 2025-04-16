using Cysharp.Threading.Tasks;

namespace Vampwolf.Battles.Commands
{
    public interface IBattleCommand
    {
        UniTask Execute();
    }
}
