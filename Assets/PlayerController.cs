using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	// Use this for initialization

	public Rigidbody2D Body;

	public int Speed;

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		// Rotate to face cursor

		Vector3 mouse_pos = Input.mousePosition;
		
		Vector3 object_pos = Camera.main.WorldToScreenPoint(transform.position);
		mouse_pos.x = mouse_pos.x - object_pos.x;
		mouse_pos.y = mouse_pos.y - object_pos.y;
		float angle = Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

		Vector2 newpos = Body.position;

		if (Input.GetKey("w")) {
			newpos += new Vector2(0, Time.deltaTime * Speed);
		}
		
		if (Input.GetKey("s")) {
			newpos += new Vector2(0, Time.deltaTime * -Speed);
		}

		if (Input.GetKey("a")) {
			newpos += new Vector2(Time.deltaTime * -Speed, 0);
		}
		
		if (Input.GetKey("d")) {
			newpos += new Vector2(Time.deltaTime * Speed, 0);
		}

		Body.MovePosition(newpos);
	}
}
