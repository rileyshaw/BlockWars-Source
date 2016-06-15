using UnityEngine;
using System.Collections;

public class ItemDrop : MonoBehaviour {
	public Item item;
	private GameManager gamemanager;
	// Use this for initialization
	void Start () {
		gamemanager = GameObject.Find ("GameManager").GetComponent<GameManager> ();
	}
	
	void OnCollisionEnter(Collision other) {
		if(other.collider.tag == "Player"){
			gamemanager.player.addItem(item);
			Destroy(gameObject);
		}
	}
}
