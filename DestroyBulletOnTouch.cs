using UnityEngine;
using System.Collections;

public class DestroyBulletOnTouch : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnTriggerEnter(Collider other) {
		if(other.gameObject.tag == "Bullet" || other.gameObject.tag == "EnemyBullet"){
			if(this.gameObject.tag != "Bullet" && this.gameObject.tag != "EnemyBullet" && this.gameObject.tag != "Player" 
			   && this.gameObject.tag != "Feul" && this.gameObject.tag != "Health"){
				if (other.gameObject.tag == "Bullet") {
					 Instantiate(Resources.Load("BulletExplosion"),other.transform.position, other.transform.rotation); 
					Destroy(other.gameObject.transform.parent.gameObject);
				}
				if (other.gameObject.tag == "EnemyBullet") {
					Instantiate(Resources.Load("EnemyBulletExplosion"),other.transform.position, other.transform.rotation); 
					Destroy(other.gameObject.transform.parent.gameObject);
				}
			}
			if(other.gameObject.tag == "EnemyBullet" && this.gameObject.tag == "Player"){
				Destroy(other.gameObject.transform.parent.gameObject);
			}
			if(other.gameObject.tag == "EnemyBullet" && (this.gameObject.tag == "Fuel" || this.gameObject.tag == "Health")){
				Destroy(other.gameObject.transform.parent.gameObject);
			}
		}
	}
}
	