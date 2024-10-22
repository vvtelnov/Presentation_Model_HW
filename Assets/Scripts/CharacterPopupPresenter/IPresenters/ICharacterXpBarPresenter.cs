using UniRx;

namespace CharacterPopupPresenter.IPresenters
{
    public interface ICharacterXpBarPresenter : IPresenter
    {
        public IReadOnlyReactiveProperty<string> XpFullStr { get; }

        public IReadOnlyReactiveProperty<bool> IsXpBarFull { get; }
        
        public IReadOnlyReactiveProperty<uint> CurrentXp { get; }
        public IReadOnlyReactiveProperty<uint> MaxBarXp { get; }

    }
}