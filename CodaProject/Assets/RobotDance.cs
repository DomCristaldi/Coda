using UnityEngine;
using System.Collections;

public class RobotDance : Coda.MusicBehaviour {

    Animator anim;

	// Use this for initialization
	protected override void Start () {
        base.Start();
        anim = GetComponent<Animator>();

    }

    // Update is called once per frame
    protected override void Update () {
	
	}

    public override void OnBeat() {
        int lastState = anim.GetInteger("PoseInt");
        int nextState = lastState;
        while(nextState == lastState) {
            nextState = Random.Range(0, 4);
        }
        anim.SetInteger("PoseInt", nextState);
    }
}
