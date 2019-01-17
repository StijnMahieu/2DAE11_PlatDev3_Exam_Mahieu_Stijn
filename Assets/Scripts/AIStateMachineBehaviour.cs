using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStateMachineBehaviour : StateMachineBehaviour {

    public AIBehaviour AIBehaviour { get; set; }

    private float _weightCounter = 0;

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        AIBehaviour = animator.gameObject.GetComponent<AIBehaviour>();

        Vector3 aiLookPos = AIBehaviour.Player.transform.position + (Vector3.up * 1.5f);

        if (AIBehaviour.AiSeesPlayer)
        {
            SetLookAtPos(animator, aiLookPos);
            //Make AI point at character
            LerpHandIK(animator, aiLookPos);
        }
        else if (AIBehaviour.AiSeesPlayer == false)
        {
            //Reset right hand weight if AI loses track of player
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
            _weightCounter = 0;
        }
    }

    public void SetLookAtPos(Animator animator, Vector3 lookAt)
    {
        //When following player, make head follow player
        animator.SetLookAtWeight(1);
        animator.SetLookAtPosition(lookAt);
    }

    private void LerpHandIK(Animator animator, Vector3 aiLookPos)
    {
        _weightCounter += Time.deltaTime;
        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, _weightCounter);
        animator.SetIKPosition(AvatarIKGoal.RightHand, aiLookPos);
    }
}

