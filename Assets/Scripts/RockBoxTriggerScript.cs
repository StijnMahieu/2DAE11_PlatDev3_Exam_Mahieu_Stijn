using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockBoxTriggerScript : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _rockPointers;

    // Use this for initialization
    void Start()
    {
        _rockPointers[0].SetActive(false);
        _rockPointers[1].SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider _collision)
    {
        if (_collision.gameObject.tag == "RockBoxTrigger" + 0)
        {
            _rockPointers[0].SetActive(true);
        }
        if (_collision.gameObject.tag == "RockBoxTrigger" + 1)
        {
            _rockPointers[1].SetActive(true);
        }
    }
}
