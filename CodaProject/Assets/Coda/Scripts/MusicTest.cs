using UnityEngine;
using System.Collections;

public class MusicTest : Coda.MusicBehaviour {

    /*// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}*/

    public override void OnBeat() {
        transform.rotation *= Quaternion.Euler(new Vector3(5,0,0));
    }
}
