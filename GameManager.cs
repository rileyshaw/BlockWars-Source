using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System;

public class GameManager : MonoBehaviour {
	public static int currentScreen;
	public int score = 0;
	public Player player;
	public EventSystem eventSystem;
	public Text infoText;
	public Text healthText;
	public Text fuelText;
	public Text highscoreText;
	public Text newWaveText;
	public Image healthImage;
	public Image fuelImage;
	private List<Enemy> enemies;
	public Level[] levels;
	private Wave[] tempWave;
	public Level currentLevel;              
	public Item[] weapons;     
	void Start () {

		GameObject.DontDestroyOnLoad(this);

		setup();
		eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
		player = new Player(this);
		player.addItem(createWeapon(1));
		player.addItem(createWeapon(2));
		player.addItem(createWeapon(3));
		player.addItem(createWeapon(4));
		player.addItem(createWeapon(5));
		player.addItem(createWeapon(6));
		player.addItem(createWeapon(7));
		player.addItem(createWeapon(10));
		player.addItem(createWeapon(15));
		player.addItem(createWeapon(20));
		currentScreen = 0;

	}
	public void OnOptionsClick(){
		
	}
	public void OnContinueClick(){
		currentScreen = 1;
		Camera.main.GetComponent<Animation>().Play ("MainToHangar");
		GameObject.Find("GameManager").GetComponent<GameManager>().currentLevel = GameObject.Find("GameManager").GetComponent<GameManager>().levels[0];
		//Application.LoadLevel(1);
	}
	public void OnNewGameClick(){

	}
	public void OnInventoryItemClick(){
		
	}
	public void OnMainMenuClick(){
		currentScreen = 0;
		Application.LoadLevel(0);
	}
	public void OnHangarClick(){
		currentScreen = 2;
		Camera.main.GetComponent<Animation>().Play ("HangarToEdit");
		GameObject.Find("Player").GetComponent<Animation>().Play ("PlayerToHangar");
	}
	public void OnEnterClick(){
		currentScreen = 3;
		Camera.main.GetComponent<Animation>().Play ("HangarToGame");
		GameObject.Find("Player").GetComponent<Animation>().Play ("PlayerToGame");
		GameObject.Find("HangarObj").transform.FindChild("Hangar").GetComponent<HangarController>().startlevel();
	}
	public void OnEditClick(){
		currentScreen = 1;
		Camera.main.GetComponent<Animation>().Play ("EditToHangar");
		GameObject.Find("Player").GetComponent<Animation>().Play ("PlayerFromEdit");
		GameObject.Find("Player").GetComponent<Animation>().PlayQueued("RotatePlayer",QueueMode.CompleteOthers);
	}
	public void OnQuitClick(){
		Application.Quit();
	}
	public void setup(){

		weapons = new Item[1];
		weapons[0] = new Item ("Weak Machine Gun", "Weak Machine Gun", .7f, .7f, .6f, .7f, .3f, .5f, 0);




		levels = new Level[100];
	
		tempWave = new Wave[5];


		enemies = new List<Enemy>();

		enemies = addEnemies(enemies,new Enemy((GameObject)Resources.Load("BasicEnemy"),1),10);
		tempWave[0] = new Wave(enemies,1,20f);
		enemies = new List<Enemy>();


		enemies = addEnemies(enemies,new Enemy((GameObject)Resources.Load("BasicEnemy"),1),15);
		tempWave[1] = new Wave(enemies,1.4f,25f);
		enemies = new List<Enemy>();

		enemies = addEnemies(enemies,new Enemy((GameObject)Resources.Load("BasicEnemy"),1),20);
		tempWave[2] = new Wave(enemies,1.5f,30f);
		enemies = new List<Enemy>();

		enemies = addEnemies(enemies,new Enemy((GameObject)Resources.Load("BasicEnemy"),2),30);
		tempWave[3] = new Wave(enemies,2f,30f);
		enemies = new List<Enemy>();

		enemies = addEnemies(enemies,new Enemy((GameObject)Resources.Load("BasicEnemy"),2),50);
		tempWave[4] = new Wave(enemies,2.4f,0);
		enemies = new List<Enemy>();

		levels[0] = new Level("Tutorial",tempWave,1, 0);

	}
	public List<Enemy> addEnemies(List<Enemy> previous,Enemy enemy, int amount){
		List<Enemy> temp = previous;
		for(int i = 0; i < amount; i++){
			temp.Add(enemy);
		}
		return temp;
	}
	public void death(){
		GameObject go = Instantiate(Resources.Load("Screens/GameOver")) as GameObject; 
		go.transform.FindChild("GameOverText").GetComponent<Text>().text = "You Died";
		GameObject.Find("Player").GetComponent<Rigidbody>().isKinematic = true;
		Screen.lockCursor = false;
//		Cursor.visible = true;
	}
	public Item createWeapon(int level){
		Item type = weapons[UnityEngine.Random.Range (0, weapons.Length-1)];
		int levelmodifyer = level * level;
		float accuracy = UnityEngine.Random.Range(type.accuracy-.1f,type.accuracy+.1f);
		float reloadrate = 3*UnityEngine.Random.Range(type.reloadrate-.1f,type.reloadrate+.1f);
		float firerate = level/3 * UnityEngine.Random.Range(type.firerate-.1f,type.firerate+.1f);
		int shotsperfire;
		if (type.shotsperfire == 0) {
			shotsperfire = 1;
		} else {
			shotsperfire = (int)(Math.Pow(level,1/3) * UnityEngine.Random.Range(type.shotsperfire-.1f,type.shotsperfire+.1f));
		}
		int clipsize = (int)(Math.Pow(level,2/5) *100* UnityEngine.Random.Range(type.clipsize-.1f,type.clipsize+.1f));
		int damage = (int)(levelmodifyer * UnityEngine.Random.Range(type.damage-.1f,type.damage +.1f));
		int energy = (int)(Math.Pow(level,2/3) * 10 *  UnityEngine.Random.Range(type.energy-.1f,type.energy +.1f));
		Item item = new Item (type.ingameObject, type.name, accuracy, reloadrate, firerate, clipsize, damage, energy, shotsperfire);
		return item;
	}
	public Item createWeapon(Item[] optionsofweapon,int level){
		return null;
	}
	public Item createWeapon(Item typeofweapon,int level){
		return null;
	}

}
public class Level{
	public Wave[] waves;
	public int originalEnemiesRemaining;
	public string name;
	public int currentWave = 0;
	public int enemyindex = 0;
	public int enemiesRemaining;
	private bool finished = false;
	public bool levelBeaten = false;
	public bool spawnWaiting = true;
	public Level(string name, Wave[] waves, float currentDifficulty, int highScore){
		this.name = name;
		this.waves = waves;
		for(int i = 0;i < waves.Length; i++){
			this.enemiesRemaining += waves[i].enemies.Count;
		}
		this.originalEnemiesRemaining = this.enemiesRemaining;
	}
	public Enemy getNextEnemy(){
		if(spawnWaiting == false && currentWave < waves.Length-1){
			spawnWaiting = true;
			enemyindex = 1;
			currentWave++;
			if(currentWave == waves.Length-1){

			}else{
				waves[currentWave].startTime = System.DateTime.Now.TimeOfDay.TotalSeconds;
			}
			if(getRemaining() <= 0){
 				finished = true;
			}
			return waves[currentWave].enemies[enemyindex];
		}else if(enemyindex < waves[currentWave].enemies.Count){
			enemyindex++;
			return waves[currentWave].enemies[enemyindex-1];
		}else{
			return null;
		}
	}
	public bool isFinished(){
		if(enemiesRemaining <= 0){
			finished = true;
			levelBeaten = true;
		}
		return finished;
	}
	public void resetLevel(){
		currentWave = 0;
		enemyindex = 0;
		spawnWaiting = true;
		finished = false;
		enemiesRemaining = originalEnemiesRemaining;

	}
	public double getTime(){
		if(waves[currentWave].timer - System.DateTime.Now.TimeOfDay.TotalSeconds  + waves[currentWave].startTime <= 0){
			spawnWaiting = false;
			return 0;
		}
		return waves[currentWave].timer - System.DateTime.Now.TimeOfDay.TotalSeconds  + waves[currentWave].startTime;
	}
	public int getRemaining(){
		return enemiesRemaining;
	}
}
public class Wave{
	public List<Enemy> enemies;
	public float rate;
	public double timer;
	public double startTime;
	public Wave(List<Enemy> enemies, float rate,float timer){
		this.enemies = enemies;
		this.rate = rate;
		this.timer = timer;
	}

}
public class Enemy{
	public GameObject enemyObject;
	public int level;
	public Enemy(GameObject enemyObject, int level){
		this.enemyObject = enemyObject;
		this.level = level;
	}
}
public class Item {
	public string name;
	public float accuracy;
	public float reloadrate;
	public float firerate;
	public int shotsperfire;
	public float clipsize;
	public float damage;
	public float energy;
	public string ingameObject;

