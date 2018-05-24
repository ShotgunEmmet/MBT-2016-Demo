using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputNew;
using UnityEngine.Serialization;

public class CharacterInputController
	: MonoBehaviour
{
	FirstPersonControls m_MapInput;
	Rigidbody2D m_Rigid;
	Vector2 m_Rotation = Vector2.zero;
	float m_TimeOfLastShot;
	
	public PlayerInput playerInput;
	public float moveSpeed = 5;
	public GameObject projectile;
	public float timeBetweenShots = 0.5f;
    public Transform Gun;

    [Space(10)]
	
	//public CubeSizer sizer;
	//public Text controlsText;
	public RuntimeRebinding rebinder;

	public void Start()
	{
		m_MapInput = playerInput.GetActions<FirstPersonControls>();

		if (rebinder != null)
			rebinder.Initialize(m_MapInput);

		m_Rigid = GetComponent<Rigidbody2D>();
		//LockCursor(true);

		if (!playerInput.handle.global)
			transform.Find("Canvas/Virtual Joystick").gameObject.SetActive(false);
	}

	public void Update()
	{
		// Move
		var move = m_MapInput.move.vector2;
        Console.WriteLine(move.ToString());
		Vector3 velocity = transform.TransformDirection(new Vector3(0, move.y, 0)) * moveSpeed;
		m_Rigid.velocity = new Vector3(velocity.x, velocity.y, velocity.z);

		// Look
		var look = m_MapInput.look.vector2;

        //transform.up = look;
		m_Rotation.y -= move.x;
		m_Rotation.x = Mathf.Clamp(m_Rotation.x - look.y, -89, 89);

		transform.localEulerAngles = new Vector3(0, 0, m_Rotation.y);

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

		//if (m_MapInput.lockCursor.wasJustPressed)
		//	LockCursor(true);

		//if (m_MapInput.unlockCursor.wasJustPressed)
		//	LockCursor(false);
		
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
				//controlsText.enabled = !rebinder.enabled;
			}
		}
		
		//HandleControlsText();
	}
	
	//void HandleControlsText()
	//{
	//	string help = string.Empty;
		
	//	help += GetControlHelp(m_MapInput.moveX) + "\n";
	//	help += GetControlHelp(m_MapInput.moveY) + "\n";
	//	help += GetControlHelp(m_MapInput.lookX) + "\n";
	//	help += GetControlHelp(m_MapInput.lookY) + "\n";
	//	help += GetControlHelp(m_MapInput.fire) + "\n";
	//	help += GetControlHelp(m_MapInput.menu);
	//	controlsText.text = help;
	//}
	
	private string GetControlHelp(InputControl control)
	{
		return string.Format("Use {0} to {1}!", control.GetPrimarySourceName(), control.name);
	}

	void Fire()
	{
		var newProjectile = Instantiate(projectile);
        newProjectile.transform.position = Gun.position + Gun.up * 0.1f;
		newProjectile.transform.rotation = transform.rotation;
        newProjectile.GetComponent<Rigidbody2D>().AddForce(newProjectile.transform.up * 10f * newProjectile.GetComponent<Rigidbody2D>().mass, ForceMode2D.Impulse);
	}

	//void LockCursor(bool value)
	//{
	//	var mouse = Mouse.current;
	//	if (mouse != null)
	//		mouse.cursor.isLocked = value;
	//}
}
