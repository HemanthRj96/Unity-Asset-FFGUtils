using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using System.IO;


namespace FickleFrames.Utility
{
    public enum ERecorderAction
    {
        StartRecording,
        StopRecording,
        DeleteRecording
    }

    public class PreBuildAnimationRecorder : Singleton<PreBuildAnimationRecorder>
    {
        /*.............................................Serialized Fields....................................................*/

        [Space(3)]
        [Header("-Save Settings-")]
        [Tooltip("Input a unique that will be used to save data into PlayerPrefs")]
        [SerializeField] private string _playerPrefsKey = "";
        [Tooltip("Set this as true if you want to reset PlayerPrefs key, use it if you delete all animations")]
        [SerializeField] private bool _resetIndex = false;
        [Space(3)]
        [Header("-Recorder Settings-")]
        [Tooltip("This is the target GameObject we want to record")]
        [SerializeField] private GameObject _targetGameObject = null;
        [Tooltip("Save file name that must be used")]
        [SerializeField] private string _clipFileSaveName = "";
        [Tooltip("Do not modify it unless necessary")]
        [SerializeField] private string _saveAnimationClipsTo = "Assets/Animation Recordings/Clips/";
        [Tooltip("Do not modify it unless necessary")]
        [SerializeField] private string _saveAnimationControllerTo = "Assets/Animation Recordings/Controllers/";
        [SerializeField] float _frameRate = 24;
        [Space(3)]
        [Header("-Recorder Key Bindings-")]
        [Tooltip("Set this as true if you want trigger the recording and deletion from a script")]
        [SerializeField] private bool _useExternalTrigger = false;
        [SerializeField] private KeyCode _startRecord = KeyCode.Mouse0;
        [SerializeField] private KeyCode _stopRecord = KeyCode.Mouse1;
        [SerializeField] private KeyCode _deleteRecord = KeyCode.Mouse2;

        /*.............................................Private Fields.......................................................*/
       
        private bool _shouldRecord = false;
        private bool _isRecorderReady = false;
        private int _index = 0;
        private GameObjectRecorder _recorder = null;
        private AnimationClip _cachedClip = null;
        private bool _animatorLoaded = false;
        private const string DEFAULT_KEY_SUFFIX = "Animation_Recorder";

        /*.............................................Properties...........................................................*/
        
        private bool subjectReady => _targetGameObject != null;
        private string key => DEFAULT_KEY_SUFFIX + $"_{_playerPrefsKey}";
        private string saveName => _clipFileSaveName + $"_{_index}";

        /*.............................................Private Methods......................................................*/

        /// <summary>
        /// Get key when this gameObject is active
        /// </summary>
        private void OnEnable()
        {
            // Initialize clipFileSaveName if empty
            if (_clipFileSaveName == "")
                _clipFileSaveName = gameObject.name;

            // Check if key has to reset
            if (_resetIndex == false)
                _index = PlayerPrefs.GetInt(key, 0);
            else
                PlayerPrefs.DeleteKey(key);
        }


        /// <summary>
        /// Save key when this gameObject is deactivated
        /// </summary>
        private void OnDisable()
        {
            // Stop recording and save clip if recorder is recording
            if (_shouldRecord)
                stopRecording();
            // Save index to playerPrefs
            PlayerPrefs.SetInt(key, _index);
        }


        /// <summary>
        /// Initializes recorder
        /// </summary>
        private void Start()
        {
            // Initialize recorder
            initializeRecorder();
            // Initialize directories
            initializeDirectories();
        }


        /// <summary>
        /// Check for inputs
        /// </summary>
        private void Update()
        {
            // If using external trigger then don't check for inputs
            if (_useExternalTrigger == false && _isRecorderReady)
                inputUpdate();
        }


        /// <summary>
        /// Recorder update
        /// </summary>
        private void LateUpdate()
        {
            if (_isRecorderReady && subjectReady)
                tryRecord();
        }


        /// <summary>
        /// Initialize recorder
        /// </summary>
        private void initializeRecorder()
        {
            if (_targetGameObject != null)
            {
                _recorder = new GameObjectRecorder(_targetGameObject);
                _recorder.BindComponentsOfType<Component>(_targetGameObject, true);
                _isRecorderReady = true;
            }
        }


