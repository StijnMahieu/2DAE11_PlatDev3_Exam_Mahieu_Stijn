using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowRockStateMachine : StateMachineBehaviour {

    private RockThrowScript _rockThrowScript;

    public Transform PickUp { get; set; }

    private float _animatorCutTime = 0.265f;

    private void Awake()
    {
        _rockThrowScript = GameObject.Find("MainCharacter").GetComponent<RockThrowScript>();
    }
    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _rockThrowScript.AimToNormal();
    }
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= _animatorCutTime && animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1)
        {
            Debug.Log(animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
            _rockThrowScript.ThrowRock();
        }
    }
}
