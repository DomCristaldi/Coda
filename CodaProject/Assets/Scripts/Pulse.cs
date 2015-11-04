using UnityEngine;
using System.Collections;
using Coda;

public class Pulse : MonoBehaviour {

    public float scale = 0.01f;
    public float scaleSpeed = 5f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one * Maestro.current.GetFundamentalFrequency() * scale, Time.deltaTime * scaleSpeed);
	}
}
