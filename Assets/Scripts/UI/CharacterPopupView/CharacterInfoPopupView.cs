using CharacterPopupPresenter.IPresenters;
using Sirenix.OdinInspector;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UI.CharacterPopupView
{
    public class CharacterInfoPopupView : BasePopupView<ICharacterInfoPresenter>
    {
        [Title("Character Fields")]
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _description;
        [SerializeField] private Image _profilePicture;

        protected override void Show(ICharacterInfoPresenter args)
        {
            args.Name.SkipLatestValueOnSubscribe().Subscribe(OnNameChanged).AddTo(disposable);
            args.Description.SkipLatestValueOnSubscribe().Subscribe(OnDescriptionChanged).AddTo(disposable);
            args.Icon.SkipLatestValueOnSubscribe().Subscribe(OnProfilePictureChanged).AddTo(disposable);

            _name.text = args.Name.Value;
            _description.text = args.Description.Value;
            _profilePicture.sprite = args.Icon.Value;
        }
        
        private void OnNameChanged(string newName)
        {
            _name.text = newName;
        }

        private void OnDescriptionChanged(string newDescription)
        {
            _description.text = newDescription;
        }
        
        private void OnProfilePictureChanged(Sprite newImage)
        {
            _profilePicture.sprite = newImage;
        }
    }
}