using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using FickleFrames;
using UnityEditor.Animations;
using System;

[CustomEditor(typeof(AnimationController))]
public class Editor_AnimationController : Editor
{
    #region Attributes

    // Static attributes
    static AnimationController controller = null;
    static GameObject gameObject = null;
    static Animator animator = null;
    static AnimatorController animatorController = null;
    static bool canEditController = false;
    static string animationClipsSourcePath = "Assets/Animations/Clips/";
    static string animatorControllerSavePath = "Assets/Animations/Controllers/";

    static List<string> popUps = new List<string>();
    static Dictionary<string, AnimationClip> clips = new Dictionary<string, AnimationClip>();

    // Other attributes
    int index = 0;
    string currentSelection = "";

    #endregion


    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        propertyUpdate();
        serializedObject.ApplyModifiedProperties();
    }


    /// <summary>
    /// Runs all property updates
    /// </summary>
    private void propertyUpdate()
    {
        loadValues();

        // Create animator
        property_createAnimator();

        // Enable auto update property
        property_autoUpdateAnimation();

        // Animation clip file path property
        property_animationClips();

        // Animation controller path property
        property_animationController();
    }


    private void loadValues()
    {
        if (controller == null)
            controller = (AnimationController)target;
        if (gameObject == null)
            gameObject = controller.gameObject;
    }


    private void property_createAnimator()
    {
        if (animator == null)
            if (gameObject.TryGetComponent(out animator) == false)
            {
                GUILayout.BeginHorizontal();

                EditorGUILayout.HelpBox("MISSING COMPONENT! : ANIMATOR", MessageType.Error);

                if (GUILayout.Button("Attach Animator?", GUILayout.Height(37.5f)))
                {
                    serializedObject.FindProperty("animator").objectReferenceValue = gameObject.AddComponent<Animator>();
                    serializedObject.FindProperty("canUseAnimator").boolValue = true;
                    animator = (Animator)serializedObject.FindProperty("animator").objectReferenceValue;
                }

                GUILayout.EndHorizontal();
            }
    }


    private void property_autoUpdateAnimation()
    {
        if (gameObject.GetComponent<StateControllerComponent>() == null)
        {
            serializedObject.FindProperty("enableAutoUpdate").boolValue = false;
            return;
        }
        bool enableAutoUpdate = serializedObject.FindProperty("enableAutoUpdate").boolValue;
        serializedObject.FindProperty("enableAutoUpdate").boolValue = EditorGUILayout.Toggle
            (
                new GUIContent
                    (
                     "Enable Auto Update",
                     "Set this as true if animation automatically should change automatically if " +
                     "there's a state controller component attached to this gameObject"
                     ),
                enableAutoUpdate
            );
    }


    private void property_animationClips()
    {
        animationClipsSourcePath = EditorGUILayout.TextField
            (
                new GUIContent
                    (
                     "Animation Clips Filepath",
                     "Filepath from where animation clips for this gameObject should be loaded"
                     ),
                animationClipsSourcePath
            );

        if (!Directory.Exists(animationClipsSourcePath))
        {
            if (GUILayout.Button($"Create directory : {animationClipsSourcePath}", GUILayout.Height(25)))
                Directory.CreateDirectory(animationClipsSourcePath);
        }
        else
        {
            if (GUILayout.Button("Refresh clip database", GUILayout.Height(20)))
            {
                clips.Clear();
                popUps.Clear();
                popUps.Add("None");

                foreach (string path in Directory.EnumerateFiles(animationClipsSourcePath, "*.anim"))
                {
                    AnimationClip clip = AssetDatabase.LoadAssetAtPath<AnimationClip>(path);
                    if (clip != null)
                    {
                        popUps.Add(clip.name);
                        clips.Add(clip.name, clip);
                    }
                }
            }
        }
    }


    private void property_animationController()
    {
        animatorControllerSavePath = EditorGUILayout.TextField
            (
                new GUIContent
                    (
                     "Animator Controller Filepath",
                     "This is the target filepath for new or existing animator controller"
                     ),
                animatorControllerSavePath
            );


        if (!Directory.Exists(animatorControllerSavePath))
        {
            if (GUILayout.Button($"Create directory : {animatorControllerSavePath}", GUILayout.Height(25)))
            {
                Directory.CreateDirectory(animatorControllerSavePath);
                animatorController = AnimatorController.CreateAnimatorControllerAtPath(animatorControllerSavePath + $"/{gameObject.name}.controller");
            }
        }
        else
        {
            animatorController = AssetDatabase.LoadAssetAtPath<AnimatorController>(animatorControllerSavePath + $"/{gameObject.name}.controller");
            if (animatorController == null)
                if (GUILayout.Button("Create animator controller at path"))
                    animatorController = AnimatorController.CreateAnimatorControllerAtPath(animatorControllerSavePath + $"/{gameObject.name}.controller");
        }

        if (animator != null)
            animator.runtimeAnimatorController = animatorController;

        canEditController = EditorGUILayout.Toggle
            (
                new GUIContent("Show controller settings?"),
                canEditController
            );

        if (canEditController && animatorController != null)
        {
            Dictionary<string, AnimatorState> states = new Dictionary<string, AnimatorState>();

            var root = animatorController.layers[0].stateMachine;

            // Create popup
            GUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("Select default state for animator controller");
            index = EditorGUILayout.Popup(index, popUps.ToArray());

            GUILayout.EndHorizontal();

            // Create button to update controller
            if (GUILayout.Button("Update Animator Controller States"))
            {
                // Create dictionary for easy lookup
                foreach (var state in root.states)
                    states.Add(state.state.name, state.state);

                // Add states if required
                for (int i = 0; i < popUps.Count; ++i)
                    if (!states.ContainsKey(popUps[i]))
                        states.Add(popUps[i], root.AddState(popUps[i]));

                // Discard states which are not included
                foreach (var state in states)
                    if (!popUps.Contains(state.Key))
                        root.RemoveState(state.Value);

                // Update the default state
                if (currentSelection != popUps[index])
                {
                    currentSelection = popUps[index];
                    if (currentSelection != "None")
                        root.RemoveState(states["None"]);
                    root.defaultState = states[currentSelection];
                }
            }

            if (GUILayout.Button("Update motion for states"))
            {
                foreach (var state in root.states)
                    if (state.state.motion == null)
                        if (clips.ContainsKey(state.state.name))
                            state.state.motion = clips[state.state.name];
            }
        }
    }
}
