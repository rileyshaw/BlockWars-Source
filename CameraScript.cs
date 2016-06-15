using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {
	Transform player;
	float maxLength;
	float horiz =0;
	float temp = 0;
	Vector3 origin;
	// Use this for initialization
	void Start () {
		origin = gameObject.transform.localEulerAngles;
		maxLength = transform.localPosition.magnitude;
		player = GameObject.FindGameObjectWithTag("Player").transform;

	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(!player.GetComponent<Rigidbody>().isKinematic){
			horiz = Input.GetAxis("Horizontal");
			if(horiz > 0.1){
				temp += ((25-temp)/20);
			}else if(horiz < -0.1){
				temp += -((25+temp)/20);
			}else if(temp > 0){
				temp -= ((temp)/20);
			}else if(temp < 0){
				temp -= ((temp)/20);
			}
			transform.localEulerAngles = new Vector3(origin.x, origin.y, temp);

			bool closeFlag = Physics.Raycast(new Vector3(player.position.x,player.position.y+1.0f,player.position.z), -new Vector3(transform.forward.x,transform.forward.y,transform.forward.z), transform.localPosition.magnitude);
			bool farFlag = Physics.Raycast(new Vector3(player.position.x,player.position.y+1.0f,player.position.z), -new Vector3(transform.forward.x,transform.forward.y,transform.forward.z), transform.localPosition.magnitude + 0.4f);
			if(closeFlag && farFlag && transform.localPosition.magnitude > 1f){
				transform.position += (transform.forward)*0.1f;
			}else if(!farFlag && !closeFlag){
				if (transform.localPosition.magnitude < maxLength) {
					transform.position -= (transform.forward)*0.1f;
				}
			}
		} 
	}
}
