using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

//require some things the bot control needs
[RequireComponent(typeof(Animator), typeof(Rigidbody), typeof(CapsuleCollider))]
public class SimpleControlScript : MonoBehaviour
{

    private Animator anim;	
    private Rigidbody rbody;

    private Transform leftFoot;
    private Transform rightFoot;

    public int groundContacts;

    public AudioSource FootStepSource;
	public AudioSource noAccessSound;
    public AudioClip[] FootStepSounds;

    public int speedModifier = 1;
    public int speedBoostDuration;
    private float filteredForwardInput = 0f;
    private float filteredTurnInput = 0f;

    public float forwardInputFilter = 5f;
    public float turnInputFilter = 5f;

    private float forwardSpeedLimit = 1f;

//    private float timeOfStep;
//    private float timeOfLastLeftStep = 0f;
//    private float timeOfLastRightStep = 0f;
//    public float timeSinceLastStepBuffer = 0.05f;
	private float minFootStepSeparation = 0.4f;

	public int ballForwardForce = 10;
	public int ballYForce = 5;

    public int startingNumSocks;

    public bool hidden;
    private int bushesIn;

	public int curNumSocks;
    public Text sockText;

	public GameObject AccessDenied;

	private Vector3 aimingVector;
	public float throwPower = 3;
	public int yForce = 5;
	public int forwardForce = 10;
	public Slider throwMeter;
	public int screenPadding;
	public bool IsGrounded
    {
        get { return groundContacts > 0; }
    }


	public GameOverManager GameOverManager;
	float restartTimer;

    void Awake()
    {

        anim = GetComponent<Animator>();

        if (anim == null)
            Debug.Log("Animator could not be found");

        rbody = GetComponent<Rigidbody>();

        if (rbody == null)
            Debug.Log("Rigid body could not be found");

    }


    // Use this for initialization
    void Start()
    {
        groundContacts = 0;
        hidden = false;
		//example of how to get access to certain limbs
        leftFoot = this.transform.Find("mixamorig:Hips/mixamorig:LeftUpLeg/mixamorig:LeftLeg/mixamorig:LeftFoot");
        rightFoot = this.transform.Find("mixamorig:Hips/mixamorig:RightUpLeg/mixamorig:RightLeg/mixamorig:RightFoot");

        if (leftFoot == null || rightFoot == null)
            Debug.Log("One of the feet could not be found");

        curNumSocks = startingNumSocks;
    }

	protected float lastLeftFoot = 0f;
	//anim event callback called by all animations that make footsteps
	void leftFootStep() {
		//      if (filteredForwardInput < -0.5f || filteredForwardInput > 0.5f ) {
		//          minFootStepSeparation = 0.35f;
		//      } else {
		//          minFootStepSeparation = 0.8f;
		//      }
		//see if it's been long enough since the last footstep
		if(Time.timeSinceLevelLoad - lastLeftFoot > minFootStepSeparation)
		{
			lastLeftFoot = Time.timeSinceLevelLoad;

			const float rayOriginOffset = 0.5f; //origin near bottom of collider, so need a fudge factor up away from there
			const float rayDepth = 0.5f; //how far down will we look for ground?
			const float totalRayLen = rayOriginOffset + rayDepth;

			Ray ray = new Ray(leftFoot.position + Vector3.up * rayOriginOffset, Vector3.down);
			Debug.DrawLine(ray.origin, ray.origin + ray.direction * totalRayLen, Color.green);
			RaycastHit hit;
			//Cast ray and look for ground. If ground is close, then transition out of falling animation
			if (Physics.Raycast(ray, out hit, totalRayLen))
			{
				//if (hit.collider.gameObject.CompareTag ("grass")) {   
				//spawn event for footstep sound
				AudioSource.PlayClipAtPoint(FootStepSounds[0],leftFoot.position);

			}
		}

		//Otherwise, just fall through and ignore the footstep callback that wanted to play
	}


