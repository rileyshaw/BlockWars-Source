using UnityEngine;
using System.Collections;

public class EnemyBullet : MonoBehaviour {
	private Rigidbody bulletrigid;
	public GameObject enemy;
	public float damage;
	// Use this for initialization
	void Start () {
		GetComponent<Rigidbody>().velocity = enemy.transform.forward*60;
		StartCoroutine(deathwait());
	}
	IEnumerator deathwait(){
		yield return new WaitForSeconds(4f); 
		Destroy(gameObject.transform.parent.gameObject);
	}
}
