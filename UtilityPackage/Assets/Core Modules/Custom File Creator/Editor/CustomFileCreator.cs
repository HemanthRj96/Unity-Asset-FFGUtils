using FickleFrames.Managers;
using UnityEditor;
using UnityEngine;

namespace FickleFrames
{
    public class CustomFileCreator
    {
        /*.............................................Private Fields.......................................................*/

        private static readonly string s_stateControllerTemplateFilePath = @"Assets\Core Modules\Custom File Creator\Template\state_controller_template.cs.txt";
        private static readonly string s_actionSystemTemplateFilepath = @"Assets\Core Modules\Custom File Creator\Template\action_system_template.cs.txt";
        private static readonly string s_subGameManagerTemplateFilepath = @"Assets\Core Modules\Custom File Creator\Template\game_manager_template.cs.txt";
        private const string MENUNAME = "Fickle Frames/";

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
        [MenuItem(itemName: MENUNAME + "Managers/1. Create A Game Manager", isValidateFunction: false)]
        public static void CreateGameManager()
        {
            new GameObject("-Game Manager-", typeof(GameManager));
        }


        /// <summary>
        /// Method to create level manager
        /// </summary>
        [MenuItem(itemName: MENUNAME + "Managers/2. Create A Level Manager", isValidateFunction: false)]
        public static void CreateLevelManager()
        {
            new GameObject("-Level Manager-", typeof(LevelManager));
        }

        /// <summary>
        /// Method to create custom sub game manager from template file
        /// </summary>
        [MenuItem(itemName: MENUNAME + "Managers/3. Create New Sub Game Manager", isValidateFunction: false)]
        public static void CreateSubGameManager()
        {
            scriptBuilder(s_subGameManagerTemplateFilepath, "NewSubGameManager.cs");
        }


        /// <summary>
        /// Method to create custom state from template file
        /// </summary>
        [MenuItem(itemName: MENUNAME + "Controllers/Create New State From Template", isValidateFunction: false)]
        public static void CreateStateFromTemplate()
        {
            scriptBuilder(s_stateControllerTemplateFilePath, "NewState.cs");
        }


        /// <summary>
        /// Method to create custom action component
        /// </summary>
        [MenuItem(itemName: MENUNAME + "Systems/Create New Action Component", isValidateFunction: false)]
        public static void CreateActionComponentFromTemplate()
        {
            scriptBuilder(s_actionSystemTemplateFilepath, "NewActionComponent.cs");
        }
    }
}