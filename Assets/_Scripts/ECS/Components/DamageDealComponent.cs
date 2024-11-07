using System;
using System.Collections.Generic;
using Leopotam.EcsLite;

[Serializable]
public struct DamageDealComponent
{
    public List<DamageModificatorContainer> DamageModificators;
    public List<GameAction> ActionsOnDamageDeal;
}
