using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour {

  public float speed;
  public float rotationSpeed;
  private Rigidbody rb;

  // Use this for initialization
  void Start () {
    rb = GetComponent<Rigidbody>();
  }

  void FixedUpdate(){
    float moveHorizontal = Input.GetAxis ("Horizontal");
    float moveVertical = Input.GetAxis ("Vertical");

	Vector3 movement = new Vector3 (0.0f, moveVertical, -moveHorizontal);
	//rb.MovePosition(rb.position + movement * speed);
	//Quaternion deltaRotation = Quaternion.Euler(m_EulerAngleVelocity * Time.deltaTime);
	//m_Rigidbody.rotation *= deltaRotation;
	//rb.MoveRotation(rb.rotation + movement * speed);
	//rb.MoveRotation(movement);

	//Vector3 movement = new Vector3 (moveHorizontal, moveVertical, 0.0f);
	//rb.AddForce (movement * speed);
	
	Vector3 movementRotation = new Vector3 (moveHorizontal, moveVertical, 0.0f) * 100;
	Vector3 m_EulerAngleVelocity = movementRotation * speed * 10; //new Vector3(0, , 0);
	Quaternion deltaRotation = Quaternion.Euler(m_EulerAngleVelocity * Time.deltaTime);
	rb.MoveRotation(rb.rotation * deltaRotation);
	
  }

  // Update is called once per frame
  void Update () {
  }
}
