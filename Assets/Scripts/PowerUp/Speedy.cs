using UnityEngine;

public class Speedy : PowerUp
{

    protected override void PickUp(Collision2D player)
    {
        player.gameObject.GetComponent<PlayerController>().Speedy();
    }

}