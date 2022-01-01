
namespace Unity1week202112.Data
{
    /// <summary>
    /// ユーザーデータ
    /// </summary>
    public class UserData
    {
        /// <summary>
        /// チュートリアルを見たか
        /// </summary>
        public bool ShownTutorial { get; set; } = false;

        /// <summary>
        /// MaxHp
        /// </summary>
        public int Hp { get; set; } = 10;
    }
}
