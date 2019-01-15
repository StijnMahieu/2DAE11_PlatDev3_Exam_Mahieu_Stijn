using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimingArchScript : MonoBehaviour {

    //inspector private vars
    [SerializeField] private float _maxLength;
    [SerializeField] private float _segmentLength;
    //private components
    private LineRenderer _line;
    //private vars
    [SerializeField]
    private Vector3 _directionVelocity;
    //properties
    public bool DrawParabola {get; set;}

	// Use this for initialization
	void Start () {
        _line = GetComponent<LineRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        
            //draw parabola
            Vector3 prev = transform.position;
            _line.positionCount = 1;
            _line.SetPosition(0, transform.position);

            for (int i = 1; ; i++)
                {
                float t = _segmentLength * i;
                if (t > _maxLength) break;
                Vector3 pos = PlotTrajectoryAtTime(transform.position, _directionVelocity, t);
                if (Physics.Linecast(prev, pos, LayerMask.GetMask("Default"))) break;
                _line.positionCount++;
                _line.SetPosition(_line.positionCount - 1, pos);
                }
            
        }

    private Vector3 PlotTrajectoryAtTime(Vector3 start, Vector3 startVelocity, float time)
        {
        return start + startVelocity * time + Physics.gravity * time * time * 0.5f;
        }
    }
