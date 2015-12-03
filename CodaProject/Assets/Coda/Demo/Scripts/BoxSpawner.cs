using UnityEngine;
using System.Collections;

public class BoxSpawner : MonoBehaviour {

    public GameObject proj;
    public int projsPerFrame = 3;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Random.value > .5)
        {
            for (int i = 0; i < projsPerFrame; i++)
            {
                Vector3 offset = Random.insideUnitSphere;
                GameObject p = Instantiate(proj, transform.position + offset, Quaternion.identity) as GameObject;
            }
            
        }
	}
}
