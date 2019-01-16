using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveGateScript : MonoBehaviour {

    private GameObject _gate;

    private bool _enemyDead;

	// Use this for initialization
	void Start ()
    {
        _gate = this.gameObject;
        Debug.Log(_gate);
        _enemyDead = true;
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(_enemyDead)
        {
            Destroy(_gate);
        }
	}
}
