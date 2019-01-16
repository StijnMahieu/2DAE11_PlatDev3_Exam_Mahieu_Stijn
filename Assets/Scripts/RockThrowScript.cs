using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockThrowScript : MonoBehaviour {

    //bool
    private bool _isRockPickedUp;
    private bool _isAiming;

    //cameras
    [SerializeField]
    private GameObject _firstPersonCamera;
    [SerializeField]
    private GameObject _playerCamera;

    //rockprefab
    [SerializeField]
    private GameObject _rockPrefab;

    private Animator _animator;

    private GameObject _rockBox;
    public GameObject ThrowableRock;

    //spawn new rock here
    private Vector3 _rockSpawnPosition;

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
    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.tag == "RockBoxTrigger")
        {
            if(Input.GetButtonDown("RockPickup") && !_isRockPickedUp && this.gameObject.GetComponent<CharacterControlScript>().State == CharacterControlScript.States.normalMode)
            {
                //Debug.Log("Rock picked up");
                _rockBox = collision.gameObject.transform.gameObject;
                ThrowableRock = collision.gameObject.transform.GetChild(2).gameObject;
                _rockSpawnPosition = ThrowableRock.transform.position;
                this.gameObject.GetComponent<CharacterControlScript>().ChangePlayerForward(_rockBox.transform);
                _isRockPickedUp = true;

                this.gameObject.GetComponent<CharacterControlScript>().State = CharacterControlScript.States.holdingRock;
                _animator.SetTrigger("HoldingRock");
            }
        }
    }

    public void ThrowRock()
    {
        if(_isRockPickedUp && _isAiming)
        {
            if(Input.GetButtonDown("Throw"))
            {
                _animator.SetTrigger("Throw");
                _isRockPickedUp = false;
                _animator.SetBool("Aiming", false);
                this.gameObject.GetComponent<CharacterControlScript>().State = CharacterControlScript.States.normalMode;
                ThrowingPhysics();
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

        //instantiate new rock
        Instantiate<GameObject>(_rockPrefab, _rockSpawnPosition, Quaternion.identity).transform.SetParent(_rockBox.transform);
    }

    public void ThrowingPhysics()
    {
        ThrowableRock.transform.parent = null;
        ThrowableRock.GetComponent<Rigidbody>().isKinematic = false;
        ThrowableRock.GetComponent<Rigidbody>().AddForce(this.gameObject.transform.Find("PlayerPivot").forward * 12, ForceMode.Impulse);
    }
}
