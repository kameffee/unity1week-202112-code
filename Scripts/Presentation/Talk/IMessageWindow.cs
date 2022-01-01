using Cysharp.Threading.Tasks;

namespace Unity1week202112.Presentation.Talk
{
    public interface IMessageWindow
    {
        UniTask Open();

        UniTask Close();

        UniTask StartMessage(string message);

        void SetMessage(string message);
    }
}
