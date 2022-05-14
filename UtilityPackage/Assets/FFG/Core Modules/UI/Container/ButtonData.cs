using System;
using UnityEngine;
using UnityEngine.Events;


namespace FFG.UI.Internal
{
    [Serializable]
    public class ButtonData
    {
        public UnityEvent ButtonClickEvent;
        public UnityEvent ButtonUpEvent;
        public UnityEvent ButtonDownEvent;
        public UnityEvent ButtonEnterEvent;
        public UnityEvent ButtonExitEvent;
        public EButtonTransitions ButtonTransitions;
        public ButtonSpriteData SpriteData;
        public ButtonColorData ColorData;
        public ButtonAnimationData AnimationData;
    }

    [Serializable]
    public struct ButtonSpriteData
    {
        public Sprite ButtonUp;
        public Sprite ButtonDown;
        public Sprite ButtonEnter;
        public Sprite ButtonExit;
    }

    [Serializable]
    public struct ButtonColorData
    {
        public Color ButtonUp;
        public Color ButtonDown;
        public Color ButtonEnter;
        public Color ButtonExit;
    }

    [Serializable]
    public struct ButtonAnimationData
    {
        public AnimationClip ButtonUp;
        public AnimationClip ButtonDown;
        public AnimationClip ButtonEnter;
        public AnimationClip ButtonExit;
    }
}