	public Item(string ingameObject,String name, float accuracy, float reloadrate, float firerate, float clipsize, float damage, float energy, int shotsperfire){
		this.name= name;
		this.ingameObject = ingameObject;
		this.accuracy = accuracy;
		this.reloadrate = reloadrate;
		this.firerate = firerate;
		this.clipsize = clipsize;
		this.damage = damage;
		this.energy = energy;
		this.shotsperfire = shotsperfire;
	}
}
public class Player{
	string name;
	int level;
	int experience;
	int experienceneeded;
	float currenthealth;
	float currentfuel;
	float maxhealth;
	float maxfuel;
	GameManager gamemanager;
	decimal xpfactor = 1.05m;
	List<Item> inventory;
	public Item[,,] playerloadout;

	public Player(GameManager gamemanger){
		this.name = "";
		this.level = 1;
		this.experience = 0;
		this.currenthealth = 10f;
		this.currentfuel = 100f;
		this.maxhealth = 10f;
		this.maxfuel = 100f;
		this.experienceneeded = 50;
		this.gamemanager = gamemanger;
		inventory = new List<Item>();
		playerloadout = new Item[11,3,3];
	}
	public float getCurrentHealth(){
		return this.currenthealth;
	}
	public void addItem(Item item){
		this.inventory.Add(item);
	}
	public Item getItemFromInventory(int index){
		if(index < inventory.Count){
			return inventory[index];	
		}else{
			Debug.Log("Tried to Access a invalid index:" + index);
			return null;
		}
	}
	public void removeItemFromInventory(Item item){
		inventory.Remove(item);	
	}
	public string[] getAllInventoryNames(){
		string[] array = new string[inventory.Count];
		for(int i = 0; i < array.Length;i++){
			array[i] = inventory[i].name;
		}
		return array;
	}
	public float getCurrentFuel(){
		return this.currentfuel;
	}
	public float getExperience(){
		return this.experience;
	}
	public float getMaxHealth(){
		return this.maxhealth;
	}
	public float getMaxFuel(){
		return this.maxfuel;
	}
	public int getExperienceNeeded(){
		return this.experienceneeded;
	}
	public int getLevel(){
		return this.level;
	}
	public string getName(){
		return this.name;
	}
	public void setCurrentFuel(float val){
		this.currentfuel = val;
	}
	public void setCurrentHealth(float val){
		this.currenthealth = val;
	}
	public void setExperienceNeeded(int val){
		this.experienceneeded = val;
	}
	public void setMaxHealth(float val){
		this.maxhealth = val;
	}
	public void setMaxFuel(float val){
		this.maxfuel = val;
	}
	public void setExperience(int val){
		this.experience = val;
	}
	public void setLevel(int val){
		this.level = val;
	}
	public void setName(string val){
		this.name = val;
	}
	public void takeDamage(float damage){
		currenthealth -= damage;
		if(this.currenthealth <= 0){
			currenthealth = 0;
			this.death();
		}
	}
	public void updateHUD(){
		gamemanager.healthText.text = "Health: " + gamemanager.player.currenthealth + "/" + gamemanager.player.maxhealth;
		gamemanager.healthImage.rectTransform.sizeDelta = new Vector2((float)gamemanager.player.currenthealth / gamemanager.player.maxhealth * 200,20);
		if (isAlive ()) {
			gamemanager.infoText.text = "Wave: " + (gamemanager.currentLevel.currentWave + 1) + " of " + gamemanager.currentLevel.waves.Length + "\r\n" + "Time to Next Wave: " + Math.Round(gamemanager.currentLevel.getTime(),1) + "\r\n" + "Enemies Remaining: " + gamemanager.currentLevel.getRemaining();
			gamemanager.highscoreText.text = "Score: " + gamemanager.score;
			gamemanager.fuelImage.rectTransform.sizeDelta = new Vector2((float)gamemanager.player.currentfuel / gamemanager.player.maxfuel * 200,20);
			gamemanager.fuelText.text = "Fuel: " + (int)gamemanager.player.currentfuel + "/" + gamemanager.player.maxfuel;
			if ((int) gamemanager.currentLevel.getTime() <= 5 && gamemanager.currentLevel.getTime() >= 1) {
				gamemanager.newWaveText.text = "New Wave In " + (int) gamemanager.currentLevel.getTime();
			}else if((int) gamemanager.currentLevel.getTime() == 0) {
				gamemanager.newWaveText.text = "Wave " + (gamemanager.currentLevel.currentWave + 2);
			}else {
				gamemanager.newWaveText.text = "";
			}

		}
	}
	public bool isAlive(){
		return (currenthealth > 0);
	}
	public void death(){
		gamemanager.death();
	}
	public void addExperience(int val){
		this.experience += val;
		if(this.experienceneeded <= this.experience){
			this.levelUp();
		}
	}
	public void levelUp(){
		this.experience = this.experience - this.experienceneeded;
		this.level++;
		this.experienceneeded = (int)(this.experienceneeded * this.xpfactor);
		if(this.experience >= this.experienceneeded){
			this.levelUp ();
		}
	}
}

