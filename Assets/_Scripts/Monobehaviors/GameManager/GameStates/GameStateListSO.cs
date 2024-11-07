using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameStateList", menuName = "My Assets/GameState/Game state list")]
public class GameStateListSO : ScriptableObject
{
    public List<GameStateSO> List;

    public GameStateSO GetGameStateByName(string name)
    {
        var state = List.Find(x => x.Name == name);
        return state;
    }

    #region Validation
#if UNITY_EDITOR
    private void OnValidate()
    {
        HelperUtilities.ValidateCheckEnumerableValues(this, nameof(List), List);
    }
#endif
    #endregion

}
