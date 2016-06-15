using UnityEngine;
using System.Collections;

public class RotateAround : MonoBehaviour {
	
	private Transform ship;
	private Transform hangarhud;
	private bool mouseClickjudge = false;
	void Start(){
		ship = GameObject.Find("Player").transform;
		hangarhud = GameObject.Find("HangarObj").transform;
	}
	void Update(){
		if(GameManager.currentScreen == 2){
			if ( Input.GetMouseButtonDown( 0 ) ) {
				mouseClickjudge = true; 
			//	hangarhud.transform.SetParent(transform);
			} 
			if ( Input.GetMouseButtonUp( 0 ) ) { 
				mouseClickjudge = false; 
				//hangarhud.transform.SetParent(null);
			}
			if ( mouseClickjudge ) {
				transform.RotateAround( ship.position, ship.up, Input.GetAxis( "Mouse X" ) * 3 ); 
				hangarhud.RotateAround( ship.position, ship.up, Input.GetAxis( "Mouse X" ) * 3 ); 
				transform.Translate(-ship.up * Input.GetAxis( "Mouse Y" ) * 6 * Time.deltaTime ); 
				hangarhud.Translate( -ship.up * Input.GetAxis( "Mouse Y" ) * 6 * Time.deltaTime  ); 

			}

		}
	}
}
