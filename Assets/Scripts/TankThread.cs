using UnityEngine;
using System.Collections;

public class TankThread : MonoBehaviour {

    public enum MovementState {BACKWARD = 0, STILL, FORWARD }

    public MovementState moveDirection = MovementState.STILL;

    public Sprite[] animation;
    float currentFrame = 0;
    public float frameSpeed = 15f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if(moveDirection == MovementState.FORWARD)
        {
            currentFrame += Time.deltaTime * frameSpeed;
            if (currentFrame >= animation.Length)
                currentFrame -= animation.Length;

            (this.gameObject.GetComponent(typeof(SpriteRenderer)) as SpriteRenderer).sprite = animation[((int)currentFrame)];
        }
        else if(moveDirection == MovementState.BACKWARD)
        {
            currentFrame -= Time.deltaTime * frameSpeed;
            if (currentFrame < 0)
                currentFrame += animation.Length;

            (this.gameObject.GetComponent(typeof(SpriteRenderer)) as SpriteRenderer).sprite = animation[((int)currentFrame)];
        }
	}
}
