
using UnityEditor;

public class CreateNewEcsComponentProvider
{
 private const string pathToYourScriptTemplate = "Assets/_Scripts/ScriptTemplates/EcsComponentProviderTemplate.cs.txt";
 
    [MenuItem(itemName: "Assets/Create/Create Leo-ECS-lite Component-Provider", isValidateFunction: false, priority: 51)]
    public static void CreateScriptFromTemplate()
    {
        ProjectWindowUtil.CreateScriptAssetFromTemplateFile(pathToYourScriptTemplate, "NewEcsComponentProvider.cs");
    }
}
