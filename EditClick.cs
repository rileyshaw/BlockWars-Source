using UnityEngine;
using System.Collections;

public class EditClick : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnMouseDown() {
		int x = gameObject.transform.parent.transform.parent.GetComponent<ItemInfo>().itempositionx;
		int y = gameObject.transform.parent.transform.parent.GetComponent<ItemInfo>().itempositiony;
		int z = gameObject.transform.parent.transform.parent.GetComponent<ItemInfo>().itempositionz;
		HangarController hangarobj = GameObject.Find("HangarObj").transform.FindChild("Hangar").GetComponent<HangarController>();
		if(gameObject.name == "Right"){
			if(x+1 < 11){
				hangarobj.editClicked(x+1,y,z);
			}else{
				Debug.Log("Beyond x bound");
			}
		}else if(gameObject.name == "Left"){
			if(x-1 >= 0){
				hangarobj.editClicked(x-1,y,z);
			}else{
				Debug.Log("Beyond x bound");
			}
		}else if(gameObject.name == "Top"){
			if(y+1 < 3){
				hangarobj.editClicked(x,y+1,z);
			}else{
				Debug.Log("Beyond y bound");
			}
		}else if(gameObject.name == "Bottom"){
			if(y-1 <= 0){
				hangarobj.editClicked(x,y-1,z);
			}else{
				Debug.Log("Beyond y bound");
			}
		}else if(gameObject.name == "Front"){
			if(z+1 < 3){
				hangarobj.editClicked(x,y,z+1);
			}else{
				Debug.Log("Beyond y bound");
			}
		}else if(gameObject.name == "Back"){
			if(z-1 >= 0){
				hangarobj.editClicked(x,y,z-1);
			}else{
				Debug.Log("Beyond y bound");
			}
		}
	}
}
