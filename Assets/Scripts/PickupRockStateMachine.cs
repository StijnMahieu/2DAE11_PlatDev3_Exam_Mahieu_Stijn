using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupRockStateMachine : StateMachineBehaviour {

    private CharacterControlScript _characterControlScript;
    private RockThrowScript _rockThrowScript;

    public Transform PickUp { get; set; }

    private float _lerpCount = 0.0f;
    private float _animatorCutTime = 0.36f;

    private void Awake()
    {
        _characterControlScript = GameObject.Find("MainCharacter").GetComponent<CharacterControlScript>();
        _rockThrowScript = GameObject.Find("MainCharacter").GetComponent<RockThrowScript>();
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

    private void SetPickupIK(Animator animator)
    {
        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, Mathf.Lerp(0, 1, _lerpCount));
        animator.SetIKRotationWeight(AvatarIKGoal.RightHand, Mathf.Lerp(0, 1, _lerpCount));
        animator.SetIKPosition(AvatarIKGoal.RightHand, PickUp.position);
        animator.SetIKRotation(AvatarIKGoal.RightHand, PickUp.rotation);

        if(_lerpCount<1.0f)
        {
            _lerpCount += Time.deltaTime;
        }
    }

    public override void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PickUp = _rockThrowScript.ThrowableRock.transform;
        if (PickUp != null)
        {
            //Debug.Log(animator.GetCurrentAnimatorStateInfo(0).normalizedTime);

            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= _animatorCutTime)
            {
                SetPickupIK(animator);
            }
            else if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > _animatorCutTime && animator.GetCurrentAnimatorStateInfo(0).normalizedTime <=1)
            {
                PickUp.parent = animator.GetBoneTransform(HumanBodyBones.RightHand);
                PickUp.position = animator.GetBoneTransform(HumanBodyBones.RightMiddleProximal).position;
            }
        }
    }
}