        /// <summary>
        /// Initialize directories
        /// </summary>
        private void initializeDirectories()
        {
            if (!Directory.Exists(_saveAnimationClipsTo))
                Directory.CreateDirectory(_saveAnimationClipsTo);
            if (!Directory.Exists(_saveAnimationControllerTo))
                Directory.CreateDirectory(_saveAnimationControllerTo);
        }


        /// <summary>
        /// Records the targetGameObject
        /// </summary>
        private void tryRecord()
        {
            // Check if there's a clip
            if (_cachedClip == null)
            {
                _cachedClip = createNewAnimationClip(saveName, _frameRate);
                ++_index;
            }
            // Record is recorder is ready and shouldRecord is enabled
            if (_shouldRecord)
                _recorder.TakeSnapshot(Time.deltaTime);
        }


        /// <summary>
        /// Check for inputs
        /// </summary>
        private void inputUpdate()
        {
            if (Input.GetKeyDown(_startRecord))
                startRecording();
            else if (Input.GetKeyDown(_stopRecord))
                stopRecording();
            else if (Input.GetKeyDown(_deleteRecord))
                deleteRecording();
        }


        /// <summary>
        /// Starts recording
        /// </summary>
        private void startRecording()
        {
            if (_shouldRecord == true)
                return;
            Debug.Log("RECORDING");
            _shouldRecord = true;
        }


        /// <summary>
        /// Stops recording and saves animation clip
        /// </summary>
        private void stopRecording()
        {
            if (_shouldRecord == false)
                return;

            _shouldRecord = false;
            Debug.Log("STOP RECORDING");
            // Save recording to the animation clip
            _recorder.SaveToClip(_cachedClip);
            // Save the animation clip locally
            saveRecording(_cachedClip);
            _cachedClip = null;
            // Create or load animator and animator controller
            if (_animatorLoaded == false)
                loadAnimatorAndController();
        }


        /// <summary>
        /// Delete most recent animation clip
        /// </summary>
        private void deleteRecording()
        {
            // If currently recording then wait until it stops
            if (_shouldRecord == true)
                return;
            Debug.LogWarning("DELETING LAST RECORDING");
            deleteRecording(_cachedClip);
        }


        /// <summary>
        /// Return a new animation clip
        /// </summary>
        private AnimationClip createNewAnimationClip(string clipName, float frameRate)
        {
            AnimationClip clip = new AnimationClip();
            clip.name = clipName; ;
            clip.frameRate = frameRate;
            return clip;
        }


        /// <summary>
        /// Create animator and animator controller
        /// </summary>
        private void loadAnimatorAndController()
        {
            Animator animator;
            AnimatorController controller;
            if (_targetGameObject.GetComponent<Animator>() == null)
            {
                animator = _targetGameObject.AddComponent<Animator>();
                controller = AnimatorController.CreateAnimatorControllerAtPath(_saveAnimationControllerTo + _targetGameObject.name + ".controller");
                animator.runtimeAnimatorController = controller;
            }
            _animatorLoaded = true;
        }


        /// <summary>
        /// Save animation locally
        /// </summary>
        private void saveRecording(AnimationClip clip)
        {
            AssetDatabase.CreateAsset(clip, _saveAnimationClipsTo + clip.name + ".anim");
            AssetDatabase.SaveAssets();
        }


        /// <summary>
        /// Delete animation clip
        /// </summary>
        private void deleteRecording(AnimationClip clip)
        {
            if (AssetDatabase.Contains(clip))
                AssetDatabase.DeleteAsset(_saveAnimationClipsTo + clip.name + ".anim");
        }

        /*.............................................Public Methods.......................................................*/

        /// <summary>
        /// Method to control recorder externally
        /// </summary>
        public void ExternalRecorderInput(ERecorderAction action)
        {
            if (_useExternalTrigger)
            {
                switch (action)
                {
                    case ERecorderAction.StartRecording:
                        startRecording();
                        break;
                    case ERecorderAction.StopRecording:
                        stopRecording();
                        break;
                    case ERecorderAction.DeleteRecording:
                        deleteRecording();
                        break;
                }
            }
        }


        /// <summary>
        /// Changes the target recording object
        /// </summary>
        /// <param name="targetGameObject">New target GameObject</param>
        public void ChangeTarget(GameObject targetGameObject)
        {
            if (targetGameObject != null)
                this._targetGameObject = targetGameObject;
        }
    }
}