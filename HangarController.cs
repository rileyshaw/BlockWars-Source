using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System;

public class HangarController : MonoBehaviour {
	private GameObject playerobject;
	private Player player;
	private GameManager gamemanager;
	private GameObject hangar;
	private GameObject inventorypanel;
	private GameObject infopanel;
	public Texture common;
	public Texture ordinary;
	public Texture uncommon;
	public Texture rare;
	public Texture legendary;
	public Item currentfocus;
	public GameObject currentobjectfocus;
	private GameObject currentbuttonclicked;
	public int currentlevelfocus = 1;
	// Use this for initialization
	void Start () {
		playerobject = GameObject.Find("Player");
		gamemanager = GameObject.Find("GameManager").GetComponent<GameManager>();
		player = gamemanager.player;
		hangar = gameObject;
		inventorypanel = hangar.transform.FindChild("GameObject").transform.FindChild("InventoryPanel").gameObject;
		infopanel = hangar.transform.FindChild("GameObject").transform.FindChild("InfoPanel").gameObject;
		for(int x = 0;x < 11;x++){
			for(int y = 0; y < 3;y++){
				for(int z = 0; z < 3;z++){
					if(player.playerloadout[x,y,z] != null){
						GameObject item = Instantiate(Resources.Load ("WeaponObjects/" +player.playerloadout[x,y,z].ingameObject),new Vector3(0,0,0),Quaternion.identity) as GameObject; 
						item.transform.localRotation =  Quaternion.Euler(0, 90, 0);
						item.transform.SetParent(playerobject.transform);
						item.transform.localPosition = new Vector3(z-1,y,x-5);
						item.transform.GetComponent<ItemInfo>().itempositionx = x;
						item.transform.GetComponent<ItemInfo>().itempositiony = y;
						item.transform.GetComponent<ItemInfo>().itempositionz = z;
						item.transform.GetComponent<ItemInfo>().item = player.playerloadout[x,y,z];
					}
				}
			}
		}
		updateinventory();
	}
	public void updateinventory(){
		for(int i = 1; i < inventorypanel.transform.childCount; i++){
			Destroy(inventorypanel.transform.GetChild(i).gameObject);
		}	
		for(int i = 0; i < player.getAllInventoryNames().Length; i++){
			GameObject button = Instantiate(Resources.Load("EditShipButton"),Vector3.zero, Quaternion.identity) as GameObject; 
			Vector3 temp = button.GetComponent<RectTransform>().localScale;
			button.transform.SetParent(inventorypanel.transform);
			button.GetComponent<RectTransform>().localPosition =  inventorypanel.transform.FindChild("First").GetComponent<RectTransform>().localPosition;
			button.GetComponent<RectTransform>().localPosition = new Vector3(button.GetComponent<RectTransform>().localPosition.x,button.GetComponent<RectTransform>().localPosition.y - (i*50),button.GetComponent<RectTransform>().localPosition.z);
			button.GetComponent<RectTransform>().localScale = temp;
			button.GetComponent<RectTransform>().localRotation = inventorypanel.transform.FindChild("First").GetComponent<RectTransform>().localRotation;
			int n = i;
			button.transform.GetChild(0).GetComponent<Text>().text = player.getItemFromInventory(i).name;
			button.GetComponent<Button>().onClick.AddListener(() => { buttonClick(n,button);}); 
		}
	}
	public void buttonClick(int index,GameObject button){
			currentbuttonclicked = button.gameObject;
			if(infopanel.transform.FindChild("EditShipButton") != null){
				Destroy(infopanel.transform.FindChild("EditShipButton").gameObject);
			}
			if(hangar.transform.parent.FindChild("ItemPosition").childCount > 0){
				Destroy(hangar.transform.parent.FindChild("ItemPosition").GetChild(0).gameObject);
				Destroy(playerobject.transform.FindChild("EditCircles").gameObject);
				for(int i = 0; i < playerobject.transform.childCount-1;i++){
					Destroy(playerobject.transform.GetChild(i).FindChild("EditCircles").gameObject);
				}
			}
			GameObject maineditclicker = Instantiate(Resources.Load ("EditCircles"),playerobject.transform.position, Quaternion.identity) as GameObject; 
			maineditclicker.name = "EditCircles";
			maineditclicker.transform.SetParent(playerobject.transform);
			for(int i = 0; i < playerobject.transform.childCount-1;i++){
				GameObject editclicker = Instantiate(Resources.Load ("EditCircles"),playerobject.transform.GetChild(i).transform.position, Quaternion.identity) as GameObject; 
				editclicker.name = "EditCircles";
				editclicker.transform.SetParent(playerobject.transform.GetChild(i).transform);
			}
			updateiteminfo(player.getItemFromInventory(index),false);
			GameObject item = Instantiate(Resources.Load ("WeaponObjects/" + player.getItemFromInventory(index).ingameObject),Vector3.zero, Quaternion.identity) as GameObject; 
			item.transform.SetParent(hangar.transform.parent.FindChild("ItemPosition").transform);
			item.transform.position = hangar.transform.parent.FindChild("ItemPosition").position;
			item.transform.localRotation = new Quaternion(Quaternion.identity.x,0,Quaternion.identity.z,Quaternion.identity.w);
			currentfocus = player.getItemFromInventory(index);
	}
	public void buttonClick(Item item,GameObject inobject){
		player.addItem(item);
		player.playerloadout[inobject.GetComponent<ItemInfo>().itempositionx,inobject.GetComponent<ItemInfo>().itempositiony,inobject.GetComponent<ItemInfo>().itempositionz] = null;
		Destroy(infopanel.transform.FindChild("EditShipButton").gameObject);
		Destroy(inobject.gameObject);
		updateiteminfo(null,false);
		updateinventory();
	}
	public void cleargui(){
		infopanel.transform.FindChild("Name").GetComponent<Text>().text = "";
		infopanel.transform.FindChild("Damage").GetComponent<Text>().text = "";
		infopanel.transform.FindChild("RateOfFire").GetComponent<Text>().text = "";
		if(hangar.transform.parent.FindChild("ItemPosition").childCount > 0){
			Destroy(hangar.transform.parent.FindChild("ItemPosition").GetChild(0).gameObject);
			Destroy(playerobject.transform.FindChild("EditCircles").gameObject);
			for(int i = 0; i < playerobject.transform.childCount-1;i++){
				Destroy(playerobject.transform.GetChild(i).FindChild("EditCircles").gameObject);
			}
		}
	}
	public void updateiteminfo(Item item,Boolean onship){
		if(item == null){
			infopanel.transform.FindChild("Name").GetComponent<Text>().text = "";
			infopanel.transform.FindChild("Damage").GetComponent<Text>().text = "";
			infopanel.transform.FindChild("RateOfFire").GetComponent<Text>().text = "";
			return;
		}
		cleargui();
		infopanel.transform.FindChild("Name").GetComponent<Text>().text = item.name;
		infopanel.transform.FindChild("Damage").GetComponent<Text>().text = "Damage: " + item.damage;
		infopanel.transform.FindChild("RateOfFire").GetComponent<Text>().text = "Rate of Fire: " + item.firerate;
		if(onship){
			GameObject button = Instantiate(Resources.Load("EditShipButton"),Vector3.zero, Quaternion.identity) as GameObject; 
			button.name = "EditShipButton";
			Vector3 temp = button.GetComponent<RectTransform>().localScale;
			button.transform.SetParent(infopanel.transform);
			button.GetComponent<RectTransform>().localPosition =  infopanel.transform.FindChild("Delete").GetComponent<RectTransform>().localPosition;
			button.GetComponent<RectTransform>().localScale = temp;
			button.GetComponent<RectTransform>().localRotation = infopanel.transform.FindChild("Delete").GetComponent<RectTransform>().localRotation;
			button.GetComponent<Button>().onClick.AddListener(() => { buttonClick(item,currentobjectfocus);}); 
			button.transform.GetChild(0).GetComponent<Text>().text = "Remove";
		}
	}
	public void startlevel(){
		StartCoroutine(waitroutine());
	}
	public IEnumerator waitroutine()
	{
		Camera.main.GetComponent<CameraFade>().SetScreenOverlayColor(Color.clear);
		Camera.main.GetComponent<CameraFade>().StartFade(Color.black,4);
		yield return new WaitForSeconds(2f); 
		Application.LoadLevel(currentlevelfocus);
	}
	public void editClicked(int x, int y, int z){
		Destroy(currentbuttonclicked.gameObject);
		Destroy(playerobject.transform.FindChild("EditCircles").gameObject);
		for(int i = 0; i < playerobject.transform.childCount-1;i++){
			Destroy(playerobject.transform.GetChild(i).FindChild("EditCircles").gameObject);
		}
		gamemanager.player.playerloadout[x,y,z] = currentfocus;
		GameObject item = Instantiate(Resources.Load ("WeaponObjects/" +currentfocus.ingameObject),new Vector3(0,0,0), Quaternion.identity) as GameObject; 
		item.transform.SetParent(playerobject.transform);
		item.transform.localPosition = new Vector3(z-1,y,x-5);
		item.transform.GetComponent<ItemInfo>().itempositionx = x;
		item.transform.GetComponent<ItemInfo>().itempositiony = y;
		item.transform.GetComponent<ItemInfo>().itempositionz = z;
		item.transform.GetComponent<ItemInfo>().item = currentfocus;

		updateiteminfo(null,false);

		Destroy(hangar.transform.parent.FindChild("ItemPosition").GetChild(0).gameObject);
		player.removeItemFromInventory(currentfocus);
		updateinventory();
	}
	// Update is called once per frame
	void Update () {
	
	}
}














