using UnityEditor;

public class CreateNewDamageModificator
{
 private const string pathToYourScriptTemplate = "Assets/_Scripts/ScriptTemplates/DamageModificatorTemplate.cs.txt";
 
    [MenuItem(itemName: "Assets/Create/Templates/Damage/Create new damage modificator", isValidateFunction: false, priority: 51)]
    public static void CreateScriptFromTemplate()
    {
        ProjectWindowUtil.CreateScriptAssetFromTemplateFile(pathToYourScriptTemplate, "NewDamageModificator.cs");
    }
}
