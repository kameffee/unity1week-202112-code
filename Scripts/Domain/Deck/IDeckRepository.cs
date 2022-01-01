namespace Unity1week202112.Domain.Deck
{
    /// <summary>
    /// デッキリポジトリ
    /// </summary>
    public interface IDeckRepository
    {
        CommandDeckEntity Load();
    }
}
