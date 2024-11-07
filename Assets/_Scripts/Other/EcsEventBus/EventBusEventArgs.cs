using System;
using System.Collections.Generic;
using AYellowpaper;
using CustomAstar;
using UnityEngine;

public class EventBusEventArgs
{

}

#region Skills

public class LearnSkillEventArgs : EventArgs
{
    public int SkillId {get; set;}

/*     public LearnSkillEventArgs(int skillId)
    {
        SkillId = skillId;
    } */
}

public class AddOrRemoveDamageModificatorEventArgs : EventArgs
{
    public DamageModificatorContainer DamageModificatorContainer {get; set;}
}

public class TakeDamageEventArgs : EventArgs
{
    public int DamageDealerEntity {get;set;}
    public DamageInformation DamageInformation {get;set;}
}

public class OnDamageTakeEventArgs : EventArgs
{
    public DamageInformation DamageInformation {get;set;}
    public int DamageDealerEntity {get;set;}
}

public class AddOrRemoveOnDamageTakeActionEventArgs : EventArgs
{
    public GameAction NewOnDamageTakeAction {get;set;}
}

public class OnDamageDealEventArgs : EventArgs
{
    public int DamageTakerEntity {get;set;}
    public DamageInformation DamageInformation {get;set;}
    public int FinalDamage {get;set;}
}

public class AddOrRemoveOnDamageDealActionEventArgs : EventArgs
{
    public GameAction DamageAction {get;set;}
}

public class UseSkillEventArgs : EventArgs
{
    public int SkillId {get; set;}

/*     public UseSkillEventArgs(int skillId)
    {
        SkillId = skillId;
    } */
}

public class PlayerUseSkillEventArgs : EventArgs
{
    public int SkillId {get; set;}

    public bool IsAutomatic {get; set;}

    public bool IsPressed {get; set;}

/*     public PlayerUseSkillEventArgs(int skillId, bool isAutomatic, bool isPressed)
    {
        SkillId = skillId;
        IsAutomatic = isAutomatic;
        IsPressed = isPressed;
    } */

}

public class SelectSkillEventArgs : EventArgs
{
    public int SkillId { get; set;}

    /*public SelectSkillEventArgs(int skillId)
    {
        SkillId = skillId;
    }
    */
}

public class RemoveSkillEventArgs : EventArgs
{
    public int SkillId { get; set;}

/*     public RemoveSkillEventArgs(int skillId)
    {
        SkillId = skillId;
    } */
}


#endregion

public class DealDamageEventArgs : EventArgs
{
    public DamageType DamageType {get;set;}
    public int InitialDamage {get;set;}
    public Guid DamageId {get;set;}
    public int DamageTakerEntity { get; set;}
}

public class SpawnEntityEventArgs : EventArgs
{
    public GameObject PrefabToSpawn { get; set;}
    public Vector3 Position { get; set;}
    public Quaternion Rotation { get; set;}

/*     public SpawnEntityEventArgs(GameObject prefabToSpawn, Vector3 position, Quaternion rotation)
    {
        PrefabToSpawn = prefabToSpawn;
        Position = position;
        Rotation = rotation;
    } */
}

public class AddStatusEffectEventArgs : EventArgs
{
    public StatusEffectShell EffectShell { get; set;}
    public int TargetEntity { get; set;}

    public float NewDuration;

    public bool OverrideDuration;
}

public class DealDamageSnapshot : EventArgs
{
    public GlobalStatsComponent SenderStats {get;}
    public int DamageTakerEntity { get; }
    public int Damage { get; }
    public DealDamageSnapshot(int damageTakerEntity, int damage, GlobalStatsComponent senderStats)
    {
        SenderStats = senderStats;
        DamageTakerEntity = damageTakerEntity;
        Damage = damage;
    }

}

public class PlaySoundEventArgs : EventArgs
{
    public AudioClip Sfx { get; set;}
    public float Volume { get; set;}

}

public class CreateFloatingTextEventArgs : EventArgs
{
    public string Text { get; }

    public Color Color { get; }

    public Vector2 Direction { get; }

    public float Lifetime { get; }

    public float Velocity { get; }

    public CreateFloatingTextEventArgs(string text, Color color, Vector2 direction, float lifetime = 0.4f, float velocity = 2f)
    {
        Text = text;
        Color = color;
        Direction = direction;
        Lifetime = lifetime;
        Velocity = velocity;
    }
}

