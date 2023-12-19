using System;
using System.Collections.Generic;

[Serializable]
public struct EventsComponent
{
    public List<GameAction> OnDamageTake;
    public List<GameAction> OnDamageDeal;
    public List<GameAction> OnSkillUse;
    public List<GameAction> OnDeath;
    public List<GameAction> OnEvasion;

    public List<GameAction> OnStart;

    public bool StartEventsInvoked;
}
