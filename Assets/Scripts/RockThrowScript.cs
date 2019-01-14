using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockThrowScript : MonoBehaviour {

    private bool _isRockPickedUp;
    private bool _isAiming;

    [SerializeField]
    private GameObject _firstPersonCamera;
    [SerializeField]
    private GameObject _playerCamera;

    private Animator _animator;

    private GameObject _rockBox;

    public GameObject ThrowableRock;

    // Use this for initialization
    void Start ()
    {
        _animator = GetComponent<Animator>();
        _isRockPickedUp = false;
        _isAiming = false;
        _firstPersonCamera.SetActive(false);
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(_isRockPickedUp && Input.GetButtonDown("AimingRock") && !_isAiming)
        {
            _isAiming = true;
            _animator.SetBool("Aiming", true);
            NormalToAim();
        }
        else if (_isRockPickedUp && Input.GetButtonDown("AimingRock") && _isAiming)
        {
            _isAiming = false;
            _animator.SetBool("Aiming", false);
            _animator.SetTrigger("NoThrow");
            AimToNormal();
        }
        ThrowRock();
    }
    private void OnTriggerStay(Collider _collision)
    {
        if (_collision.gameObject.tag == "RockBoxTrigger")
        {
            if(Input.GetButtonDown("RockPickup") && !_isRockPickedUp && this.gameObject.GetComponent<CharacterControlScript>().State == CharacterControlScript.States.normalMode)
            {
                Debug.Log("Rock picked up");
                _rockBox = _collision.gameObject.transform.parent.gameObject;
                ThrowableRock = _collision.gameObject.transform.GetComponent<GameObject>();
                this.gameObject.GetComponent<CharacterControlScript>().ChangePlayerForward(_rockBox.transform);
                _isRockPickedUp = true;
                this.gameObject.GetComponent<CharacterControlScript>().State = CharacterControlScript.States.holdingRock;
                _animator.SetTrigger("HoldingRock");
            }
        }
    }

    private void ThrowRock()
    {
        if(_isRockPickedUp && _isAiming)
        {
            if(Input.GetButtonDown("Throw"))
            {
                _animator.SetTrigger("Throw");
                _isRockPickedUp = false;
                _animator.SetBool("Aiming", false);
                this.gameObject.GetComponent<CharacterControlScript>().State = CharacterControlScript.States.normalMode;
            }
        }
    }
    private void NormalToAim()
    {
        _firstPersonCamera.SetActive(true);
        _playerCamera.SetActive(false);
    }

    public void AimToNormal()
    {
        _firstPersonCamera.SetActive(false);
        _playerCamera.SetActive(true);
        _animator.ResetTrigger("Throw");
        _isAiming = false;
    }

}