	protected float lastRightFoot = 0f;
	//anim event callback called by all animations that make footsteps
	void rightFootStep() {
		//      if (filteredForwardInput < -0.5f || filteredForwardInput > 0.5f ) {
		//          minFootStepSeparation = 0.35f;
		//      } else {
		//          minFootStepSeparation = 0.8f;
		//      }
		//see if it's been long enough since the last footstep
		if(Time.timeSinceLevelLoad - lastRightFoot > minFootStepSeparation)
		{
			lastRightFoot = Time.timeSinceLevelLoad;

			const float rayOriginOffset = 0.5f; //origin near bottom of collider, so need a fudge factor up away from there
			const float rayDepth = 0.5f; //how far down will we look for ground?
			const float totalRayLen = rayOriginOffset + rayDepth;

			Ray ray = new Ray(rightFoot.position + Vector3.up * rayOriginOffset, Vector3.down);
			Debug.DrawLine(ray.origin, ray.origin + ray.direction * totalRayLen, Color.green);
			RaycastHit hit;
			//Cast ray and look for ground. If ground is close, then transition out of falling animation
			if (Physics.Raycast(ray, out hit, totalRayLen))
			{
				AudioSource.PlayClipAtPoint (FootStepSounds [0], leftFoot.position);
			}
		}

		//Otherwise, just fall through and ignore the footstep callback that wanted to play
	}
    public float applyRootMotionBoolean = 0;


