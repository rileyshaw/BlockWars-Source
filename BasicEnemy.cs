using UnityEngine;
using System.Collections;

public class BasicEnemy : MonoBehaviour {
	private GameObject player;
	private bool isfiring = false;
	private float damage;
	public float health;
	public int level = 1;
	private bool isDestroyed = false;
	private GameManager gamemanager;

	// Use this for initialization
	void Start () {
		health = Mathf.RoundToInt(3 * level * level * UnityEngine.Random.Range(.6f,3f));
		damage = Mathf.RoundToInt(level * level * UnityEngine.Random.Range(.6f,3f));
		player = GameObject.FindGameObjectWithTag("Player");
		gamemanager = GameObject.Find("GameManager").GetComponent<GameManager>(); 
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		transform.LookAt(player.transform);
		GetComponent<Rigidbody>().velocity = gameObject.transform.forward*15;
		if(isfiring == false){
			isfiring = true;
			StartCoroutine(firewait());
			GameObject go = Instantiate(Resources.Load("EnemyBullet"),new Vector3(transform.position.x, transform.position.y,transform.position.z), transform.rotation) as GameObject; 
			go.transform.GetChild(0).gameObject.GetComponent<EnemyBullet>().enemy = gameObject;
			go.transform.GetChild(0).gameObject.GetComponent<EnemyBullet>().damage = damage;
		}
	}
	void OnTriggerEnter(Collider other) {
		if(other.gameObject.tag == "Bullet"){
			takeDamage(other,other.gameObject.GetComponent<Bullet>().damage);
		}
	}
	public void takeDamage(Collider other, float damage){
		if(health > 0){
			health = health - damage;
			Instantiate(Resources.Load("BulletExplosion"),transform.position,transform.rotation);
			gamemanager.score += 10;
		}else if(!isDestroyed){
			GameObject drop = Instantiate(Resources.Load("ItemDrop"),transform.position, transform.rotation) as GameObject; 
			drop.GetComponent<ItemDrop>().item = gamemanager.createWeapon(level);
			Debug.Log(	drop.GetComponent<ItemDrop>().item);
			gamemanager.score += 10;
			isDestroyed = true;
			Destroy(other.gameObject.transform.parent.gameObject);
			Instantiate(Resources.Load("DeadBasicEnemy"),transform.position, transform.rotation); 
			gamemanager.currentLevel.enemiesRemaining--; 
			Destroy(gameObject);
		}
	}
	IEnumerator firewait() {
		yield return new WaitForSeconds(1f); 
		isfiring = false;
	}
}
