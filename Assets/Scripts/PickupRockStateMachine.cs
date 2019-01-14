using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupRockStateMachine : StateMachineBehaviour {

    private CharacterControlScript _characterControlScript;
    private void Awake()
    {
        _characterControlScript = GameObject.Find("MainCharacter").GetComponent<CharacterControlScript>();
    }
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _characterControlScript.AllowMovement = false;
        _characterControlScript.AllowCameraRotation = false;
    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _characterControlScript.AllowMovement = true;
        _characterControlScript.AllowCameraRotation = true;
    }
}
