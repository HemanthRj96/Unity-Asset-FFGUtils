using System;
using UnityEngine;
using UnityEngine.Events;


namespace FFG.Systems.Internal
{
    [Serializable]
    public class FFG_ButtonData
    {
        public UnityEvent UnityEvent;
        public EButtonTransitions ButtonTransitions;
        public FFG_ButtonSpriteData SpriteData;
        public FFG_ButtonColorData ColorData;
        public FFG_ButtonAnimationData AnimationData;
    }

    [Serializable]
    public struct FFG_ButtonSpriteData
    {
        public Sprite ButtonUp;
        public Sprite ButtonDown;
        public Sprite ButtonEnter;
        public Sprite ButtonExit;
    }

    [Serializable]
    public struct FFG_ButtonColorData
    {
        public Color ButtonUp;
        public Color ButtonDown;
        public Color ButtonEnter;
        public Color ButtonExit;
    }

    [Serializable]
    public struct FFG_ButtonAnimationData
    {
        public AnimationClip ButtonUp;
        public AnimationClip ButtonDown;
        public AnimationClip ButtonEnter;
        public AnimationClip ButtonExit;
    }
}
