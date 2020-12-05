using UnityEngine;

public class Sword : MonoBehaviour
{
    private int swordDamage;

    public void Start()
    {
        swordDamage = (int)GameObject.Find("Swordsman").GetComponent<Swordsman>().GetCurrAttack();
    }


    private void OnCollisionEnter2D(Collision2D hitInfo)
    {
        EnemyType enemy = hitInfo.gameObject.GetComponent<EnemyType>();
        if (enemy != null)
        {
            //Debug.Log("Enemy is hitted, dealing damage to enemy");
            enemy.TakeDamage(swordDamage);

        }
    }


}