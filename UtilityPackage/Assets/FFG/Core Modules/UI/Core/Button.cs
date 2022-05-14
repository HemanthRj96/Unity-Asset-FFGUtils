using FFG.UI.Internal;
using UnityEngine;
using UnityEngine.EventSystems;


namespace FFG.UI
{
    public sealed class Button :
        MonoBehaviour,
        IPointerClickHandler,
        IPointerEnterHandler,
        IPointerExitHandler,
        IPointerUpHandler,
        IPointerDownHandler
    {
        [SerializeField]
        private SpriteRenderer _spriteRenderer;
        [SerializeField]
        private ButtonData _data;
        [SerializeField]
        private Animator _buttonAnimator;

        private bool _canUse => _spriteRenderer;


        public void Awake()
        {
            if (GameObject.Find("EventSystem") == null)
                new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!_canUse)
                return;
            _data.ButtonClickEvent.Invoke();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!_canUse)
                return;
            switch (_data.ButtonTransitions)
            {
                case EButtonTransitions.SpriteUpdate:
                    _spriteRenderer.sprite = _data.SpriteData.ButtonDown;
                    break;
                case EButtonTransitions.ColorUpdate:
                    _spriteRenderer.color = _data.ColorData.ButtonDown;
                    break;
                case EButtonTransitions.AnimationUpdate:
                    if (_buttonAnimator.runtimeAnimatorController != null)
                        _buttonAnimator.Play("ButtonDown");
                    break;
            }
            _data.ButtonDownEvent.Invoke();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (!_canUse)
                return;
            switch (_data.ButtonTransitions)
            {
                case EButtonTransitions.SpriteUpdate:
                    _spriteRenderer.sprite = _data.SpriteData.ButtonUp;
                    break;
                case EButtonTransitions.ColorUpdate:
                    _spriteRenderer.color = _data.ColorData.ButtonUp;
                    break;
                case EButtonTransitions.AnimationUpdate:
                    if (_buttonAnimator.runtimeAnimatorController != null)
                        _buttonAnimator.Play("ButtonUp");
                    break;
            }
            _data.ButtonUpEvent.Invoke();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!_canUse)
                return;
            switch (_data.ButtonTransitions)
            {
                case EButtonTransitions.SpriteUpdate:
                    _spriteRenderer.sprite = _data.SpriteData.ButtonEnter;
                    break;
                case EButtonTransitions.ColorUpdate:
                    _spriteRenderer.color = _data.ColorData.ButtonEnter;
                    break;
                case EButtonTransitions.AnimationUpdate:
                    if (_buttonAnimator.runtimeAnimatorController != null)
                        _buttonAnimator.Play("ButtonEnter");
                    break;
            }
            _data.ButtonEnterEvent.Invoke();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!_canUse)
                return;
            switch (_data.ButtonTransitions)
            {
                case EButtonTransitions.SpriteUpdate:
                    _spriteRenderer.sprite = _data.SpriteData.ButtonExit;
                    break;
                case EButtonTransitions.ColorUpdate:
                    _spriteRenderer.color = _data.ColorData.ButtonExit;
                    break;
                case EButtonTransitions.AnimationUpdate:
                    if (_buttonAnimator.runtimeAnimatorController != null)
                        _buttonAnimator.Play("ButtonExit");
                    break;
            }
            _data.ButtonExitEvent.Invoke();
        }
    }
}
