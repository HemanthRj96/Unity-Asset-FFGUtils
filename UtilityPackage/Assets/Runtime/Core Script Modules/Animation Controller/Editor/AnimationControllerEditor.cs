using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEditor.Animations;
using System.Collections.Generic;
using System.Linq;


namespace FickleFrames.Controllers.Editor_
{
    [CustomEditor(typeof(AnimationController))]
    public class AnimationControllerEditor : BaseEditor<AnimationController>
    {
        Animator _animator;
        AnimatorController controller;
        string _animationClipSource;
        string _animationControllerSavePath;
        StateController _stateController;
        bool shouldEnableAutoUpdate = false;

        bool validClipDirectory => Directory.Exists(_animationClipSource);
        bool validControllerDirectory => Directory.Exists(_animationControllerSavePath);

        private void InpectorUpdate()
        {
            // _animator
            _animator = root.GetComponent<Animator>();
            if (_animator == null)
            {
                _animator = root.AddComponent<Animator>();
                getProperty("_animator").objectReferenceValue = _animator;
            }
            else if (getProperty("_animator").objectReferenceValue == null)
            {
                getProperty("_animator").objectReferenceValue = _animator;
            }


            space(10);
            heading("Animation Clip Settings");
            space(5);


            // _animationClipSource
            propertyField(getProperty("_animationClipSource"), "Animation Clips Source Path", "Filepath to all the animation clips for this gameObject");
            _animationClipSource = getProperty("_animationClipSource").stringValue;

            if (_animationClipSource == "")
            {
                space(5);
                info("This field cannot be empty!!", MessageType.Error);
            }
            else if (!validClipDirectory)
            {
                space(5);
                GUILayout.BeginHorizontal();
                info("Directory not found!!", MessageType.Error);
                if (button("Create directory ?", 37.5f))
                {
                    Directory.CreateDirectory(_animationClipSource);
                    AssetDatabase.Refresh();
                }
                GUILayout.EndHorizontal();
            }
            else if (Directory.GetFiles(_animationClipSource, "*.anim").Length == 0)
            {
                space(5);
                info("Directory is empty, no animation clips found", MessageType.Warning);
            }


            space(10);
            heading("Animator Controller Settings");
            space(5);


            // _animationControllerSavePath
            propertyField(getProperty("_animationControllerSavePath"), @"Controller Save/Source Path", @"Filepath where the controller has to be saved to/loaded from");
            _animationControllerSavePath = getProperty("_animationControllerSavePath").stringValue;

            if (_animationControllerSavePath == "")
            {
                space(5);
                info("This field cannot be empty!!", MessageType.Error);
                return;
            }
            else if (!validControllerDirectory)
            {
                space(5);
                GUILayout.BeginHorizontal();
                info("Directory not found!!", MessageType.Error);
                if (button("Create directory ?", 37.5f))
                {
                    Directory.CreateDirectory(_animationControllerSavePath);
                    AssetDatabase.Refresh();
                }
                GUILayout.EndHorizontal();
                return;
            }
            else if (Directory.GetFiles(_animationControllerSavePath, "*.controller").Length == 0)
            {
                space(5);
                GUILayout.BeginHorizontal();
                info("Directory is empty, no controller found", MessageType.Warning);
                if (button("Create controller?", 37.5f))
                {
                    controller = AnimatorController.CreateAnimatorControllerAtPath($"{_animationControllerSavePath}/{root.gameObject.name}.controller");
                    _animator.runtimeAnimatorController = controller;
                }
                GUILayout.EndHorizontal();
            }
            else if (controller == null)
            {
                if (_animator.runtimeAnimatorController != null)
                    controller = (AnimatorController)_animator.runtimeAnimatorController;
                else
                {
                    string path = Directory.GetFiles(_animationControllerSavePath, "*.controller")[0];
                    controller = AssetDatabase.LoadAssetAtPath<AnimatorController>(path);
                    _animator.runtimeAnimatorController = controller;
                }
            }


            // _enableAutoUpdate
            if (controller == null)
                return;
            if (getProperty("_stateController").objectReferenceValue == null)
            {
                if (root.TryGetComponent(out _stateController))
                {
                    shouldEnableAutoUpdate = true;
                    getProperty("_stateController").objectReferenceValue = _stateController;
                }
                else
                {
                    shouldEnableAutoUpdate = false;
                    getProperty("_enableAutoUpdate").boolValue = false;
                    getProperty("_stateController").objectReferenceValue = null;
                }
            }
            else
            {
                shouldEnableAutoUpdate = true;
                _stateController = (StateController)getProperty("_stateController").objectReferenceValue;
            }
            EditorGUI.BeginDisabledGroup(!shouldEnableAutoUpdate);
            propertyField(getProperty("_enableAutoUpdate"), "Enable Auto Update", "If set then this animation controller will work automatically with a state controller component");
            EditorGUI.EndDisabledGroup();


            // Updating _animator controller
            if (!validClipDirectory)
                return;
            List<AnimationClip> clips = new List<AnimationClip>();
            List<string> dropDown = new List<string>() { "None" };
            AnimatorStateMachine rootState = controller.layers[0].stateMachine;

            foreach (string path in Directory.EnumerateFiles(_animationClipSource, "*.anim"))
                clips.Add(AssetDatabase.LoadAssetAtPath<AnimationClip>(path));
            dropDown.AddRange(clips.Select(x => x.name));

            EditorGUILayout.BeginHorizontal();
            int selection = getProperty("selection").intValue;
            getProperty("selection").intValue = dropdownList("Default Animator State ", selection, dropDown.ToArray());
            selection = getProperty("selection").intValue;
            if (button("Update Controller", 17.5f))
            {
                foreach (var _animatorState in rootState.states)
                    rootState.RemoveState(_animatorState.state);
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