    //Update whenever physics updates with FixedUpdate()
    //Updating the animator here should coincide with "Animate Physics"
    //setting in Animator component under the Inspector
    void FixedUpdate()
    {
		Transform handTransform = this.transform.Find("mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:LeftShoulder/mixamorig:LeftArm/mixamorig:LeftForeArm/mixamorig:LeftHand");
		Vector3 mousePos = new Vector3();
		mousePos = Input.mousePosition;
		mousePos.z = 17f;
//		Debug.Log ("mousePos: " + mousePos);

		Vector3 sp = new Vector3 ();
		sp =(Camera.main.ScreenToWorldPoint(mousePos));
//		Debug.Log ("screen position" + sp);
//		sp = Vector3.Normalize (sp);

//		Debug.Log ("screen position normalized" + sp);
		Debug.DrawLine (handTransform.position, sp);
		throwMeter.value = (mousePos.y - screenPadding) / (Screen.height - 2 * screenPadding);
        //GetAxisRaw() so we can do filtering here instead of the InputManager
        float h = Input.GetAxisRaw("Horizontal");// setup h variable as our horizontal input axis
        float v = Input.GetAxisRaw("Vertical");	// setup v variables as our vertical input axis


        //enforce circular joystick mapping which should coincide with circular blendtree positions
        Vector2 vec = Vector2.ClampMagnitude(new Vector2(h, v), 1.0f);

        h = vec.x;
        v = vec.y;


        //do some filtering of our input as well as clamp to a speed limit
        filteredForwardInput = Mathf.Clamp(Mathf.Lerp(filteredForwardInput, v, 
                Time.deltaTime * forwardInputFilter), -forwardSpeedLimit, forwardSpeedLimit);
        
        filteredTurnInput = Mathf.Lerp(filteredTurnInput, h, 
            Time.deltaTime * turnInputFilter);

        //speed boosting
        if (speedModifier > 1)
        {
            anim.speed = speedModifier;
            StartCoroutine(resetSpeed());
        }
        else
        {
            anim.speed = speedModifier;
        }

        //finally pass the processed input values to the animator
        anim.SetFloat("velx", filteredTurnInput);	// set our animator's float parameter 'Speed' equal to the vertical input axis				
        anim.SetFloat("vely", filteredForwardInput); // set our animator's float parameter 'Direction' equal to the horizontal input axis		

        if (Input.GetButtonDown("Fire1")) //normally left-ctrl on keyboard
            anim.SetTrigger("throw");

		if (Input.GetKey (KeyCode.LeftShift))
			anim.SetBool ("crouching", true);
		else
			anim.SetBool ("crouching", false);
        Debug.Log(groundContacts);
        if (Input.GetKey(KeyCode.Space) && IsGrounded)
        {
            float lastForwardSign;
            anim.applyRootMotion = false;
            if (filteredForwardInput < 0)
                lastForwardSign = -1;
            else if(filteredForwardInput > 0)
            {
                lastForwardSign = 1;
            } else
            {
                lastForwardSign = 0;
            }

			float vely = filteredTurnInput * Mathf.Cos (Mathf.Deg2Rad * this.transform.localEulerAngles.y);
			float velx = filteredForwardInput * Mathf.Sin (Mathf.Deg2Rad * this.transform.localEulerAngles.y);

			rbody.AddForce(new Vector3(velx, 3, vely), ForceMode.VelocityChange);
        }
            
        bool isFalling = !IsGrounded;

        if (isFalling)
        {

            const float rayOriginOffset = .1f; //origin near bottom of collider, so need a fudge factor up away from there
            const float rayDepth = 2f; //how far down will we look for ground?
            const float totalRayLen = rayOriginOffset + rayDepth;

            Ray ray = new Ray(this.transform.position + Vector3.up * rayOriginOffset, Vector3.down);
            
            anim.applyRootMotion = false;
            applyRootMotionBoolean++;

            //visualize ray in the editor
            //I'm using DrawLine because Debug.DrawRay() doesn't allow setting ray length past a certain size
            Debug.DrawLine(ray.origin, ray.origin + ray.direction * totalRayLen, Color.green);

            RaycastHit hit;


            //Cast ray and look for ground. If ground is close, then transition out of falling animation
            if (Physics.Raycast(ray, out hit, totalRayLen))
            {
				//Debug.Log (hit.collider.gameObject);
//                if (hit.collider.gameObject.CompareTag("ground") || hit.collider.CompareTag("grass") || hit.collider.CompareTag("sand") || hit.collider.CompareTag("metal") || hit.collider.CompareTag("water"))
				if (hit.collider.gameObject.CompareTag("ground"))
				{
                    isFalling = false; //turning falling back off because we are close to the ground
                    anim.applyRootMotion = true;
                    //draw an X that denotes where ray hit
                    /*const float ZBufFix = 0.01f;
                    const float edgeSize = 0.2f;
                    Color col = Color.red;

                    Debug.DrawRay(hit.point + Vector3.up * ZBufFix, Vector3.forward * edgeSize, col);
                    Debug.DrawRay(hit.point + Vector3.up * ZBufFix, Vector3.left * edgeSize, col);
                    Debug.DrawRay(hit.point + Vector3.up * ZBufFix, Vector3.right * edgeSize, col);
                    Debug.DrawRay(hit.point + Vector3.up * ZBufFix, Vector3.back * edgeSize, col);*/
                }
            }
        }
        if (bushesIn > 0)
        {
            print("player is hidden");
            hidden = true;
        } else
        {
            hidden = false;
        }

		//Code for footstep sounds
		float footValue = anim.GetFloat("foot");

		if (footValue > forwardSpeedLimit * 0.8) {
			leftFootStep ();
		} else if (footValue < forwardSpeedLimit * -0.8) {
			rightFootStep ();
		} 

        anim.SetBool("isFalling", isFalling);

        sockText.text = "SOCKS: " + curNumSocks.ToString() + "/20";
        if (curNumSocks <= 5)
        {
            sockText.color = Color.red;
        }
    }

    //reset speed after obtaining speed boost
    IEnumerator resetSpeed()
    {
        // wait some seconds
        yield return new WaitForSeconds(speedBoostDuration);
        // return to normal speed
        speedModifier = 1;
        Debug.Log("boost ended");
    }

