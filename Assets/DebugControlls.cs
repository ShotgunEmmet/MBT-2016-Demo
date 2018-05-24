using UnityEngine;
using UnityEngine.InputNew;

public class DebugControlls : MonoBehaviour {

    FirstPersonControls m_MapInput;
    public PlayerInput playerInput;

    public Soldier soldier;
    public CharacterInputController tank;
    public MBT mbt;
    public MBT2 mbt2;

    public void Start()
    {
        m_MapInput = playerInput.GetActions<FirstPersonControls>();

        if (!playerInput.handle.global)
            transform.Find("Canvas/Virtual Joystick").gameObject.SetActive(false);

        tank.enabled = false;
        mbt.enabled = false;
        mbt2.enabled = false;
    }

    public void Update()
    {
        var soldierSelected = m_MapInput.selectSoldier.isHeld;
        if (soldierSelected)
        {
            soldier.enabled = true;
            tank.enabled = false;
            mbt.enabled = false;
            mbt2.enabled = false;
        }

        var tankSelected = m_MapInput.selectTank.isHeld;
        if (tankSelected)
        {
            soldier.enabled = false;
            tank.enabled = true;
            mbt.enabled = false;
            mbt2.enabled = false;
        }

        var mbtSelected = m_MapInput.selectMBT.isHeld;
        if (mbtSelected)
        {
            soldier.enabled = false;
            tank.enabled = false;
            mbt.enabled = true;
            mbt2.enabled = false;
        }

        var mbt2Selected = m_MapInput.selectMBT2.isHeld;
        if (mbt2Selected)
        {
            soldier.enabled = false;
            tank.enabled = false;
            mbt.enabled = false;
            mbt2.enabled = true;
        }


    }

    
}

