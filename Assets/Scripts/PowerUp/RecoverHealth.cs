using UnityEngine;

public class RecoverHealth : PowerUp
{

    protected override void PickUp(Collision2D player)
    {
        player.gameObject.GetComponent<PlayerController>().RecoverHealth();
    }


}