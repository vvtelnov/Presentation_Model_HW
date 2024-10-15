using System;
using CharacterPopupPresenter;

namespace PopupView
{
    public interface IPopupView
    {
        public void Open(ICharacterPopupPresenter args);
        public void Close();
    }

    public interface IPopupEventEmitter
    {
        public event Action OnClose;
        public event Action OnActionButtonClick;
    }
}