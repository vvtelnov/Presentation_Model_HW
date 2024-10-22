using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.CharacterPopupView
{
    // Я эту вьюшку не наследовал от AbstractPopupView потому что она не принимает в себя никакого презентора.
    // а объединять ее с какой-то другой вьюшкой не хочу, что бы было разграничение ответственности.
    public class CharacterPopupView : MonoBehaviour, IPopupView
    {
        public event Action OnClose;
        [SerializeField] private Button _closeBtn;

        public void Open()
        {
            _closeBtn.onClick.AddListener(Close);
        }

        public void Close()
        {
            _closeBtn.onClick.RemoveListener(Close);
            OnClose?.Invoke();
            gameObject.SetActive(false);
        }
    }
}