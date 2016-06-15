using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour {
	public Item weapon;
	float firerate;
	bool isfiring = false;
	IEnumerator firewait() {
		GameObject bullet = (GameObject)Instantiate(Resources.Load("Bullet"),new Vector3(transform.position.x+transform.forward.x, transform.position.y,transform.position.z+transform.forward.z), transform.rotation);
		bullet.transform.GetChild(0).GetComponent<Bullet>().damage = weapon.damage;
		yield return new WaitForSeconds(1/weapon.firerate); 
		isfiring = false;
	}
	public void fire(){
		if(isfiring == false){
			isfiring = true;
			StartCoroutine(firewait());
		}
	}
}
