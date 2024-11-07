using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDeepClonable<T>
{
    T DeepClone();
}
