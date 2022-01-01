using Cysharp.Threading.Tasks;

namespace Unity1week202112.Domain.Command
{
    public interface IAttackEffect : ICommandEffect
    {
        UniTask Execute(PlayerStatusModel attacker, PlayerStatusModel enemy);
    }
}
