using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using FickleFrames;
using UnityEditor.Animations;


[CustomEditor(typeof(AnimationController))]
public class Editor_AnimationController : Editor
{
    #region Attributes

    // Cached attributes
    GameObject gameObject = null;
    AnimationController controller = null;
    AnimatorController animatorController = null;

    // Serialized attributes
    Animator animator = null;
    Dictionary<string, AnimationClip> clips = new Dictionary<string, AnimationClip>();
    bool canEditController;

    // Other attributes
    int index = 0;
    string currentSelection = "";
    List<string> popUps = new List<string>();

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
        // Load values
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


    /// <summary>
    /// Loads controller and gameObject
    /// </summary>
    private void loadValues()
    {
        if (controller == null)
            controller = (AnimationController)target;
        gameObject = controller.gameObject;
    }


    /// <summary>
    /// GUI update for autoUpdateAnimation
    /// </summary>
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


    /// <summary>
    /// GUI update for loading animation clips
    /// </summary>
    private void property_animationClips()
    {
        string filePath = serializedObject.FindProperty("animationClipsSourcePath").stringValue;

        serializedObject.FindProperty("animationClipsSourcePath").stringValue = EditorGUILayout.TextField
            (
                new GUIContent
                    (
                     "Animation Clips Filepath",
                     "Filepath from where animation clips for this gameObject should be loaded"
                     ),
                filePath
            );

        if (!Directory.Exists(filePath))
        {
            if (GUILayout.Button($"Create directory : {filePath}", GUILayout.Height(25)))
                Directory.CreateDirectory(filePath);
        }
        else
        {
            clips = controller.GetClips();

            if (GUILayout.Button("Refresh clip database", GUILayout.Height(20)))
            {
                clips.Clear();
                popUps.Clear();
                popUps.Add("None");
                foreach (string path in Directory.EnumerateFiles(filePath, "*.anim"))
                {
                    AnimationClip clip = AssetDatabase.LoadAssetAtPath<AnimationClip>(path);
                    if (clip != null)
                    {
                        clips.Add(clip.name, clip);
                        popUps.Add(clip.name);
                    }
                }
            }
        }
    }


    /// <summary>
    /// GUI update for creating animator
    /// </summary>
    private void property_createAnimator()
    {
        if (animator == null)
        {
            if (gameObject.TryGetComponent(out animator) == false)
            {
                GUILayout.BeginHorizontal();

                EditorGUILayout.HelpBox("MISSING COMPONENT! : ANIMATOR", MessageType.Error);
                if (GUILayout.Button("Attach Animator?", GUILayout.Height(37.5f)))
                {
                    animator = gameObject.AddOrGetComponent<Animator>();
                    serializedObject.FindProperty("animator").objectReferenceValue = animator;
                    serializedObject.FindProperty("canUseAnimator").boolValue = true;
                }

                GUILayout.EndHorizontal();
            }
        }

    }


    /// <summary>
    /// GUI update for creating animator controller and updating states and motion
    /// </summary>
    private void property_animationController()
    {
        string controllerFilepath = serializedObject.FindProperty("animatorControllerSavePath").stringValue;
        serializedObject.FindProperty("animatorControllerSavePath").stringValue = EditorGUILayout.TextField
            (
                new GUIContent
                    (
                     "Animator Controller Filepath",
                     "This is the target filepath for new or existing animator controller"
                     ),
                controllerFilepath
            );

        if (!Directory.Exists(controllerFilepath))
        {
            if (GUILayout.Button($"Create directory : {controllerFilepath}", GUILayout.Height(25)))
            {
                Directory.CreateDirectory(controllerFilepath);
                animatorController = AnimatorController.CreateAnimatorControllerAtPath(controllerFilepath + $"/{gameObject.name}.controller");
            }
        }
        else
        {
            animatorController = AssetDatabase.LoadAssetAtPath<AnimatorController>(controllerFilepath + $"/{gameObject.name}.controller");
            if (animatorController == null)
                if (GUILayout.Button("Create animator controller at path"))
                    animatorController = AnimatorController.CreateAnimatorControllerAtPath(controllerFilepath + $"/{gameObject.name}.controller");
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
            if (root.states.Length > 0 && GUILayout.Button("Update motion for states"))
            {
                foreach (var state in root.states)
                    if (state.state.motion == null)
                        if (clips.ContainsKey(state.state.name))
                            state.state.motion = clips[state.state.name];
            }
        }
    }
}
