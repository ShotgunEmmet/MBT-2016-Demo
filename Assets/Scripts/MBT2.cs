using UnityEngine;
using System.Collections;
using UnityEngine.InputNew;

public class MBT2 : MonoBehaviour
{
    FirstPersonControls m_MapInput;
    Rigidbody2D m_Rigid;
    Vector2 m_Rotation = Vector2.zero;
    float m_TimeOfLastShot;

    public PlayerInput playerInput;
    public RuntimeRebinding rebinder;

    public float acceleration = 0.05f;
    public float moveSpeed = 5f;
    public float steering = 1f;
    public float turretSpeed = 2f;
    public GameObject projectile;
    public float timeBetweenShots = 0.5f;
    public Transform Gun;


    public Transform body, turret;
    public TankThread threadL, threadR;

    //PlayerInput input;

    // Use this for initialization
    void Start()
    {
        //input = GetComponent<playerInput>();
        m_MapInput = playerInput.GetActions<FirstPersonControls>();

        if (rebinder != null)
            rebinder.Initialize(m_MapInput);

        m_Rigid = GetComponent<Rigidbody2D>();
        //LockCursor(true);

        if (!playerInput.handle.global)
            transform.Find("Canvas/Virtual Joystick").gameObject.SetActive(false);
    }

    enum TURNING { FALSE, LEFT, RIGHT };
    // Update is called once per frame
    void FixedUpdate()
    {
        TURNING turning = TURNING.FALSE;

        var move = m_MapInput.move.vector2;

        /*
        if (Input.GetKey("q"))
        {
            turret.transform.Rotate(Vector3.FORWARD * turretSpeed);
        }
        if (Input.GetKey("e"))
        {
            turret.transform.Rotate(Vector3.FORWARD * -turretSpeed);
        }
        */
        //if (Input.GetKey("a") || Input.GetAxisRaw("Horizontal") < -0.5f)
        if (move.x < -.02f)
        {
            turning = TURNING.LEFT;
        }
        //if (Input.GetKey("d") || Input.GetAxisRaw("Horizontal") > 0.5f)
        if (move.x > .02f)
        {
            turning = TURNING.RIGHT;
        }
        if (turning == TURNING.FALSE)
        {

        }


        threadL.moveDirection = TankThread.MovementState.STILL;
        threadR.moveDirection = TankThread.MovementState.STILL;

        //if (Input.GetKey("w") || Input.GetAxisRaw("Vertical") > 0.5f)
        if (move.y > .02f)
        {
            transform.position = transform.position + (transform.up * acceleration);

            threadL.moveDirection = TankThread.MovementState.FORWARD;
            threadR.moveDirection = TankThread.MovementState.FORWARD;

            if (turning == TURNING.LEFT)
            {
                threadL.moveDirection = TankThread.MovementState.STILL;
                transform.Rotate(0, 0, steering);
            }
            if (turning == TURNING.RIGHT)
            {
                threadR.moveDirection = TankThread.MovementState.STILL;
                transform.Rotate(0, 0, -steering);
            }
        }
        //if (Input.GetKey("s") || Input.GetAxisRaw("Vertical") < -0.5f)
        if (move.y < -.02f)
        {
            transform.position = transform.position + (transform.up * -acceleration);

            threadL.moveDirection = TankThread.MovementState.BACKWARD;
            threadR.moveDirection = TankThread.MovementState.BACKWARD;

            if (turning == TURNING.LEFT)
            {
                threadL.moveDirection = TankThread.MovementState.STILL;
                transform.Rotate(0, 0, -steering);
            }
            if (turning == TURNING.RIGHT)
            {
                threadR.moveDirection = TankThread.MovementState.STILL;
                transform.Rotate(0, 0, steering);
            }
        }

        var look = m_MapInput.look.vector2;

        if (look.magnitude > 0.4f)
        {
                float heading = Mathf.Atan2(-look.x, look.y);
                Gun.transform.rotation = Quaternion.Euler(0f, 0f, heading * Mathf.Rad2Deg);
        }
        var fire = m_MapInput.fire.isHeld;
        if (fire)
        {
            var currentTime = Time.time;
            var timeElapsedSinceLastShot = currentTime - m_TimeOfLastShot;
            if (timeElapsedSinceLastShot > timeBetweenShots)
            {
                m_TimeOfLastShot = currentTime;
                Fire();
            }
        }

    }
    void Fire()
    {
        var newProjectile = Instantiate(projectile);
        newProjectile.transform.position = Gun.position + Gun.up * 1.1f;
        newProjectile.transform.rotation = Gun.transform.rotation;
        //newProjectile.transform.Rotate(0, 0, -90);
        newProjectile.GetComponent<Rigidbody2D>().AddForce(newProjectile.transform.up * 20f * newProjectile.GetComponent<Rigidbody2D>().mass, ForceMode2D.Impulse);
    }


    void LookAtMouse()
    {
        var objectPos = Camera.main.WorldToScreenPoint(transform.position);
        var dir = Input.mousePosition - objectPos;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg));
    }
}
