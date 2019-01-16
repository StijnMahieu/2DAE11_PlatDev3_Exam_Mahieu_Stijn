using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowRockStateMachine : StateMachineBehaviour {

    private RockThrowScript _rockThrowScript;
    private CharacterControlScript _characterControlScript;

    private float _animatorCutTime = 0.265f;

    private void Awake()
    {
        _rockThrowScript = GameObject.Find("MainCharacter").GetComponent<RockThrowScript>();
        _characterControlScript = GameObject.Find("MainCharacter").GetComponent<CharacterControlScript>();
    }
    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _rockThrowScript.AimToNormal();
        _characterControlScript.AllowMovement = true;
    }
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _characterControlScript.AllowMovement = false;
    }
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        Debug.Log(animator.GetCurrentAnimatorStateInfo(0).normalizedTime);

        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= _animatorCutTime && animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1)
        {

        }
    }
}
