using UnityEngine;


// Class extended by characters: swordsman, archer, mage
public abstract class PlayerType : MonoBehaviour
{
    public Transform attackPosition;
    protected ObjectPool pool;


    // 2d array (3 x 4) -- health, speed, attack, power up durr 
    // against defualt and current
    protected float[,] playerStats;

    public abstract void Attack();


    // Health variables -------------------
    public float GetHealthPercentage()
    {
        return playerStats[0, 1] / playerStats[0, 0];
    }


    public void LoseHealth(float amt)
    {
        playerStats[0, 1] -= amt;
        if (playerStats[0, 1] <= 0)
        {
            GameObject playerDead = transform.parent.gameObject;

            playerDead.GetComponent<PlayerController>().healthBar.SetActive(false);

            playerDead.SetActive(false);

            this.RecoverHealthPercent(.5f);

            if (playerDead.GetComponentInChildren<Mage>() != null)
                playerDead.GetComponentInChildren<Mage>().mageBar.SetActive(false);


            GameObject.Find("Canvas").GetComponent<BaseWorldCanvas>().EndGame(playerDead);
        }
    }


    public void RecoverHealthPercent(float percent)
    {
        float totalHealth = playerStats[0, 0] * percent + playerStats[0, 1];
        if (totalHealth >= playerStats[0, 0])
            playerStats[0, 1] = playerStats[0, 0];
        else playerStats[0, 1] = totalHealth;

    }


    public void UpgradeMaxHealthPercent()
    {
        playerStats[0, 0] *= playerStats[0, 2];
        playerStats[0, 1] *= playerStats[0, 2];
    }


    public void SetHealthDifficulty()
    {
        switch (PlayerPrefs.GetInt("Difficulty", 2))
        {
            case 1:
                playerStats[0, 0] *= 2;
                playerStats[0, 1] *= 2;
                break;

            case 3:
                playerStats[0, 0] *= .5f;
                playerStats[0, 1] *= .5f;
                break;
        }
    }


    // Speed variable -------------------
    public float GetCurrSpeed()
    {
        return playerStats[1, 1];
    }


    public void SetCurrSpeedPercent(float speed)
    {
        playerStats[1, 1] = playerStats[1, 0] * speed;
    }


    public void UpgradeSpeed()
    {
        playerStats[1, 0] *= playerStats[1, 2];
        ResetCurrSpeed();
    }


    public void ResetCurrSpeed()
    {
        playerStats[1, 1] = playerStats[1, 0];
    }


    // Attack variables -------------------
    public float GetCurrAttack()
    {
        return playerStats[2, 1];
    }


    public void IncreaseCurrAttack(float incAttack)
    {
        playerStats[2, 1] *= incAttack;
    }


    public float GetDefaultAttack()
    {
        return playerStats[2, 0];
    }


    public void ResetCurrAttack()
    {
        playerStats[2, 1] = playerStats[2, 0];
    }


    // Power Up variables-------------------
    public float GetPowerUpDuration()
    {
        return playerStats[3, 1];
    }


    public void UpgradePowerUpDuration()
    {
        playerStats[3, 1] *= playerStats[3, 2];
    }


}