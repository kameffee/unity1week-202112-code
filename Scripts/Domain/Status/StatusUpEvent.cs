using System.Threading;
using Cysharp.Threading.Tasks;
using Unity1week202112.Domain.User;

namespace Unity1week202112.Domain
{
    /// <summary>
    /// ステータスアップイベント
    /// </summary>
    public class StatusUpEvent
    {
        private readonly IStatusUpPerformPresenter _statusUpPerformPresenter;
        private readonly UserRepository _userRepository;

        public StatusUpEvent(
            IStatusUpPerformPresenter statusUpPerformPresenter,
            UserRepository userRepository)
        {
            _statusUpPerformPresenter = statusUpPerformPresenter;
            _userRepository = userRepository;
        }

        public async UniTask Execute(int eventId, CancellationToken cancellationToken)
        {
            // HP付与
            var userData = _userRepository.Load();
            var oldHp = userData.Hp;

            bool isWin = eventId / 1000 == 1;
            // Todo: 追加量をマスタから取得
            var addHp = isWin ? 5 : 2;
            userData.Hp += addHp;
            // 保存
            _userRepository.Save(userData);

            // 演出開始
            var statusChange = StatusChange.FromBeforeAndAddValue(oldHp, addHp);
            await _statusUpPerformPresenter.Perform(isWin, statusChange, cancellationToken);
        }
    }
}
