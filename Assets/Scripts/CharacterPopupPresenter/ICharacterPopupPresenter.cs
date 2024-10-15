using Player;
using PopupView;
using ScriptableObjects;
using UniRx;
using UnityEngine;

namespace CharacterPopupPresenter
{
    public interface ICharacterPopupPresenter
    {
        public IReadOnlyReactiveProperty<string> Name { get; }
        public IReadOnlyReactiveProperty<string> Description { get; }
        public IReadOnlyReactiveProperty<Sprite> Icon { get; }
        
        public IReadOnlyReactiveProperty<string> CurrentLevel { get; }
        public IReadOnlyReactiveProperty<string> XpFullStr { get; }

        public IReadOnlyReactiveProperty<bool> IsXpBarFull { get; }
        public IReadOnlyReactiveProperty<bool> CanLevelUp { get; }
        
        public IReadOnlyReactiveProperty<uint> CurrentXp { get; }
        public IReadOnlyReactiveProperty<uint> MaxBarXp { get; }

        public ReactiveDictionary<Stats, StringReactiveProperty> CharacterStatsMapToReactiveProperties { get; }
        
        public CharacterPopupElements PopupElements { get; }

    }

    public interface IEventsubscriberPresenter
    {
        public void SubscribeToViewEvents(IPopupEventEmitter viewEventEmitter);
    }
}