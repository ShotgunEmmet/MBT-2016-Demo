using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputNew;
using UnityEngine.Serialization;

public class Soldier
    : MonoBehaviour
{
    FirstPersonControls m_MapInput;
    Rigidbody2D m_Rigid;
    Vector2 m_Rotation = Vector2.zero;
    float m_TimeOfLastShot;

    public PlayerInput playerInput;
    public float moveSpeed = 5;
    public GameObject projectile;
    public float timeBetweenShots = 0.2f;
    public Transform Gun;

    [Space(10)]
    
    public RuntimeRebinding rebinder;

    public void Start()
    {
        m_MapInput = playerInput.GetActions<FirstPersonControls>();

        if (rebinder != null)
            rebinder.Initialize(m_MapInput);

        m_Rigid = GetComponent<Rigidbody2D>();

        if (!playerInput.handle.global)
            transform.Find("Canvas/Virtual Joystick").gameObject.SetActive(false);
    }

    public void Update()
    {
        // Move
        var move = m_MapInput.move.vector2;
        Console.WriteLine(move.ToString());
        Vector3 velocity = new Vector3(move.x, move.y, 0) * moveSpeed;
        m_Rigid.velocity = new Vector3(velocity.x, velocity.y, velocity.z);

        // Look
        var look = m_MapInput.look.vector2;

        if (look.magnitude > 0.4f)
        {
            float heading = Mathf.Atan2(-look.x, look.y);
            transform.rotation = Quaternion.Euler(0f, 0f, heading * Mathf.Rad2Deg + 90f);
        }
        //LookAtMouse();
        

        // Fire
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
        

        //if (m_MapInput.menu.wasJustPressed)
        //	sizer.OpenMenu();

        if (rebinder != null)
        {
            if (m_MapInput.reconfigure.wasJustPressed)
                rebinder.enabled = !rebinder.enabled;

            if (rebinder.enabled == m_MapInput.active)
            {
                //LockCursor(!rebinder.enabled);
                m_MapInput.active = !rebinder.enabled;
            }
        }
        
    }
    

    private string GetControlHelp(InputControl control)
    {
        return string.Format("Use {0} to {1}!", control.GetPrimarySourceName(), control.name);
    }

    void Fire()
    {
        var newProjectile = Instantiate(projectile);
        newProjectile.transform.position = Gun.position + Gun.right * 0.1f;
        newProjectile.transform.rotation = transform.rotation;
        newProjectile.transform.Rotate(0, 0, -90);
        newProjectile.GetComponent<Rigidbody2D>().AddForce(newProjectile.transform.up * 20f * newProjectile.GetComponent<Rigidbody2D>().mass, ForceMode2D.Impulse);
    }
    

    void LookAtMouse()
    {
        var objectPos = Camera.main.WorldToScreenPoint(transform.position);
        var dir = Input.mousePosition - objectPos;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg));
    }
}
