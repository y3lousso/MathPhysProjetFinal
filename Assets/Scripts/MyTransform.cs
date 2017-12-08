using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTransform : MonoBehaviour {

	public MyVector3 position;
	public MyVector3 rotation;
	public MyVector3 localScale;

	void Start () {
		position = transform.position;
		rotation = transform.rotation.eulerAngles;
		localScale = transform.localScale;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = position;
        transform.rotation = MathsUtility.GetQuaternionFromEulerAngle(rotation);
        //transform.rotation = Quaternion.Euler(rotation);
        transform.localScale = localScale;
	}

	public void OnDrawGizmos()
	{
		position = transform.position;
		rotation = transform.rotation.eulerAngles;
		localScale = transform.localScale;
	}

	public void Translate(MyVector3 translation) {
		position += translation;
	}

	public void Rotate(MyVector3 angles) {
		rotation += angles;
	}
}
