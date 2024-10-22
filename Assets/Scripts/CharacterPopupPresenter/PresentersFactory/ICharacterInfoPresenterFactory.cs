using System;
using System.Collections.Generic;
using CharacterPopupPresenter.IPresenters;

namespace CharacterPopupPresenter.PresentersFactory
{
    public interface ICharacterPresenterFactory
    {
        public List<Type> СanCreatePresenters { get; }
        public IPresenter CreatePresenter(Type presenterType);
    }
}