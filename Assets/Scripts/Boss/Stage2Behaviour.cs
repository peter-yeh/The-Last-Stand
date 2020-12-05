using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage2Behaviour : StateMachineBehaviour
{
    private int rand;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rand = Random.Range(0, 3);

        if (rand == 0)
        {
            animator.SetTrigger("Jump");
        }
        else if (rand == 1)
        {
            animator.SetTrigger("Attack");
        }
        else if (rand == 2)
        {
            animator.SetTrigger("Dash");
        }
    }


    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }


    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
