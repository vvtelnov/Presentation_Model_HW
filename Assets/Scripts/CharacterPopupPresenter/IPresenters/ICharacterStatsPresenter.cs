using UniRx;

namespace CharacterPopupPresenter.IPresenters
{
    public interface ICharacterStatsPresenter : IPresenter
    {
        public IReadOnlyReactiveCollection<StringReactiveProperty> Stats { get; }

    }
}