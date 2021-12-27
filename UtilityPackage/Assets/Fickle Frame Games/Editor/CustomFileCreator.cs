using FFG.Managers;
using FFG.Systems;
using UnityEditor;
using UnityEngine;

public static class CustomFileCreator
{
    /*.............................................Private Fields.......................................................*/

    private const string _FILE_PATH_ = @"Assets/Fickle Frame Games/Editor/Script Templates/";
    private static readonly string s_stateControllerTemplateFilePath = _FILE_PATH_ + "state_controller_template.cs.txt";
    private static readonly string s_actionSystemTemplateFilepath = _FILE_PATH_ + "action_system_template.cs.txt";
    private static readonly string s_subGameManagerTemplateFilepath = _FILE_PATH_ + "game_manager_template.cs.txt";
    private static readonly string s_stateSharedDataTemplateFilepath = _FILE_PATH_ + "state_shared_data_template.cs.txt";
    private static readonly string s_stateSyncInputTemplateFilepath = _FILE_PATH_ + "state_sync_input_template.cs.txt";

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
    [MenuItem(itemName: "Fickle Frame Games/Create Game Manager [Type: GameManager, FileType: GameObject]", isValidateFunction: false)]
    public static void CreateGameManager()
    {
        new GameObject("-Game-Manager-", typeof(GameManager));
    }


    /// <summary>
    /// Method to create level manager
    /// </summary>
    [MenuItem(itemName: "Fickle Frame Games/Create Level Manager [Type: LevelManager, FileType: GameObject]", isValidateFunction: false)]
    public static void CreateLevelManager()
    {
        new GameObject("-Level-Manager-", typeof(LevelManager));
    }


    /// <summary>
    /// Method to create a sound manager
    /// </summary>
    [MenuItem(itemName: "Fickle Frame Games/Create Sound Manager [Type: SoundManager, FileType: GameObject]", isValidateFunction: false)]
    public static void CreateNewSoundManager()
    {
        new GameObject("-Sound-Manager-", typeof(SoundManager));
    }


    /// <summary>
    /// Method to create custom sub game manager from template file
    /// </summary>
    [MenuItem(itemName: "Assets/Create/Fickle Frame Games/Create New Sub-Game Manager [Type: SubGameManagerBase, FileType: C# script]", isValidateFunction: false)]
    public static void CreateSubGameManager()
    {
        scriptBuilder(s_subGameManagerTemplateFilepath, "NewSubGameManager.cs");
    }


    /// <summary>
    /// Method to create custom state from template file
    /// </summary>
    [MenuItem(itemName: "Assets/Create/Fickle Frame Games/Create New State From Template [Type: State, FileType: C# script]", isValidateFunction: false)]
    public static void CreateStateFromTemplate()
    {
        scriptBuilder(s_stateControllerTemplateFilePath, "NewState.cs");
    }


    /// <summary>
    /// Method to create state sync data from template file
    /// </summary>
    [MenuItem(itemName: "Assets/Create/Fickle Frame Games/Create New State Shared Data From Template [Type: StateSharedData, FileType: C# script]", isValidateFunction: false)]
    public static void CreateStateSyncDataFromTemplate()
    {
        scriptBuilder(s_stateSharedDataTemplateFilepath, "NewStateSharedData.cs");
    }


    /// <summary>
    /// Method to create state synchronised input from template file
    /// </summary>
    [MenuItem(itemName: "Assets/Create/Fickle Frame Games/Create New State Sync Input From Template [Type: StateSyncInput, FileType: C# script", isValidateFunction: false)]
    public static void CreateStateSyncInputFromTemplate()
    {
        scriptBuilder(s_stateSyncInputTemplateFilepath, "NewStateSyncInput.cs");
    }


    /// <summary>
    /// Method to create custom action component
    /// </summary>
    [MenuItem(itemName: "Assets/Create/Fickle Frame Games/Create New Action Component [Type: ActionComponent, FileType: C# script]", isValidateFunction: false)]
    public static void CreateActionComponentFromTemplate()
    {
        scriptBuilder(s_actionSystemTemplateFilepath, "NewActionComponent.cs");
    }
}
