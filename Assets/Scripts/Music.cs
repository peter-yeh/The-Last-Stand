using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{

    public AudioSource mainTheme;
    public AudioSource bossMusic;

    // Start is called before the first frame update
    void Start()
    {
        //mainTheme.Play();
    }

    // Update is called once per frame
    public void playBossMusic()
    {
        mainTheme.Stop();
        bossMusic.pitch = 1;
        bossMusic.Play();
    }

    public void playBossMusicStage2()
    {
        bossMusic.pitch = 1.25f;
    }

    public void playMainTheme()
    {
        bossMusic.Stop();
        mainTheme.Play();

    }
}
