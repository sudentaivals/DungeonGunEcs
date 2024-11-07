using System.Collections.Generic;
using System.Linq;

public class ConditionAndActionArgs
{

}

public static class ConditionAndActionArgsPool
{
    private static List<ConditionAndActionArgs> _conditionAndActionArgsPool = new ();

    public static T GetArgs<T>() where T : ConditionAndActionArgs, new()
    {
        var arg = _conditionAndActionArgsPool.FirstOrDefault(a => a is T);
        if(arg == null)
        {
            arg = new T();
            _conditionAndActionArgsPool.Add(arg);
            return arg as T;
        }
        return arg as T;

    }
}

public class DamageArgs : ConditionAndActionArgs
{
    public DamageInformation DamageInformation {get;set;}
}


