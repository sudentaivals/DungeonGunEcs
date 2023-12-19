using UnityEditor;

public class CreateNewGameAction
{
    private const string pathToYourScriptTemplate = "Assets/_Scripts/ScriptTemplates/GameActionTemplate.cs.txt";
 
    [MenuItem(itemName: "Assets/Create/Game action template", isValidateFunction: false, priority: 51)]
    public static void CreateScriptFromTemplate()
    {
        ProjectWindowUtil.CreateScriptAssetFromTemplateFile(pathToYourScriptTemplate, "NewGameAction.cs");
    }
}
