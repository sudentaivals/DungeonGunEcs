public enum GameplayEventType
{
    GameOver,
    Pause,
    Unpause,
    Restart,
    Victory,
    PlaySound,
    LevelLoading,
    //skills
    ObjectLearnSkill,
    ObjectRemoveSkill,
    ObjectUseSkill,

    NpcUseSkill,

    PlayerUseSkill,
    ObjectSelectSKill,
    DestroyObject,
    SpawnObject,
    DealDamage,
    AddStatusEffect,
    CreateDamageFloatingText,
    CreateHealFloatingText,
    CreateEvadeFloatingText,
    CreateFloatingText,
    //materials
    AddColor,
    RemoveColor,
    //Movement
    SetMovementStatus,
    ChangeNpcMovement,
    //target
    FindNewTarget,
    RemoveTarget,
    ChangeAnimation,
    ReturnObjectToPool,
    TakeObjectFromPool,
    AddPush,
    RemovePush,
    RemoveSenderFromCinemachineGroup,
    AddSenderToCinemachineGroup,
    RotateObject,
    ResetRotation,
    ActivateSpecialMovement,
    SetRotationStats,
    RemoveMovement,
    ChangeDirectionPattern,
    ChangeMovementSpeed,
    RemoveAllStatusEffects,
    SetImmuneStatus,
}