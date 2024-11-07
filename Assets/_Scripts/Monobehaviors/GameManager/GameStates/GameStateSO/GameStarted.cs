using UnityEngine;

[CreateAssetMenu(fileName = "GameStarted", menuName = "My Assets/GameState/Game started")]
public class GameStarted : GameStateSO
{
    public override void EndActions()
    {
        
    }

    public override void Handle()
    {
        
    }

    public override void StartActions()
    {
        EcsEventBus.Publish(GameplayEventType.PlayDungeonLevel, 0, null);

        var gameStateSo = GameResources.Instance.GameStateList.GetGameStateByName("PlayingLevel");
        var args = EventArgsObjectPool.GetArgs<ChangeGameStateEventArgs>();
        args.NewGameState = gameStateSo;
        EcsEventBus.Publish(GameplayEventType.ChangeGameState, 0, args);
    }
}
