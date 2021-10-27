using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using System.IO;


namespace FickleFrames.Utility
{
    public class AnimationRecorderBuildExcluded : Singleton<AnimationRecorderBuildExcluded>
    {
        #region Private Fields

        private const string DEFAULT_KEY_SUFFIX = "Animation_Recorder";

        [Space(3)]
        [Header("-Save Settings-")]
        [Tooltip("Input a unique that will be used to save data into PlayerPrefs")]
        [SerializeField]
        private string playerPrefsKey = "";
        [Tooltip("Set this as true if you want to reset PlayerPrefs key, use it if you delete all animations")]
        [SerializeField]
        private bool resetIndex = false;

        [Space(3)]
        [Header("-Recorder Settings-")]
        [Tooltip("This is the target GameObject we want to record")]
        [SerializeField]
        private GameObject targetGameObject = null;
        [Tooltip("Save file name that must be used")]
        [SerializeField]
        private string clipFileSaveName = "";
        [Tooltip("Do not modify it unless necessary")]
        [SerializeField]
        private string saveAnimationClipsTo = "Assets/Animation Recordings/Clips/";
        [Tooltip("Do not modify it unless necessary")]
        [SerializeField]
        private string saveAnimationControllerTo = "Assets/Animation Recordings/Controllers/";
        [SerializeField]
        float frameRate = 24;

        [Space(3)]
        [Header("-Recorder Key Bindings-")]
        [Tooltip("Set this as true if you want trigger the recording and deletion from a script")]
        [SerializeField]
        private bool useExternalTrigger = false;
        [SerializeField]
        private KeyCode startRecord = KeyCode.Mouse0;
        [SerializeField]
        private KeyCode stopRecord = KeyCode.Mouse1;
        [SerializeField]
        private KeyCode deleteRecord = KeyCode.Mouse2;
        private bool shouldRecord = false;
        private bool isRecorderReady = false;
        private int index = 0;
        private GameObjectRecorder recorder = null;
        private AnimationClip cachedClip = null;
        private bool animatorLoaded = false;

        #endregion Private Fields


        #region Private Properties

        private bool subjectReady => targetGameObject != null;
        private string key => DEFAULT_KEY_SUFFIX + $"_{playerPrefsKey}";
        private string saveName => clipFileSaveName + $"_{index}";

        #endregion Private Properties


        #region Private Methods

        /// <summary>
        /// Get key when this gameObject is active
        /// </summary>
        private void OnEnable()
        {
            // Initialize clipFileSaveName if empty
            if (clipFileSaveName == "")
                clipFileSaveName = gameObject.name;

            // Check if key has to reset
            if (resetIndex == false)
                index = PlayerPrefs.GetInt(key, 0);
            else
                PlayerPrefs.DeleteKey(key);
        }


        /// <summary>
        /// Save key when this gameObject is deactivated
        /// </summary>
        private void OnDisable()
        {
            // Stop recording and save clip if recorder is recording
            if (shouldRecord)
                stopRecording();
            // Save index to playerPrefs
            PlayerPrefs.SetInt(key, index);
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
            if (useExternalTrigger == false && isRecorderReady)
                inputUpdate();
        }


        /// <summary>
        /// Recorder update
        /// </summary>
        private void LateUpdate()
        {
            if (isRecorderReady && subjectReady)
                tryRecord();
        }


        /// <summary>
        /// Initialize recorder
        /// </summary>
        private void initializeRecorder()
        {
            if (targetGameObject != null)
            {
                recorder = new GameObjectRecorder(targetGameObject);
                recorder.BindComponentsOfType<Component>(targetGameObject, true);
                isRecorderReady = true;
            }
        }


        /// <summary>
        /// Initialize directories
        /// </summary>
        private void initializeDirectories()
        {
            if (!Directory.Exists(saveAnimationClipsTo))
                Directory.CreateDirectory(saveAnimationClipsTo);
            if (!Directory.Exists(saveAnimationControllerTo))
                Directory.CreateDirectory(saveAnimationControllerTo);
        }


        /// <summary>
        /// Records the targetGameObject
        /// </summary>
        private void tryRecord()
        {
            // Check if there's a clip
            if (cachedClip == null)
            {
                cachedClip = createNewAnimationClip(saveName, frameRate);
                ++index;
            }
            // Record is recorder is ready and shouldRecord is enabled
            if (shouldRecord)
                recorder.TakeSnapshot(Time.deltaTime);
        }


        /// <summary>
        /// Check for inputs
        /// </summary>
        private void inputUpdate()
        {
            if (Input.GetKeyDown(startRecord))
                startRecording();
            else if (Input.GetKeyDown(stopRecord))
                stopRecording();
            else if (Input.GetKeyDown(deleteRecord))
                deleteRecording();
        }


        /// <summary>
        /// Starts recording
        /// </summary>
        private void startRecording()
        {
            if (shouldRecord == true)
                return;
            Debug.Log("RECORDING");
            shouldRecord = true;
        }


        /// <summary>
        /// Stops recording and saves animation clip
        /// </summary>
        private void stopRecording()
        {
            if (shouldRecord == false)
                return;

            shouldRecord = false;
            Debug.Log("STOP RECORDING");
            // Save recording to the animation clip
            recorder.SaveToClip(cachedClip);
            // Save the animation clip locally
            saveRecording(cachedClip);
            cachedClip = null;
            // Create or load animator and animator controller
            if (animatorLoaded == false)
                loadAnimatorAndController();
        }


        /// <summary>
        /// Delete most recent animation clip
        /// </summary>
        private void deleteRecording()
        {
            // If currently recording then wait until it stops
            if (shouldRecord == true)
                return;
            Debug.LogWarning("DELETING LAST RECORDING");
            deleteRecording(cachedClip);
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
            if (targetGameObject.GetComponent<Animator>() == null)
            {
                animator = targetGameObject.AddComponent<Animator>();
                controller = AnimatorController.CreateAnimatorControllerAtPath(saveAnimationControllerTo + targetGameObject.name + ".controller");
                animator.runtimeAnimatorController = controller;
            }
            animatorLoaded = true;
        }


        /// <summary>
        /// Save animation locally
        /// </summary>
        private void saveRecording(AnimationClip clip)
        {
            AssetDatabase.CreateAsset(clip, saveAnimationClipsTo + clip.name + ".anim");
            AssetDatabase.SaveAssets();
        }


        /// <summary>
        /// Delete animation clip
        /// </summary>
        private void deleteRecording(AnimationClip clip)
        {
            if (AssetDatabase.Contains(clip))
                AssetDatabase.DeleteAsset(saveAnimationClipsTo + clip.name + ".anim");
        }

        #endregion Private Methods


        #region Public Methods

        /// <summary>
        /// Method to control recorder externally
        /// </summary>
        public void ExternalRecorderInput(ERecorderAction action)
        {
            if (useExternalTrigger)
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
                this.targetGameObject = targetGameObject;
        }

        #endregion Public Methods
    }
}