using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardboardTriggerScript : MonoBehaviour
{
    private Animator _animator;

    private bool _isPushing;

    private GameObject _box;
    // Use this for initialization
    void Start()
    {
        _animator = GetComponent<Animator>();
        _isPushing = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        if (_isPushing)
            PushingBox();
    }
    private void OnTriggerStay(Collider _collision)
    {
        if (_collision.gameObject.tag == "CardboardTrigger")
        {
            if (Input.GetButtonDown("PushingBox") && this.gameObject.GetComponent<CharacterControlScript>().State == CharacterControlScript.States.normalMode)
            {
                this.gameObject.GetComponent<CharacterControlScript>().State = CharacterControlScript.States.pushingBox;
                //_collision.gameObject.transform.parent.parent = this.gameObject.transform;
                _isPushing = true;
                _box = _collision.gameObject.transform.parent.gameObject;
                this.gameObject.GetComponent<CharacterControlScript>().ChangePlayerForward(_box.transform);
                _animator.SetBool("PushingBox", true);
            }
            else if (Input.GetButtonDown("PushingBox") && this.gameObject.GetComponent<CharacterControlScript>().State == CharacterControlScript.States.pushingBox)
            {
                this.gameObject.GetComponent<CharacterControlScript>().State = CharacterControlScript.States.normalMode;
               // _collision.gameObject.transform.parent.parent = null;
                _isPushing = false;
                _animator.SetBool("PushingBox", false);
            }
        }
    }

    private void PushingBox()
    {
        _box.GetComponent<Rigidbody>().AddForce(this.gameObject.transform.forward * 12, ForceMode.Force);
    }
}
