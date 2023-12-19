using UnityEditor;

public class CreateNewGameCondition
{
    private const string pathToYourScriptTemplate = "Assets/_Scripts/ScriptTemplates/GameConditionTemplate.cs.txt";
 
    [MenuItem(itemName: "Assets/Create/Game condition template", isValidateFunction: false, priority: 51)]
    public static void CreateScriptFromTemplate()
    {
        ProjectWindowUtil.CreateScriptAssetFromTemplateFile(pathToYourScriptTemplate, "NewGameCondition.cs");
    }
}
