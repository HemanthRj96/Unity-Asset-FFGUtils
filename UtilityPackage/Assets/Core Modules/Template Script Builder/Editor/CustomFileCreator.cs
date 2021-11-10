using FickleFrames.Managers;
using UnityEditor;
using UnityEngine;

namespace FickleFrames
{
    public class CustomFileCreator
    {
        #region Internal

        private static readonly string s_stateControllerTemplateFilePath = @"Assets\Core Modules\Template Script Builder\Template\state_controller_template.cs.txt";
        private static readonly string s_actionSystemTemplateFilepath = @"Assets\Core Modules\Template Script Builder\Template\action_system_template.cs.txt";
        private static readonly string s_subGameManagerTemplateFilepath = @"Assets\Core Modules\Template Script Builder\Template\game_manager_template.cs.txt";
        private const string MENUNAME = "Assets/Create/-Fickle Frames-/";

        #region Private Methods

        /// <summary>
        /// Script builder that creates the scripts in the backend
        /// </summary>
        /// <param name="scriptTemplatePath">template path</param>
        /// <param name="scriptName">Name for the script</param>
        private static void scriptBuilder(string scriptTemplatePath, string scriptName)
        {
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(scriptTemplatePath, scriptName);
        }

        #endregion

        #endregion

        #region Public Methods

        /// <summary>
        /// Method to create game manager
        /// </summary>
        [MenuItem(itemName: MENUNAME + "Managers/2. Create A Game Manager", isValidateFunction: false)]
        public static void CreateGameManager()
        {
            GameObject gameObject = new GameObject("-Game Manager-", typeof(GameManager));
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
        [MenuItem(itemName: MENUNAME + "Controllers/2. Create New State From Template", isValidateFunction: false)]
        public static void CreateStateFromTemplate()
        {
            scriptBuilder(s_stateControllerTemplateFilePath, "NewState.cs");
        }


        /// <summary>
        /// Method to create custom action component
        /// </summary>
        [MenuItem(itemName: MENUNAME + "Systems/1. Create New Action Component", isValidateFunction: false)]
        public static void CreateActionComponentFromTemplate()
        {
            scriptBuilder(s_actionSystemTemplateFilepath, "NewActionComponent.cs");
        }

        #endregion
    }
}