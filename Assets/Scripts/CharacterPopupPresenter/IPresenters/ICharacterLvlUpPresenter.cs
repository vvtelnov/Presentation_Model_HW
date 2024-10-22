using UniRx;

namespace CharacterPopupPresenter.IPresenters
{
    public interface ICharacterLvlUpPresenter : IPresenter
    {
        public IReadOnlyReactiveProperty<bool> CanLevelUp { get; }
    }
}