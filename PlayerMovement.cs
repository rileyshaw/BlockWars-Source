using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;


public class PlayerMovement : MonoBehaviour {
	public Rigidbody playerrigid;
	public GameObject playerObject;
	private Player player;
	float horiz =0;
	float vert = 0;
	float horizlook = 0;
	float verlook = 0;
	float rise = 0;
	float temp = 0;
	Vector2 _mouseAbsolute;
	Vector2 _smoothMouse;
	RectTransform panel;
	Vector3 hitPoint;
	public float fuelIncrement = 0;
	public float fuelAdd = 0;
	public float healthAdd = 0;
	public Vector2 clampInDegrees = new Vector2(360, 180);
	public bool lockCursor;
	public Vector2 sensitivity = new Vector2(2, 2);
	public Vector2 smoothing = new Vector2(1, 1);
	public Vector2 targetDirection;
	public Vector2 targetCharacterDirection;
	public bool hasfuel = true;
	public Gun[] guns;
	private int count;
	// Use this for initialization
	void Start () {
		player = GameObject.Find("GameManager").GetComponent<GameManager>().player;
		playerObject = gameObject;
		playerrigid = playerObject.GetComponent<Rigidbody>();
	}
	void FixedUpdate () {
		if(GetComponent<Rigidbody>().isKinematic == false){
			if(Input.GetAxis("Fire1") > 0.001f){
				count = 0;
				while(guns[count] != null){
					guns[count].fire();
					count++;
				}
				//Instantiate(Resources.Load("Bullet"),new Vector3(transform.position.x+transform.forward.x, transform.position.y,transform.position.z+transform.forward.z), transform.rotation);
			}
			horiz =0;
			vert = 0;
			horizlook = 0;
			verlook = 0;
			rise = 0;
			horiz = Input.GetAxis("Horizontal");
			vert = Input.GetAxis("Vertical");
			rise = Input.GetAxis("Jump");
			if(hasfuel){
				playerrigid.velocity = new Vector3 (0, 0, 0);
				playerrigid.velocity = new Vector3(transform.forward.x,Vector3.forward.y,transform.forward.z) * vert * 15;
				playerrigid.velocity += new Vector3(transform.right.x,Vector3.right.y,transform.right.z)* horiz * 15;
				playerrigid.velocity += Vector3.up* rise * 15;
			}else{
				playerrigid.velocity = new Vector3 (0, 0, 0);
				playerrigid.velocity = new Vector3(transform.forward.x,Vector3.forward.y,transform.forward.z) * vert * 2;
				playerrigid.velocity += new Vector3(transform.right.x,Vector3.right.y,transform.right.z)* horiz * 2;
				playerrigid.velocity += Vector3.up* rise * 2;
			}
			if(playerrigid.velocity != Vector3.zero && hasfuel == true){
				fuelIncrement = fuelIncrement + (60*Time.deltaTime);
				if(fuelIncrement >= 100) {
					fuelIncrement = 0;
					player.setCurrentFuel(player.getCurrentFuel() - 1);
					if(player.getCurrentFuel() <= 0){
						hasfuel = false;
					}
				}
			}
			
			Screen.lockCursor = lockCursor;
			var targetOrientation = Quaternion.Euler(targetDirection);
			var mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X") * 180*Time.deltaTime, Input.GetAxisRaw("Mouse Y") *180* Time.deltaTime);
			mouseDelta = Vector2.Scale(mouseDelta, new Vector2(sensitivity.x * smoothing.x, sensitivity.y * smoothing.y));
			_smoothMouse.x = Mathf.Lerp(_smoothMouse.x, mouseDelta.x, 1f / smoothing.x);
			_smoothMouse.y = Mathf.Lerp(_smoothMouse.y, mouseDelta.y, 1f / smoothing.y);
			_mouseAbsolute += _smoothMouse;
			if (clampInDegrees.x < 360)
				_mouseAbsolute.x = Mathf.Clamp(_mouseAbsolute.x, -clampInDegrees.x * 0.5f, clampInDegrees.x * 0.5f);
			var xRotation = Quaternion.AngleAxis(-_mouseAbsolute.y, targetOrientation * Vector3.right);
			transform.localRotation = xRotation;
			if (clampInDegrees.y < 360)
				_mouseAbsolute.y = Mathf.Clamp(_mouseAbsolute.y, -clampInDegrees.y * 0.5f, clampInDegrees.y * 0.5f);
			
			transform.localRotation *= targetOrientation;
			var yRotation = Quaternion.AngleAxis(_mouseAbsolute.x, transform.InverseTransformDirection(Vector3.up));
			transform.localRotation *= yRotation;
			
			
			if(horiz > 0.9){
				temp += ((25-temp)/20);
			}else if(horiz < -0.9){
				temp += -((25+temp)/20);
			}else if(temp > 0){
				temp -= ((temp)/20);
			}else if(temp < 0){
				temp -= ((temp)/20);
			}
			transform.eulerAngles = new Vector3(transform.eulerAngles.x + verlook*3, transform.eulerAngles.y + horizlook*3,-temp);
		}

		
	}
	void OnTriggerEnter(Collider other) {
		if(other.gameObject.tag == "EnemyBullet"){
			player.takeDamage(other.GetComponent<EnemyBullet>().damage);
		}
	}
	void OnCollisionEnter(Collision other) {
//		playerrigid.velocity = new Vector3 (0, 0, 0);
	}
}