using UnityEditor;

public class CreateNewEcsSystem
{
    private const string pathToYourScriptTemplate = "Assets/_Scripts/ScriptTemplates/EcsSystemTemplate.cs.txt";
 
    [MenuItem(itemName: "Assets/Create/Create Leo-ECS-lite System", isValidateFunction: false, priority: 51)]
    public static void CreateScriptFromTemplate()
    {
        ProjectWindowUtil.CreateScriptAssetFromTemplateFile(pathToYourScriptTemplate, "NewEcsSystem.cs");
    }
}
