using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveGateScript : MonoBehaviour {

    private GameObject _gate;

    public bool EnemyDead { get; set; }

	// Use this for initialization
	void Start ()
    {
        _gate = this.gameObject;
        EnemyDead = false;
        //Debug.Log(_gate);
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(EnemyDead)
        { 
            Destroy(_gate);
        }
	}
}