    //This is a physics callback
    void OnCollisionEnter(Collision collision)
    {

//        if (collision.transform.gameObject.tag == "ground" || collision.transform.gameObject.tag == "wood" || collision.transform.gameObject.tag == "grass" || collision.transform.gameObject.tag == "sand" || collision.transform.gameObject.tag == "metal" || collision.transform.gameObject.tag == "water")
		/*if (collision.transform.gameObject.tag == "ground")
		{
            ++groundContacts;

            //Debug.Log("Player hit the ground at: " + collision.impulse.magnitude);

            if (collision.impulse.magnitude > 100f)
            {               
//                EventManager.TriggerEvent<PlayerLandsEvent, Vector3>(collision.contacts[0].point);
            }
        }*/

		if (collision.transform.gameObject.tag == "boundary") {
			AccessDenied.SetActive (true);
			noAccessSound.Play ();
		}

        //Debug.Log(collision.gameObject.tag);
        


    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.gameObject.tag == "ground")
        {
            ++groundContacts;
        }

        if (other.transform.gameObject.tag == "ammo")
        {
            curNumSocks = curNumSocks + 5;
            if (curNumSocks > startingNumSocks)
            {
                curNumSocks = startingNumSocks;
            }
        }

        if (other.transform.gameObject.tag == "hidable")
        {
            print("player hit bush");
            bushesIn = bushesIn + 1;
			EventManager.TriggerEvent<BushSoundEvent, Vector3>(other.transform.position);
        }

        if (other.transform.gameObject.tag == "speed boost")
        {
            speedModifier = other.GetComponent<PickupSpeedBoostScript>().speedBoostModifier;
            speedBoostDuration = other.GetComponent<PickupSpeedBoostScript>().speedBoostDuration;
        }
    }

    //Creating ball based off an empty object in the character's hand
    public GameObject ball;
	GameObject ballClone;

	public void throwing()
	{
        // make a ball that's attached to the parent's hand
        if (curNumSocks > 0) {
            curNumSocks--;
			Transform handLocation = this.transform.Find ("mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:LeftShoulder/mixamorig:LeftArm/mixamorig:LeftForeArm/mixamorig:LeftHand");

			handLocation.position = new Vector3 (0f, .5f, 0f) + handLocation.position;

			ballClone = (GameObject)Instantiate(ball, handLocation.position, handLocation.rotation);
        }
		
	}
	// instant when arm is at apex
	public void release()
	{
		ballClone.transform.parent = null;
		Rigidbody bcRigidBody = (Rigidbody)ballClone.GetComponent<Rigidbody> ();
		bcRigidBody.isKinematic = false;

		Vector3 mousePos = Input.mousePosition;
		bcRigidBody.AddForce(new Vector3(throwMeter.value * throwPower * Mathf.Sin(Mathf.Deg2Rad * this.transform.localEulerAngles.y), 2 + throwMeter.value * 3, throwMeter.value * throwPower * Mathf.Cos(Mathf.Deg2Rad * this.transform.localEulerAngles.y)), ForceMode.Impulse);
		EventManager.TriggerEvent<PlayerLandsEvent, Vector3>(this.transform.position);
	}

    //This is a physics callback
    void OnCollisionExit(Collision collision)
    {
        Debug.Log("CollisionExit: " + collision.transform.gameObject.tag);
        
		if (collision.transform.tag == "boundary") {
			AccessDenied.SetActive (false);
		}

        
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.transform.gameObject.tag == "ground")
        {
            --groundContacts;
            anim.applyRootMotion = true;
        }

        if (other.transform.gameObject.tag == "hidable")
        {
            bushesIn = bushesIn - 1;
        }

    }


    void OnAnimatorMove()
    {
        if (IsGrounded)
        {
         	//use root motion as is if on the ground		
            this.transform.position = anim.rootPosition;

        }
        else
        {
            //Simple trick to keep model from climbing other rigidbodies that aren't the ground
            this.transform.position = new Vector3(anim.rootPosition.x, this.transform.position.y, anim.rootPosition.z);
        }

        //use rotational root motion as is
        this.transform.rotation = anim.rootRotation;
        				
    }
}
