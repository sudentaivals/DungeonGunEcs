using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/Conditions/Player can pick up item")]
public class PlayerCanPickupItemCondition : BaseGameCondition
{
    [SerializeField] int _itemId;
    public override bool CheckCondition(int senderEntity, int? takerEntity)
    {
        /*var playerItems = taker.GetComponent<PlayerItemSystem>();
        if (playerItems)
        {
            return playerItems.IsPlayerHaveFreeSlots || (playerItems.GetItemById(_itemId, out var item) && playerItems.IsItemCanGetMoreStacks(_itemId));
        }
        return false;
        */
        return true;
    }
}
