using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour {
	private bool spawned = false;
	public int level = 1;
	private Player player;
	public Level currentLevel;
	private GameObject[] spawns;
	private GameManager gameManager;
	private GameObject playerobject;
	private bool exploded = false;
	void Start () {
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>(); 
		gameManager.eventSystem =  GameObject.Find("EventSystem").GetComponent<EventSystem>();
		gameManager.infoText = GameObject.Find("InGameHUD").transform.FindChild("InfoText").GetComponent<Text>();
		gameManager.healthText = GameObject.Find("InGameHUD").transform.FindChild("HealthCurrentLabel").GetComponent<Text>();
		gameManager.fuelText = GameObject.Find("InGameHUD").transform.FindChild("FuelText").GetComponent<Text>();
		gameManager.newWaveText = GameObject.Find("InGameHUD").transform.FindChild("NewWaveText").GetComponent<Text>();
		gameManager.highscoreText = GameObject.Find("InGameHUD").transform.FindChild("HighScoreText").GetComponent<Text>();
		gameManager.healthImage = GameObject.Find("InGameHUD").transform.FindChild("HealthCurrentImage").GetComponent<Image>();
		gameManager.fuelImage = GameObject.Find("InGameHUD").transform.FindChild("FuelCurrentImage").GetComponent<Image>();
		playerobject = GameObject.Find("Player");
		gameManager.currentLevel.resetLevel();
		gameManager.currentLevel.waves[gameManager.currentLevel.currentWave].startTime = (System.DateTime.Now.TimeOfDay.TotalSeconds);
		player = gameManager.player;
		player.setCurrentHealth(player.getMaxHealth());
		player.setCurrentFuel(player.getMaxFuel());
		currentLevel = gameManager.currentLevel;
		gameManager.score = 0;
		spawns = GameObject.FindGameObjectsWithTag("Spawn");
		Item[,,] temp  = gameManager.player.playerloadout;
//		Cursor.visible = false;
		Debug.Log("Beginning");
		int guncount = 0;
		playerobject.GetComponent<PlayerMovement>().guns = new Gun[100];
		for(int x = 0;x < 11;x++){
			for(int y = 0; y < 3;y++){
				for(int z = 0; z < 3;z++){
					if(temp[x,y,z] != null){

						GameObject item = Instantiate(Resources.Load ("WeaponObjects/" +temp[x,y,z].ingameObject),new Vector3(0,0,0), Quaternion.identity) as GameObject; 
						item.transform.SetParent(playerobject.transform);
						item.transform.localPosition = new Vector3(x-5,y,z-1);
						item.transform.GetComponent<ItemInfo>().itempositionx = x;
						item.transform.GetComponent<ItemInfo>().itempositiony = y;
						item.transform.GetComponent<ItemInfo>().itempositionz = z;
						item.transform.GetComponent<ItemInfo>().item = temp[x,y,z];
						if(temp[x,y,z].firerate != -1){
							playerobject.GetComponent<PlayerMovement>().guns[guncount] = item.GetComponent<Gun>();
							item.GetComponent<Gun>().weapon = temp[x,y,z];
							guncount++;
						}
					}
				}
			}
		}
	}
	void FixedUpdate(){
		player.updateHUD ();
			if (!player.isAlive () && !exploded) {
				exploded = true;
				GameObject.Find ("Player").transform.localScale = new Vector3 (0.001f, 0.001f, 0.001f);
				Instantiate(Resources.Load("DeadPlayer"),GameObject.Find ("Player").transform.position, GameObject.Find ("Player").transform.rotation);
			}
		}
	void Update(){
		if(player.isAlive() && !currentLevel.isFinished()){
			if(spawned == false){
				spawned = true;
				StartCoroutine(spawnwait(currentLevel.getNextEnemy()));
			}
		}else if(currentLevel.isFinished()){
			GameObject go = Instantiate(Resources.Load("Screens/GameOver")) as GameObject; 
			go.transform.FindChild("GameOverText").GetComponent<Text>().text = "Level Completed";
			GameObject.Find("Player").GetComponent<Rigidbody>().isKinematic = true;
			Screen.lockCursor = false;
//			Cursor.visible = true;
		}
	}
	IEnumerator spawnwait(Enemy enemy){
		yield return new WaitForSeconds(1/currentLevel.waves[currentLevel.currentWave].rate); 
		spawned = false;
		if(enemy != null){
			GameObject thisenemy = Instantiate(enemy.enemyObject,spawns[Random.Range(0,3)].transform.position,transform.rotation) as GameObject; 
			if(enemy.enemyObject.name == "BasicEnemy"){
				thisenemy.GetComponent<BasicEnemy>().level = enemy.level;
			}
		}
	}
	void levelComplete(){

	}
}
