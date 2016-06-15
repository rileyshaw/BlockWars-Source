using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
	public Rigidbody bulletrigid;
	public float damage = 1;
	// Use this for initialization
	void Start () {
		transform.Rotate(new Vector3(Random.Range(-3,3),0,Random.Range(-3,3)));
		GetComponent<Rigidbody>().velocity = new Vector3(transform.up.x,transform.up.y,transform.up.z) *140 ;
		StartCoroutine(deathwait());
	}
	IEnumerator deathwait()
	{
		yield return new WaitForSeconds(4f); 
		Destroy(gameObject.transform.parent.gameObject);
	}

}