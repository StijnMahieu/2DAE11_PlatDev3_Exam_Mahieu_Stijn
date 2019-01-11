using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardboardTriggerScript : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerStay(Collider _collision)
    {
        if (_collision.gameObject.tag == "CardboardTrigger")
        {
            if (Input.GetButtonDown("PushingBox") && this.gameObject.GetComponent<CharacterControlScript>().State == CharacterControlScript.States.normalMode)
            {
                this.gameObject.GetComponent<CharacterControlScript>().State = CharacterControlScript.States.pushingBox;
                _collision.gameObject.transform.parent.parent = this.gameObject.transform;
            }
            else if (Input.GetButtonDown("PushingBox") && this.gameObject.GetComponent<CharacterControlScript>().State == CharacterControlScript.States.pushingBox)
            {
                this.gameObject.GetComponent<CharacterControlScript>().State = CharacterControlScript.States.normalMode;
                _collision.gameObject.transform.parent.parent = null;
            }
        }
    }
}
