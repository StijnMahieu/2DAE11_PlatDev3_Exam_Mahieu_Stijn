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

	// Use this for initialization
	void Start ()
    {
        _isRockPickedUp = false;
        _isAiming = false;
        _firstPersonCamera.SetActive(false);
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(_isRockPickedUp && Input.GetButtonDown("AimingRock") && !_isAiming)
        {
            Debug.Log("Aiming");
            _isAiming = true;
            _firstPersonCamera.SetActive(true);
            _playerCamera.SetActive(false);
        }
        else if (_isRockPickedUp && Input.GetButtonDown("AimingRock") && _isAiming)
        {
            Debug.Log("Normal Mode");
            _isAiming = false;
            _firstPersonCamera.SetActive(false);
            _playerCamera.SetActive(true);
        }
    }
    private void OnTriggerStay(Collider _collision)
    {
        if (_collision.gameObject.tag == "RockBoxTrigger" + 0)
        {
            if(Input.GetButtonDown("RockPickup") && !_isRockPickedUp)
            {
                Debug.Log("Rock picked up");
                _isRockPickedUp = true;
            }
        }
        if (_collision.gameObject.tag == "RockBoxTrigger" + 1)
        {
            if (Input.GetButtonDown("RockPickup") && !_isRockPickedUp)
            {
                Debug.Log("Rock picked up");
                _isRockPickedUp = true;
            }
        }
    }
}
