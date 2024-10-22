using UniRx;

namespace CharacterPopupPresenter.IPresenters
{
    public interface ICharacterLvlPresenter : IPresenter
    {
        public IReadOnlyReactiveProperty<string> CurrentLevel { get; }
    }
}