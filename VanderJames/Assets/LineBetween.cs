using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LineBetween : MonoBehaviour {
    public Transform[] transforms;
    public Vector3 offset;

    public LineRenderer lr;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        lr.SetPositions(PosArray());
	}

    private Vector3[] PosArray()
    {
        return transforms.Select(x => x.position + offset).ToArray();
    }
}
