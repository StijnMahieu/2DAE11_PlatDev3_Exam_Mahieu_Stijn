using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowRockStateMachine : StateMachineBehaviour {

    private RockThrowScript _rockThrowScript;
    private CharacterControlScript _characterControlScript;
    private void Awake()
    {
        _rockThrowScript = GameObject.Find("MainCharacter").GetComponent<RockThrowScript>();
        _characterControlScript = GameObject.Find("MainCharacter").GetComponent<CharacterControlScript>();
    }
    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _rockThrowScript.AimToNormal();
    }
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
