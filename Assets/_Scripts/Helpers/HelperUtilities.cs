using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.EventSystems;
using Object = UnityEngine.Object;
public static class HelperUtilities
{
    /// <summary>
    /// Empty string debug check
    /// </summary>
    public static bool ValidateCheckEmptyString(Object thisObject, string fieldName, string stringToCheck)
    {
        if(stringToCheck == "")
        {
            UnityEngine.Debug.Log(fieldName + " is empty and must contain a value in object " + thisObject.name.ToString());
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
            UnityEngine.Debug.Log(fieldName + " is null in object " + thisObject.name.ToString());
            return true;
        }
        foreach (var item in enumerable)
        {
            if(item == null)
            {
                UnityEngine.Debug.Log(fieldName + " has null values in object " + thisObject.name.ToString());
                error = true;
            }
            else
            {
                count++;
            }
        }
        if(count == 0)
        {
            UnityEngine.Debug.Log(fieldName + " has no values in object " + thisObject.name.ToString());
            error = true;
        }
        return error;
    }

    private static readonly Dictionary<float, WaitForSeconds> WaitDictionary = new();
    /// <summary>
    /// Returns a wait for the specified time for coroutines, non-allocated
    /// </summary>
    public static WaitForSeconds GetWait(float time)
    {
        if(WaitDictionary.TryGetValue(time, out var wait)) return wait;
        WaitDictionary[time] = new WaitForSeconds(time);
        return WaitDictionary[time];
    }

    private static PointerEventData _eventDataCurrentPosition;
    private static List<RaycastResult> _results;
    /// <summary>
    /// Returns true if the mouse is over a UI element
    /// </summary>
    public static bool IsOverUi()
    {
        _eventDataCurrentPosition = new PointerEventData(EventSystem.current) {position = Input.mousePosition};
        _results = new();
        EventSystem.current.RaycastAll(_eventDataCurrentPosition, _results);
        return _results.Count > 0;
    }

    /// <summary>
    /// Destroys all children of the transform
    /// </summary>
    public static void DeleteChildren(this Transform parent)
    {
        foreach (Transform child in parent) UnityEngine.Object.Destroy(child.gameObject);
    }

    /// <summary>
    /// Measure performance of functions
    /// </summary>
    public static void MeasurePerformace<T>(Func<T> func, int measureCount = 100000)
    {
        Stopwatch sw = new();
        sw.Start();
        for (int i = 0; i < measureCount; i++)
        {
            T result = func();
        }
        sw.Stop();
        UnityEngine.Debug.Log("Performance: " + sw.ElapsedMilliseconds + "ms");
    }
}
