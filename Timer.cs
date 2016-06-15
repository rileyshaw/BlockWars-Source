using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour {
	public  float time = 1;
	// Use this for initialization
	void Start () {
		StartCoroutine(count());
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	IEnumerator count() {
		yield return new WaitForSeconds(time); 
		Destroy(gameObject);
	}
}
