using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CreateNewCustonDamageVariable : MonoBehaviour
{
 private const string pathToYourScriptTemplate = "Assets/_Scripts/ScriptTemplates/CustomDamageVariableTemplate.cs.txt";
 
    [MenuItem(itemName: "Assets/Create/Templates/Damage/Create new custom damage variable", isValidateFunction: false, priority: 51)]
    public static void CreateScriptFromTemplate()
    {
        ProjectWindowUtil.CreateScriptAssetFromTemplateFile(pathToYourScriptTemplate, "NewCustomDamageVariable.cs");
    }
}
