using UnityEngine;
using System.Collections;
using Coda;

public class MusicalBouncyCube : MonoBehaviour {

    public float forceScale = 1f;
    private Rigidbody rb;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {

	}
}
