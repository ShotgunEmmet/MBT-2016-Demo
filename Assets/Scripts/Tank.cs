using UnityEngine;
using System.Collections;

public class Tank : MonoBehaviour {

    public float acceleration = 0.05f;
    public float steering = 1f;
    public float turretSpeed = 2f;

    float tireRotation = 45f;

    public Transform body, turret, tire1, tire2, tire3, tire4;

    //PlayerInput input;

	// Use this for initialization
	void Start () {
        //input = GetComponent<playerInput>();
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey("q"))
        {
            turret.parent.transform.Rotate(Vector3.forward * turretSpeed);
        }
        if (Input.GetKey("e"))
        {
            turret.parent.transform.Rotate(Vector3.forward * -turretSpeed);
        }

        if (Input.GetKey("w") || Input.GetAxisRaw("Vertical") > 0.5f)
        {
            transform.position = transform.position + (transform.up * acceleration);
        }
        if (Input.GetKey("s") || Input.GetAxisRaw("Vertical") < -0.5f)
        {
            transform.position = transform.position + (transform.up * -acceleration);
        }

        bool turning = false;
        if (Input.GetKey("a") || Input.GetAxisRaw("Horizontal") < -0.5f)
        {
            transform.Rotate(0,0,steering);

            tire1.up = body.up;
            tire1.Rotate(Vector3.forward * tireRotation);

            tire2.up = body.up;
            tire2.Rotate(Vector3.forward * tireRotation);

            tire3.up = body.up;
            tire3.Rotate(Vector3.forward * -tireRotation);

            tire4.up = body.up;
            tire4.Rotate(Vector3.forward * -tireRotation);

            turning = true;
        }
        if (Input.GetKey("d") || Input.GetAxisRaw("Horizontal") > 0.5f)
        {
            transform.Rotate(0, 0, -steering);

            tire1.up = body.up;
            tire1.Rotate(Vector3.forward * -tireRotation);

            tire2.up = body.up;
            tire2.Rotate(Vector3.forward * -tireRotation);

            tire3.up = body.up;
            tire3.Rotate(Vector3.forward * tireRotation);

            tire4.up = body.up;
            tire4.Rotate(Vector3.forward * tireRotation);

            turning = true;
        }
        if(!turning)
        {
            tire1.up = body.up;
            tire2.up = body.up;
            tire3.up = body.up;
            tire4.up = body.up;
        }
        
    }
    
}
