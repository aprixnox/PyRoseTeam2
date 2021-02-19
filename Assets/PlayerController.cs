using UnityEngine;
using System.Collections.Generic;
using UnityEditor.VersionControl;

public class PlayerController : MonoBehaviour {

	// Use this for initialization

	public Rigidbody2D Body;

	public int Speed;

	// How many rays are fired per frame, higher numbers provide better field of vision but poorer performance.
	[Range(10, 2000)]
	public int Accuracy = 400;

	[Range(0, Mathf.Infinity)]
	public float Distance = 9f;

	[Range(0, 360)]
	public float FOV = 0;

	// The MeshFilter the FOV mesh is loaded into.
	public MeshFilter VMeshFilter;

	Mesh VMesh;

	void Start () {
		VMesh = new Mesh();
		VMesh.name = "View Mesh";

		VMeshFilter.mesh = VMesh;
	}

	Vector3 RotateAround(Vector3 point, Vector3 pivot, Vector3 angles) {
		Vector3 direction = point - pivot;

		return (Quaternion.Euler(angles) * direction) + pivot;
	}

	void PaintFOV() {
		List<Vector3> vertpoints = new List<Vector3>();

		for (int i = 0; i <= Accuracy; i++) {
			float angle = transform.eulerAngles.z - FOV / 2 + (FOV / Accuracy) * i;

			RaycastHit2D hit = Physics2D.Raycast(transform.position, AngleToDir(angle));

			if (hit.collider)
				vertpoints.Add(hit.point);
			else
				vertpoints.Add(transform.position + AngleToDir(angle) * Distance);
		}

		Vector3[] vertices = new Vector3[vertpoints.Count + 1];

		vertices[0] = Vector3.zero;

		int[] triangles = new int[vertpoints.Count * 3];
		
		for (int i = 0; i < vertpoints.Count; i++) {
			vertices[i + 1] = transform.InverseTransformPoint(vertpoints[i]);

			if (i < vertpoints.Count - 1) {
				triangles [i * 3] = 0;
				triangles [i * 3 + 1] = i + 1;
				triangles [i * 3 + 2] = i + 2;
			}
		}

		VMesh.Clear();

		VMesh.vertices = vertices;

		VMesh.triangles = triangles;

		VMesh.RecalculateNormals();
	}

	Vector3 AngleToDir(float angle) {
		// angle += transform.eulerAngles.z;

		return new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0);
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

	void LateUpdate() {
		PaintFOV();
	}
}
