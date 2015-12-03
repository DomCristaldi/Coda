using UnityEngine;
using System.Collections;

using Coda;

public class DissolveController : MusicBehaviour {

    Renderer rend;

    public float minDissolveAmount;
    public float maxDissolveAmount;
    public bool buildingUp;

    /*
    public float stableDissolveAmount;
    public float desiredDissolveAmount;
    */
     public float dissolveDelta;
    
    public float trueDissolveAmount;

    protected override void Awake() {
        base.Awake();

        rend = GetComponent<Renderer>();

        buildingUp = true;
    }

	// Use this for initialization
	protected override void Start () {
        base.Start();

	}
	
	// Update is called once per frame
	protected override void Update () {
        base.Update();

        UpdateDisolveAmount();
        UpdateMaterial();
	}

    public override void OnBeat() {
        base.OnBeat();

        buildingUp = !buildingUp;
    }

    private void UpdateDisolveAmount() {
        //trueDissolveAmount = Mathf.MoveTowards(trueDissolveAmount, stableDissolveAmount, dissolveDelta);
        trueDissolveAmount = Mathf.MoveTowards(trueDissolveAmount,
                                               buildingUp ? maxDissolveAmount : minDissolveAmount,
                                               dissolveDelta);
    }

    private void UpdateMaterial() {
        rend.material.SetFloat("_SliceAmount", trueDissolveAmount);
    }
}
