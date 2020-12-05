using UnityEngine;

public class IncreaseAttack : PowerUp
{
    protected override void PickUp(Collision2D player)
    {
        player.gameObject.GetComponent<PlayerController>().IncreaseAttack();
    }


}