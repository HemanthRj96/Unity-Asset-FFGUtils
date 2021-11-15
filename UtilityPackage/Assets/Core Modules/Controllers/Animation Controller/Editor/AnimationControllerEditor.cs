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
        bool validClipDirectory => Directory.Exists(_clipSourceCache);
        bool validControllerDirectory => Directory.Exists(_controllerSourceCache);


        SerializedProperty _animator;
        SerializedProperty _animationClipSource;
        SerializedProperty _animationControllerPathSavePath;
        SerializedProperty _stateController;
        SerializedProperty _enableAutoUpdate;

        Animator _animatorCache;
        AnimatorController _controllerCache;
        string _clipSourceCache;
        string _controllerSourceCache;
        StateController _stateControllerCache;

        private void InpectorUpdate()
        {
            _animator = GetProperty("_animator");
            _animationClipSource = GetProperty("_animationClipSource");
            _animationControllerPathSavePath = GetProperty("_animationControllerSavePath");
            _stateController = GetProperty("_stateController");
            _enableAutoUpdate = GetProperty("_enableAutoUpdate");


            #region Animator

            // _animator
            _animatorCache = Root.GetComponent<Animator>();
            if (_animatorCache == null)
            {
                _animatorCache = Root.AddComponent<Animator>();
                _animator.objectReferenceValue = _animatorCache;
            }
            else if (_animator.objectReferenceValue == null)
            {
                _animator.objectReferenceValue = _animatorCache;
            }

            #endregion Animator

            Space(10);
            Heading("Animation Clip Settings");
            Space(5);

            #region Animation Clip Source

            // _animationClipSource
            PropertyField(_animationClipSource, "Animation Clips Source Path", "Filepath to all the animation clips for this gameObject");

            _clipSourceCache = _animationClipSource.stringValue;

            if (_clipSourceCache == "")
            {
                Space(5);
                Info("This field cannot be empty!!", MessageType.Error);
            }
            else if (!validClipDirectory)
            {
                Space(5);
                GUILayout.BeginHorizontal();

                Info("Directory not found!!", MessageType.Error);
                if (Button("Create directory ?", 37.5f))
                {
                    Directory.CreateDirectory(_clipSourceCache);
                    AssetDatabase.Refresh();
                }

                GUILayout.EndHorizontal();
            }
            else if (Directory.GetFiles(_clipSourceCache, "*.anim").Length == 0)
            {
                Space(5);
                Info("Directory is empty, no animation clips found", MessageType.Warning);
            }

            #endregion Animation Clip Source

            Space(10);
            Heading("Animator Controller Settings");
            Space(5);

            #region Animation Controller Save Path

            // _animationControllerSavePath
            PropertyField(_animationControllerPathSavePath, @"Controller Save/Source Path", @"Filepath where the controller has to be saved to/loaded from");

            _controllerSourceCache = _animationControllerPathSavePath.stringValue;

            if (_controllerSourceCache == "")
            {
                Space(5);
                Info("This field cannot be empty!!", MessageType.Error);
                return;
            }
            else if (!validControllerDirectory)
            {
                Space(5);
                GUILayout.BeginHorizontal();
                Info("Directory not found!!", MessageType.Error);
                if (Button("Create directory ?", 37.5f))
                {
                    Directory.CreateDirectory(_controllerSourceCache);
                    AssetDatabase.Refresh();
                }
                GUILayout.EndHorizontal();
                return;
            }
            else if (Directory.GetFiles(_controllerSourceCache, "*.controller").Length == 0)
            {
                Space(5);
                GUILayout.BeginHorizontal();
                Info("Directory is empty, no controller found", MessageType.Warning);
                if (Button("Create controller?", 37.5f))
                {
                    _controllerCache = AnimatorController.CreateAnimatorControllerAtPath($"{_controllerSourceCache}/{Root.gameObject.name}.controller");
                    _animatorCache.runtimeAnimatorController = _controllerCache;
                }
                GUILayout.EndHorizontal();
            }
            else if (_controllerCache == null)
            {
                if (_animatorCache.runtimeAnimatorController != null)
                    _controllerCache = (AnimatorController)_animatorCache.runtimeAnimatorController;
                else
                {
                    string path = Directory.GetFiles(_controllerSourceCache, "*.controller")[0];
                    _controllerCache = AssetDatabase.LoadAssetAtPath<AnimatorController>(path);
                    _animatorCache.runtimeAnimatorController = _controllerCache;
                }
            }

            #endregion Animation Controller Save Path

            #region Enable Auto Update

            // _enableAutoUpdate
            bool shouldEnableAutoUpdate = false;
            if (_controllerCache == null)
                return;
            if (_stateController.objectReferenceValue == null)
            {
                if (Root.TryGetComponent(out _stateControllerCache))
                {
                    shouldEnableAutoUpdate = true;
                    _stateController.objectReferenceValue = _stateControllerCache;
                }
                else
                {
                    shouldEnableAutoUpdate = false;
                    _enableAutoUpdate.boolValue = false;
                    _stateController.objectReferenceValue = null;
                }
            }
            else
            {
                shouldEnableAutoUpdate = true;
                _stateControllerCache = (StateController)_stateController.objectReferenceValue;
            }
            EditorGUI.BeginDisabledGroup(!shouldEnableAutoUpdate);

            PropertyField(_enableAutoUpdate, "Enable Auto Update", "If set then this animation controller will work automatically with a state controller component");

            EditorGUI.EndDisabledGroup();

            #endregion Enable Auto Update

            #region Updating Animator Controller

            // Updating _animator controller
            if (!validClipDirectory)
                return;
            List<AnimationClip> clips = new List<AnimationClip>();
            List<string> dropDown = new List<string>() { "None" };
            AnimatorStateMachine rootState = _controllerCache.layers[0].stateMachine;

            foreach (string path in Directory.EnumerateFiles(_clipSourceCache, "*.anim"))
                clips.Add(AssetDatabase.LoadAssetAtPath<AnimationClip>(path));

            dropDown.AddRange(clips.Select(x => x.name));

            int selection = GetProperty("selection").intValue;

            EditorGUILayout.BeginHorizontal();

            GetProperty("selection").intValue = DropdownList("Default Animator State ", selection, dropDown.ToArray());
            selection = GetProperty("selection").intValue;
            if (Button("Update Controller", 17.5f))
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

            #endregion Updating Animator Controller
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            InpectorUpdate();
            serializedObject.ApplyModifiedProperties();
        }
    }
}