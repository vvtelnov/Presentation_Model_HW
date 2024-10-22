using UI.CharacterPopupView;

namespace CharacterPopupPresenter.IPresenters
{
    public interface IPresenter
    {
    }

    public interface IViewEventsListener : IPresenter
    {
        public void SubscribeToViewEvents(IViewEventEmmiter view);
    }
}