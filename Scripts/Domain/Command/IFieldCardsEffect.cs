namespace Unity1week202112.Domain.Command
{
    /// <summary>
    /// フィールドカードの効果
    /// </summary>
    public interface IFieldCardsEffect : ICommandEffect
    {
        void Execute(FieldCardsModel myFieldCards, FieldCardsModel otherFieldCards);
    }
}
