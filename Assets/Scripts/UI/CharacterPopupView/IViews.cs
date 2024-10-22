using System;
using System.Collections.Generic;
using CharacterPopupPresenter.IPresenters;

namespace UI.CharacterPopupView
{
    public interface IPopupView
    {
        public event Action OnClose;

        public void Open();
        public void Close();
    }
    
    public interface IViews
    {
        public void OnShow(Dictionary<Type, IPresenter> presenters);
    }

    
    public interface IViewHideable
    {
        public void OnHide();
    }

    public interface IViewEventEmmiter
    {
        
    }
    
    public interface ICharacterLvlUpView
    {
        public event Action OnLvlUpClicked;
    }
}