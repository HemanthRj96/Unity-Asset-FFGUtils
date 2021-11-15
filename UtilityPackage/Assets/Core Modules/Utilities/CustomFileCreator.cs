using FickleFrames.Managers;
using UnityEditor;
using UnityEngine;

namespace FickleFrames
{
    public class CustomFileCreator
    {
        /*.............................................Private Fields.......................................................*/

        private const string _FILE_PATH_ = @"Assets/Core Modules/Utilities/Script Templates/";
        private static readonly string s_stateControllerTemplateFilePath = _FILE_PATH_ + "state_controller_template.cs.txt";
        private static readonly string s_actionSystemTemplateFilepath = _FILE_PATH_ + "action_system_template.cs.txt";
        private static readonly string s_subGameManagerTemplateFilepath = _FILE_PATH_ + "game_manager_template.cs.txt";

        /*.............................................Private Methods......................................................*/

        /// <summary>
        /// Script builder that creates the scripts in the backend
        /// </summary>
        /// <param name="scriptTemplatePath">template path</param>
        /// <param name="scriptName">Name for the script</param>
        private static void scriptBuilder(string scriptTemplatePath, string scriptName)
        {
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(scriptTemplatePath, scriptName);
        }

        /*.............................................Public Methods.......................................................*/

        /// <summary>
        /// Method to create game manager
        /// </summary>
        [MenuItem(itemName: "Fickle Frames/Create Game Manager [Type: GameManager, FileType: GameObject]", isValidateFunction: false)]
        public static void CreateGameManager()
        {
            new GameObject("-Game-Manager-", typeof(GameManager));
        }


        /// <summary>
        /// Method to create level manager
        /// </summary>
        [MenuItem(itemName: "Fickle Frames/Create Level Manager [Type: LevelManager, FileType: GameObject]", isValidateFunction: false)]
        public static void CreateLevelManager()
        {
            new GameObject("-Level-Manager-", typeof(LevelManager));
        }


        /// <summary>
        /// Method to create a sound manager
        /// </summary>
        [MenuItem(itemName: "Fickle Frames/Create Sound Manager [Type: SoundManager, FileType: GameObject]", isValidateFunction: false)]
        public static void CreateNewSoundManager()
        {
            new GameObject("-Sound-Manager-", typeof(SoundManager));
        }


        /// <summary>
        /// Method to create custom sub game manager from template file
        /// </summary>
        [MenuItem(itemName: "Assets/Create/Fickle Frames/Create New Sub-Game Manager [Type: SubGameManagerBase, FileType: C# script]", isValidateFunction: false)]
        public static void CreateSubGameManager()
        {
            scriptBuilder(s_subGameManagerTemplateFilepath, "NewSubGameManager.cs");
        }


        /// <summary>
        /// Method to create custom state from template file
        /// </summary>
        [MenuItem(itemName: "Assets/Create/Fickle Frames/Create New State From Template [Type: State, FileType: C# script]", isValidateFunction: false)]
        public static void CreateStateFromTemplate()
        {
            scriptBuilder(s_stateControllerTemplateFilePath, "NewState.cs");
        }


        /// <summary>
        /// Method to create custom action component
        /// </summary>
        [MenuItem(itemName: "Assets/Create/Fickle Frames/Create New Action Component [Type: ActionComponent, FileType: C# script]", isValidateFunction: false)]
        public static void CreateActionComponentFromTemplate()
        {
            scriptBuilder(s_actionSystemTemplateFilepath, "NewActionComponent.cs");
        }
    }
}