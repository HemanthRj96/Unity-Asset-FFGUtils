using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEditor.Animations;
using System.Collections.Generic;
using System.Linq;


namespace FickleFrames.Controllers.AnimationControllerEditor_
{
    [CustomEditor(typeof(AnimationController))]
    public class AnimationControllerEditor : CustomInspector<AnimationController>
    {
        Animator animator;
        AnimatorController controller;
        string animationClipSource;
        string animationControllerSavePath;
        StateController stateController;
        bool shouldEnableAutoUpdate = false;

        private void InpectorUpdate()
        {
            // animator
            animator = root.GetComponent<Animator>();
            if (animator == null)
            {
                animator = root.AddComponent<Animator>();
                getProperty("animator").objectReferenceValue = animator;
            }
            else if (getProperty("animator").objectReferenceValue == null)
            {
                getProperty("animator").objectReferenceValue = animator;
            }


            space(10);
            heading("-Controller Settings-");
            space(5);


            // animationClipSource
            propertyField(getProperty("animationClipSource"), "Animation Clips Source Path", "Filepath to all the animation clips for this gameObject");
            animationClipSource = getProperty("animationClipSource").stringValue;

            if (animationClipSource == "")
            {
                space(5);
                info("This field cannot be empty!!", MessageType.Error);
                return;
            }
            else if (!Directory.Exists(animationClipSource))
            {
                space(5);
                GUILayout.BeginHorizontal();
                info("Directory not found!!", MessageType.Error);
                if (button("Create directory ?", 37.5f))
                {
                    Directory.CreateDirectory(animationClipSource);
                    AssetDatabase.Refresh();
                }
                GUILayout.EndHorizontal();
                return;
            }
            else if (Directory.GetFiles(animationClipSource, "*.anim").Length == 0)
            {
                space(5);
                info("Directory is empty, no animation clips found", MessageType.Warning);
            }


            // animationControllerSavePath
            propertyField(getProperty("animationControllerSavePath"), @"Controller Save/Source Path", @"Filepath where the controller has to be saved to/loaded from");
            animationControllerSavePath = getProperty("animationControllerSavePath").stringValue;

            if (animationControllerSavePath == "")
            {
                space(5);
                info("This field cannot be empty!!", MessageType.Error);
                return;
            }
            else if (!Directory.Exists(animationControllerSavePath))
            {
                space(5);
                GUILayout.BeginHorizontal();
                info("Directory not found!!", MessageType.Error);
                if (button("Create directory ?", 37.5f))
                {
                    Directory.CreateDirectory(animationControllerSavePath);
                    AssetDatabase.Refresh();
                }
                GUILayout.EndHorizontal();
                return;
            }
            else if (Directory.GetFiles(animationControllerSavePath, "*.controller").Length == 0)
            {
                space(5);
                GUILayout.BeginHorizontal();
                info("Directory is empty, no controller found", MessageType.Warning);
                if (button("Create controller?", 37.5f))
                {
                    controller = AnimatorController.CreateAnimatorControllerAtPath($"{animationControllerSavePath}/{root.gameObject.name}.controller");
                    animator.runtimeAnimatorController = controller;
                }
                GUILayout.EndHorizontal();
            }
            else if (controller == null)
            {
                if (animator.runtimeAnimatorController != null)
                    controller = (AnimatorController)animator.runtimeAnimatorController;
                else
                {
                    string path = Directory.GetFiles(animationControllerSavePath, "*.controller")[0];
                    controller = AssetDatabase.LoadAssetAtPath<AnimatorController>(path);
                    animator.runtimeAnimatorController = controller;
                }
            }


            // enableAutoUpdate
            if (getProperty("stateController").objectReferenceValue == null)
            {
                if (root.TryGetComponent(out stateController))
                {
                    shouldEnableAutoUpdate = true;
                    getProperty("stateController").objectReferenceValue = stateController;
                }
                else
                {
                    shouldEnableAutoUpdate = false;
                    getProperty("enableAutoUpdate").boolValue = false;
                    getProperty("stateController").objectReferenceValue = null;
                }
            }
            else
            {
                shouldEnableAutoUpdate = true;
                stateController = (StateController)getProperty("stateController").objectReferenceValue;
            }

            EditorGUI.BeginDisabledGroup(!shouldEnableAutoUpdate);
            propertyField(getProperty("enableAutoUpdate"), "Enable Auto Update", "If set then this animation controller will work automatically with a state controller component");
            EditorGUI.EndDisabledGroup();


            space(10);
            heading("-Animator Controller Settings-");
            space(5);


            // Updating animator controller
            List<AnimationClip> clips = new List<AnimationClip>();
            List<string> dropDown = new List<string>() { "None" };
            AnimatorStateMachine rootState = controller.layers[0].stateMachine;

            foreach (string path in Directory.EnumerateFiles(animationClipSource, "*.anim"))
                clips.Add(AssetDatabase.LoadAssetAtPath<AnimationClip>(path));
            dropDown.AddRange(clips.Select(x => x.name));

            EditorGUILayout.BeginHorizontal();
            int selection = getProperty("selection").intValue;
            getProperty("selection").intValue = dropdownList("Default Animator State ", selection, dropDown.ToArray());
            selection = getProperty("selection").intValue;
            if (button("Update Controller", 17.5f))
            {
                foreach (var animatorState in rootState.states)
                    rootState.RemoveState(animatorState.state);
                foreach (AnimationClip clip in clips)
                {
                    if (dropDown[selection] == clip.name)
                    {
                        rootState.defaultState = rootState.AddState(clip.name);
                        rootState.defaultState.motion = clip;
                    }
                    else
                        rootState.AddState(clip.name).motion = clip;
                }
                if (dropDown[selection] == "None")
                    rootState.defaultState = rootState.AddState("None");
            }
            EditorGUILayout.EndHorizontal();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            InpectorUpdate();
            serializedObject.ApplyModifiedProperties();
        }
    }
}