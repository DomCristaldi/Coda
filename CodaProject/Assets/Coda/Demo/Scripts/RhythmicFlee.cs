using UnityEngine;
using System.Collections;

public class RhythmicFlee : MonoBehaviour {

    public Rigidbody rb;
    public float force = 20f;

	// Use this for initialization
	void Awake () {
        rb = GetComponent<Rigidbody>();
        StartCoroutine(FleeOnNthBeat(2));
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private IEnumerator FleeOnNthBeat(int n)
    {
        // wait for nth beat
        int currBeatIndex = Coda.Maestro.current.closestBeatIndex;
        while (currBeatIndex % n != 0)
        {
            yield return Coda.Maestro.current.WaitForNextBeat();
            currBeatIndex = Coda.Maestro.current.closestBeatIndex;
        }
        Vector3 f = new Vector3(Random.value - 0.5f,
                                Random.value - 0.5f,
                                Random.value - 0.5f).normalized;

        rb.AddForce(f * force, ForceMode.Impulse);
        Destroy(gameObject, 3);
    }
}
