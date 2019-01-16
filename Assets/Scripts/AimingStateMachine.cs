using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimingStateMachine : StateMachineBehaviour
{

    private CharacterControlScript _characterControlScript;
    private void Awake()
    {
        _characterControlScript = GameObject.Find("MainCharacter").GetComponent<CharacterControlScript>();
    }
    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _characterControlScript.AllowMovement = true;
    }
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _characterControlScript.AllowMovement = false;
    }
}
