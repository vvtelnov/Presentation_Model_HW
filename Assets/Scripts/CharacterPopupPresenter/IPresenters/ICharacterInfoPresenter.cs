using UniRx;
using UnityEngine;

namespace CharacterPopupPresenter.IPresenters
{
    public interface ICharacterInfoPresenter : IPresenter
    {
        public IReadOnlyReactiveProperty<string> Name { get; }
        public IReadOnlyReactiveProperty<string> Description { get; }
        public IReadOnlyReactiveProperty<Sprite> Icon { get; }

    }
}