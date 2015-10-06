using UnityEngine;
using System.Collections;

public class MusicalBouncyCube : MonoBehaviour {

    public float forceScale = 1f;
    private Rigidbody rb;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = Vector3.Lerp(transform.position, Vector3.up * forceScale * Maestro.current.GetFundamentalFrequency(), Time.deltaTime);

        // rb.AddForce(Vector3.up * forceScale * Maestro.current.GetFundamentalFrequency(), ForceMode.Impulse);
	}
}
