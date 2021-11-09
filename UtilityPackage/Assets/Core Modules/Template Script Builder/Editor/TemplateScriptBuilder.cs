using UnityEditor;

namespace FickleFrames
{
    public class TemplateScriptBuilder
    {
        private static string s_pathToScriptTemplate = @"Assets\Core Modules\Template Script Builder\Template\TestingTemplate.cs.txt";

        private static string s_stateControllerTemplateFilePath = @"Assets\Core Modules\Template Script Builder\Template\state_controller_template.cs.txt";


        [MenuItem(itemName: "Assets/Create/Fickle Frames/Create New State", isValidateFunction: false)]
        public static void CreateStateFromTemplate()
        {
            CreateStateFromTemplate("NewState.cs");
        }


        public static void CreateStateFromTemplate(string scriptName)
        {
            scriptBuilderSlave(s_stateControllerTemplateFilePath, scriptName);
        }





        [MenuItem(itemName: "Assets/Create/Fickle Frames/Create New Testing Script", isValidateFunction: false)]
        public static void CreateScriptFromTemplate()
        {
            scriptBuilderSlave(s_pathToScriptTemplate, "TestingScript_.cs");
        }


        private static void scriptBuilderSlave(string scriptTemplatePath, string scriptName)
        {
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(scriptTemplatePath, scriptName);
        }
    }
}