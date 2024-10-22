using System;
using System.Collections.Generic;
using CharacterPopupPresenter.IPresenters;
using UI.CharacterPopupView;
using UniRx;
using UnityEngine;

namespace UI
{
    public abstract class BasePopupView<TPresenter> : MonoBehaviour, IViews
    {
        protected CompositeDisposable disposable;
        
        public void OnShow(Dictionary<Type, IPresenter> presenters)
        {
            var type = typeof(TPresenter);
            
            if (!presenters.TryGetValue(type, out var presenter))
            {
                throw new ArgumentException($"Did not find presenter type {type}");
            }
            
            if (presenter is not TPresenter typedPresenter)
            {
                throw new ArgumentException($"Presenter is not of type {type}");
            }
            
            if (presenter is IViewEventsListener eventsListener)
            {
                SubscribePresenterToViewEvents(eventsListener);
            }

            disposable = new CompositeDisposable();

            Show(typedPresenter);
        }

        protected abstract void Show(TPresenter args);

        protected virtual void SubscribePresenterToViewEvents(IViewEventsListener presenter)
        {
            throw new Exception("SubscribePresenterToViewEvents method should be overwritten on inherit class");
        }
    }
}