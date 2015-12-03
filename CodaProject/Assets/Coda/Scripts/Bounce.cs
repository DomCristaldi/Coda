using UnityEngine;
using System.Collections;

public class Bounce : Coda.MusicBehaviour {

    Rigidbody rb;
    public int forceMult = 30;
    public int downBeat = 0;

	// Use this for initialization
	protected override void Start () {
        base.Start();
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	protected override void Update () {

    }

    public override void OnBeat() {

        double energy = Coda.Maestro.current.closestBeat.energy;
        if (Coda.Maestro.current.closestBeatIndex % 2 == downBeat) {
            //print(energy);
            rb.AddForce(Vector3.up * (float)energy * forceMult, ForceMode.Impulse);
        }

    }
}
