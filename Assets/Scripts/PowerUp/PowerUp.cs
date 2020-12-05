using UnityEngine;

public abstract class PowerUp : MonoBehaviour
{

    //public GameObject pickUpEffect;
    

    private void Start()
    {
        //pickUpSound = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(("Player")))
        {
            


            PickUp(collision);
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().PlayPowerUpSound();


            // These would be deleted after slowed pwoeruup is setttled
            //GetComponent<Collider2D>().enabled = false;
            //GetComponent<SpriteRenderer>().enabled = false;

            GameObject.Find("PowerUpSpawner").GetComponent<PowerUpSpawner>().RemovePowerUp(gameObject);

            //StartCoroutine(GameObject.Find("ObjectPool").GetComponent<ObjectPool>().LatePutBackInPool(gameObject, 0.1f));



        }
    }


    protected abstract void PickUp(Collision2D player);


}