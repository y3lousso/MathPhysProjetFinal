using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Ammo {Ball, AABB, OBB};

public class Player : MonoBehaviour {

	public CollisionEngine colEngine;

	public float speed = 20f;
	public Ammo ammo = Ammo.OBB;

	private float lastShoot;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.W))
			transform.Rotate (new Vector3 (speed * Time.deltaTime, 0, 0));

		if (Input.GetKey (KeyCode.S))
			transform.Rotate (new Vector3 (-speed * Time.deltaTime, 0, 0));

		if (Input.GetKey (KeyCode.A))
			transform.Rotate (new Vector3 (0, speed * Time.deltaTime, 0));

		if (Input.GetKey (KeyCode.D))
			transform.Rotate (new Vector3 (0, -speed * Time.deltaTime, 0));

		if (Input.GetKey (KeyCode.R))
			SceneManager.LoadScene (SceneManager.GetActiveScene().name);

		if (Input.GetKey (KeyCode.Escape))
			SceneManager.LoadScene ("Menu");

		if (Input.GetKey (KeyCode.Space) && Time.realtimeSinceStartup - lastShoot > 0.3f) {
			lastShoot = Time.realtimeSinceStartup;

			GameObject go;

			switch(ammo) {
			case Ammo.Ball:
				go = GameObject.CreatePrimitive (PrimitiveType.Sphere);
				go.AddComponent<MySphereCollider> ();
				break;
			case Ammo.AABB:
				go = GameObject.CreatePrimitive (PrimitiveType.Cube);
				MyAABBCollider col = go.AddComponent<MyAABBCollider> ();
				col.adaptAABB = true;
				break;
			default:
				go = GameObject.CreatePrimitive (PrimitiveType.Cube);
				go.AddComponent<MyOBBCollider> ();
				break;
			}


			go.transform.position = Camera.main.transform.position;

			MyRigidBody rb = go.GetComponent<MyRigidBody> ();

			rb.velocity = -go.transform.position * Random.Range (0.5f, 0.8f);
			rb.angVelocity = MyVector3.Zero;

			colEngine._objects.Add (go.transform);
		}
	}
}
