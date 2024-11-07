using UnityEngine;

public abstract class GameStateSO : ScriptableObject 
{
    [Space(10)]
    [Tooltip("Name of the state")]
    public string Name;
    public abstract void Handle();
    public abstract void StartActions();
    public abstract void EndActions();
    
    #region Validation
#if UNITY_EDITOR
    private void OnValidate() 
    {
        HelperUtilities.ValidateCheckEmptyString(this, nameof(Name), Name);
    }
#endif
    #endregion
}
