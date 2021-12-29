using UnityEngine;
using UnityEditor;
using FFG.Systems;
using FFG.Systems.Internal;
using UnityEditor.Animations;


[CustomEditor(typeof(FFG_Button))]
public class FFG_ButtonEditor : BaseEditor<FFG_Button>
{
    Animator cachedAnimator;
    AnimatorController cachedController;


    public override void InspectorUpdate()
    {
        SerializedProperty spriteRenderer = GetProperty("_spriteRenderer");
        SerializedProperty data = GetProperty("_data");
        SerializedProperty animator = GetProperty("_buttonAnimator");
        SerializedProperty unityEvent = data.FindPropertyRelative("UnityEvent");
        SerializedProperty buttonTransitions = data.FindPropertyRelative("ButtonTransitions");
        SerializedProperty spriteData = data.FindPropertyRelative("SpriteData");
        SerializedProperty colorData = data.FindPropertyRelative("ColorData");
        SerializedProperty animationData = data.FindPropertyRelative("AnimationData");

        Heading("Manual setups");

        Info(
                "1. Make sure physics 2D raycaster is added to the camera\n" +
                "2. Make sure event system is added in the scene\n" + 
                "3. In the case of animation update then animator controller should be setup like the following :\n" +
                "   a. Create \'ButtonUp\' state when button is released\n" +
                "   b. Create \'ButtonDown\' state when button is pressed\n" +
                "   c. Create \'ButtonEnter\' state when pointer enters the button\n" +
                "   d. Create \'ButtonExit\' state when pointer leaves the button\n" +
                "   e. Logically set the \'ButtonUp\' state or \'ButtonExit\' state as the entry point for the Animator Controller"
            );

        Heading("Button Settings");

        if (Root.GetComponent<Collider2D>() == null)
        {
            Space(5);
            Info("Collider missing!! Please add a 2D colldier!!", MessageType.Error);
        }

        Space(5);

        PropertyField(spriteRenderer, "Target Button Sprite Renderer : ", "");
        if (spriteRenderer.objectReferenceValue == null)
        {
            Space(4);
            Info("This value cannot be null for this component to work properly!!", MessageType.Error);
        }

        Space(8);

        PropertyField(unityEvent, "On Button Click Event : ", "");

        Space(8);

        PropertyField(buttonTransitions, "Button Transitions : ", "Type of button transtion");

        Space(6);

        EButtonTransitions transition = (EButtonTransitions)buttonTransitions.enumValueIndex;

        switch (transition)
        {
            case EButtonTransitions.SpriteUpdate:
                {
                    PropertyField(spriteData.FindPropertyRelative("ButtonUp"), "Button Up Sprite : ", "");
                    PropertyField(spriteData.FindPropertyRelative("ButtonDown"), "Button Down Sprite : ", "");
                    PropertyField(spriteData.FindPropertyRelative("ButtonEnter"), "Button Enter Sprite : ", "");
                    PropertyField(spriteData.FindPropertyRelative("ButtonExit"), "Button Exit Sprite : ", "");
                }
                break;
            case EButtonTransitions.ColorUpdate:
                {
                    PropertyField(colorData.FindPropertyRelative("ButtonUp"), "Button Up Color : ", "");
                    PropertyField(colorData.FindPropertyRelative("ButtonDown"), "Button Down Color : ", "");
                    PropertyField(colorData.FindPropertyRelative("ButtonEnter"), "Button Enter Color : ", "");
                    PropertyField(colorData.FindPropertyRelative("ButtonExit"), "Button Exit Color : ", "");
                }
                break;
            case EButtonTransitions.AnimationUpdate:
                {
                    if (Root.GetComponent<Animator>() == null)
                    {
                        animator.objectReferenceValue = Root.AddComponent<Animator>();
                        Debug.Log("Adding animator to this object");
                    }

                    PropertyField(animator, "Target Animator : ", "");
                    Space(2);
                    PropertyField(animationData.FindPropertyRelative("ButtonUp"), "Button Up Animation : ", "");
                    PropertyField(animationData.FindPropertyRelative("ButtonDown"), "Button Down Animation : ", "");
                    PropertyField(animationData.FindPropertyRelative("ButtonEnter"), "Button Enter Animation : ", "");
                    PropertyField(animationData.FindPropertyRelative("ButtonExit"), "Button Exit Animation : ", "");
                }
                break;
        }
    }
}
