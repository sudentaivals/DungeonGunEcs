using System;
using System.Collections.Generic;
using System.Linq;

public static class EventArgsObjectPool
{
    private static List<EventArgs> _eventArgsPool = new List<EventArgs>();

    public static T GetArgs<T>() where T : EventArgs, new()
    {
        var arg = _eventArgsPool.FirstOrDefault(a => a is T);
        if(arg == null)
        {
            arg = new T();
            _eventArgsPool.Add(arg);
            return arg as T;
        }
        return arg as T;

    }

    public static void ClearList()
    {
        _eventArgsPool.Clear();
    }
}
