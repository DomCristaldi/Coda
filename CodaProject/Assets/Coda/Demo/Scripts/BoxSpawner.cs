using UnityEngine;
using System.Collections;

public class BoxSpawner : MonoBehaviour {

    public GameObject proj;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Random.value > .5)
        {
            Vector3 offset = Random.insideUnitSphere;
            GameObject i = Instantiate(proj, transform.position + offset, Quaternion.identity) as GameObject;
        }
	}
}
