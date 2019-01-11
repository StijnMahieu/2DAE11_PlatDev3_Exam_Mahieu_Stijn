using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardboardTriggerScript : MonoBehaviour
{
    private bool _isPushing;

    // Use this for initialization
    void Start()
    {
        _isPushing = false;
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerStay(Collider _collision)
    {
        if (_collision.gameObject.tag == "CardboardTrigger")
        {
            if (Input.GetButtonDown("PushingBox") && !_isPushing)
            {
                _isPushing = true;
                this.gameObject.GetComponent<CharacterControlScript>().State = CharacterControlScript.States.pushingBox;
            }
            if (Input.GetButtonDown("PushingBox") && _isPushing)
            {
                _isPushing = false;
                this.gameObject.GetComponent<CharacterControlScript>().State = CharacterControlScript.States.normalMode;
            }
        }
    }
}