public class AddColorEventArgs : EventArgs
{
    public Color Color { get; set;}
    public int TakerEntity { get; set;}
}

public class RemoveColorEventArgs : EventArgs
{
    public Color Color { get; set;}
    public int TakerEntity { get; set;}


/*     public RemoveColorEventArgs(Color color, int takerEntity)
    {
        Color = color;
        TakerEntity = takerEntity;
    } */
}

public class SetMovementEventArgs : EventArgs
{
    public bool NewMovementStatus { get; set;}

/*     public SetMovementEventArgs(bool newStatus)
    {
        NewMovementStatus = newStatus;
    } */
}

public class SetNpcMovementStatusEventArgs : EventArgs
{
    public bool NewNpcMovementStatus {get; set;}

/*     public SetNpcMovementStatusEventArgs(bool movementStatus)
    {
        NewNpcMovementStatus = movementStatus;
    } */
}

public class PlayAnimationEventArgs : EventArgs
{
    public Guid AnimationGuid {get; set;}
    public int AnimationId {get; set;}

    public float LockTime {get; set;}

    public float TargetAnimationSpeed {get; set;}

    public bool ChangeAnimationSpeed {get;set;}

    public bool IgnoreExitActions {get;set;}

}

public class PlayAnimationByNameEventArgs : EventArgs
{
    public string AnimationName {get; set;}

    public float LockTime {get; set;}

    public float TargetAnimationSpeed {get; set;}

    public bool ChangeAnimationSpeed {get;set;}

    public bool IgnoreExitActions {get;set;}
}

public class ReturnObjectToPoolEventArgs : EventArgs
{
    public int PoolId {get; set;}
}

public class TakeObjectFromPoolEventArgs : EventArgs
{
    public Vector3 Position {get; set;}

    public Quaternion Rotation {get; set;}

    public GameObject ObjectToSpawn {get; set;}

    public List<IMbHelperStatsSetter> StatsSetters {get;set;}
}

public class AddPushEventArgs : EventArgs
{
    public float PushPower {get; set;}
    public Vector2 Direction {get; set;}

    public bool IsImpulse;
}

public class AddSenderToCinemachineTargetGroupEventArgs : EventArgs
{
    public float Weight {get;set;}

    public float Radius {get;set;}
}

public class RotateObjectEventArgs : EventArgs
{
    public float RotationValue {get; set;}
}

public class SetRotationStatsEventArgs : EventArgs
{
    public bool IsClockwise {get;set;}
    public float RotationSpeed {get;set;}
}

public class ChangeMovementDirectionPatternEventArgs : EventArgs
{
    public InterfaceReference<IMovementDirection, ScriptableObject>  NewMovementDirectionPattern {get;set;}
}

public class ChangeMovementSpeedEventArgs : EventArgs
{
    public float SpeedModifier {get;set;}

    public bool RemoveModifier {get;set;}
}

public class ChangeImmuneStatusEventArgs : EventArgs
{
    public bool IsImmune {get;set;}
}

public class ChangePathfindingStatusEventArgs : EventArgs
{
    public bool IsPathfindingActive {get;set;}
}

public class ChangeSpriteEventArgs : EventArgs
{
    public Sprite Sprite {get;set;}
}

public class RegisterTimedActionEventArgs : EventArgs
{
    public int? TakerEntity {get;set;}
    public GameAction Action {get;set;}
    public float Timer {get;set;}
    public ConditionAndActionArgs Args {get;set;}
}

public class SetOutlineThicknessEventArgs : EventArgs
{
    public float Thickness {get;set;}
}

public class SetSfxVolumeEventArgs : EventArgs
{
    public float Volume {get;set;}
}

public class ChangeGameStateEventArgs : EventArgs
{
    public GameStateSO NewGameState {get;set;}
}

public class SetPathfindGridEventArgs : EventArgs
{
    public PathfindingGrid NewPathfindingGrid {get;set;}
}

public class ChangeActiveRoomEventArgs : EventArgs
{
    public GameObject NewRoom {get;set;}
    public List<Vector2> SpawnPoints {get;set;}
}

public class ChangeColliderStatusEventArgs : EventArgs
{
    public bool NewStatus {get;set;}
}

public class ChangeDoorStatusEventArgs : EventArgs
{
    public bool NewStatus {get;set;}
}

public class ChangeStatusEventArgs : EventArgs
{
    public bool NewStatus {get;set;}
}

