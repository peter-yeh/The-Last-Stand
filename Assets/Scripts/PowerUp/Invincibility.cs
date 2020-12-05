using UnityEngine;

public class Invincibility : PowerUp
{

    protected override void PickUp(Collision2D player)
    {
        player.gameObject.GetComponent<PlayerController>().Invincibility();
    }


}