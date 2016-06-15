using UnityEngine;
using System.Collections;

public class Towers : MonoBehaviour {
	private GameManager gameManager;
	private Player player;
	public float health;
	// Use this for initialization
	void Start () {
		player = GameObject.Find("GameManager").GetComponent<GameManager>().player; 
		health = 100;
	}
	
	// Update is called once per frame
	void Update () {
		if (health <= 0)
			Destroy (gameObject);
	
	}
	void OnTriggerEnter(Collider other) {
		if(other.gameObject.tag == "EnemyBullet"){
			health = health - other.GetComponent<EnemyBullet>().damage;
			Destroy(other);
		}
	}
	void OnTriggerStay(Collider other){
		if (other.gameObject.tag == "Player") {
			PlayerMovement user = other.GetComponent<PlayerMovement> ();
			if(gameObject.tag == "Fuel"){
				other.GetComponent<PlayerMovement> ().hasfuel = true;
				other.GetComponent<PlayerMovement> ().fuelAdd = other.GetComponent<PlayerMovement> ().fuelAdd + (60*Time.deltaTime);
				if(other.GetComponent<PlayerMovement> ().fuelAdd >= 5) {
					other.GetComponent<PlayerMovement> ().fuelAdd = 0;
					if(player.getCurrentFuel() < player.getMaxFuel()){
						player.setCurrentFuel(player.getCurrentFuel() + 1);
					}
				}
			}	
			if(gameObject.tag == "Health"){
				user.healthAdd = user.healthAdd + (60*Time.deltaTime);
				if(user.healthAdd >= 60) {
					user.healthAdd = 0;
					if(player.getCurrentHealth() < player.getMaxHealth()){
						player.setCurrentHealth(player.getCurrentHealth() + 1);
					}
				}
			}
		}
	}
}