using MainGame;
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace MainMenu
{
    public class SkinBlock : MonoBehaviour
    {
        [SerializeField] private AnimatedSkin _skin;
        [SerializeField] private Button _button;
        public event Action<AnimatedSkin> ButtonClicked;

        public AnimatedSkin Skin => _skin;

        private void OnEnable()
        {
            _button.onClick.AddListener(Click);
        }

        public void Click()
        {
            ButtonClicked?.Invoke(_skin);
        }
    }
}
