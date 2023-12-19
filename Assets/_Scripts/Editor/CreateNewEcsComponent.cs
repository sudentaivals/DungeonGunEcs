using UnityEditor;
public class CreateNewEcsComponent
{
 private const string pathToYourScriptTemplate = "Assets/_Scripts/ScriptTemplates/EcsComponentTemplate.cs.txt";
 
    [MenuItem(itemName: "Assets/Create/Create Leo-ECS-lite Component", isValidateFunction: false, priority: 51)]
    public static void CreateScriptFromTemplate()
    {
        ProjectWindowUtil.CreateScriptAssetFromTemplateFile(pathToYourScriptTemplate, "NewEcsComponent.cs");
    }
}
