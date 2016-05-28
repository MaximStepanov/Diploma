using UnityEngine;
using System.Collections;

public class facingscreen : MonoBehaviour {
	public Transform target;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (target != null) {
			transform.LookAt(transform.position + target.transform.rotation * Vector3.forward,
				target.transform.rotation * Vector3.up);
			transform.Rotate (new Vector3 (-90, 0, 0));
		}
	}
}
