using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HelperUtilities
{
    /// <summary>
    /// Empty string debug check
    /// </summary>
    public static bool ValidateCheckEmptyString(Object thisObject, string fieldName, string stringToCheck)
    {
        if(stringToCheck == "")
        {
            Debug.Log(fieldName + " is empty and must contain a value in object " + thisObject.name.ToString());
            return true;
        }
        return false;
    }
    
    /// <summary>
    /// List empty or contains null value check - return true if there is an error
    /// </summary>
    public static bool ValidateCheckEnumerableValues(Object thisObject, string fieldName, IEnumerable enumerable)
    {
        bool error = false;
        int count = 0;
        if(enumerable == null)
        {
            Debug.Log(fieldName + " is null in object " + thisObject.name.ToString());
            return true;
        }
        foreach (var item in enumerable)
        {
            if(item == null)
            {
                Debug.Log(fieldName + " has null values in object " + thisObject.name.ToString());
                error = true;
            }
            else
            {
                count++;
            }
        }
        if(count == 0)
        {
            Debug.Log(fieldName + " has no values in object " + thisObject.name.ToString());
            error = true;
        }
        return error;
    }
}
