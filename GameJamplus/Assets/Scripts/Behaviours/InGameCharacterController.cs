using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameCharacterController : MonoBehaviour {

	public PlayerBehaviour controllingPlayer;
	public float inputDelay = 0.1f;
	public float forwardVel = 12;
	public float upVel = 5f;
	public float slideRotateVel = 2;
	public float normalRotateVel = 5;
	private float slidingVel;

	Quaternion targetRotation;
	Rigidbody rBody;
	float horizontalInput, verticalInput;

	private bool isGrounded = false;
	private bool isSliding = false;

	public bool isControlledByPlayer = false;
	public Joystick joystick;
	public Color color;

	Ray raycast;
	RaycastHit hit = new RaycastHit();

	int secondsInTheAir = 0;
	public Quaternion TargetRotation
	{
		get {return targetRotation;}
	}
	// Use this for initialization
	void Start () {
		joystick = new Joystick();
		targetRotation = transform.rotation;
		if(GetComponent<Rigidbody>())
			rBody = GetComponent<Rigidbody>();
		else
			Debug.LogError("Character needs a rb");
	
		horizontalInput = verticalInput = 0;
	}

	
	// Update is called once per frame
	void Update () {
		GetInput();
			Turn();
		if(isSliding)
			Slide();		
		if(isGrounded)
			Run();
			
	}

	void GetInput()
	{
		horizontalInput = Input.GetAxis(joystick.input.Horizontal);
		verticalInput = Input.GetAxis(joystick.input.Vertical);
	}

	/// <summary>
	/// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
	/// </summary>
	void FixedUpdate()
	{
		if(rBody.transform.position.y > -7.3f){
			slidingVel = upVel;
		}else{
			slidingVel = forwardVel;
		}
		if(rBody.velocity == Vector3.zero && InGameManager.Instance.HasGameStarted()){
			Debug.Log("velocity 0");
			rBody.transform.position = new Vector3(rBody.transform.position.x, rBody.transform.position.y +0.3f, rBody.transform.position.z);
		}
	}

	void Run()
	{
		if (Mathf.Abs(horizontalInput) > inputDelay || Mathf.Abs(verticalInput) > inputDelay) {
			//move

				if(IsFacingRightDirection())
					rBody.velocity = new Vector3(verticalInput, 0f, horizontalInput) * forwardVel;
				else
					rBody.velocity = new Vector3(verticalInput, 0f, horizontalInput) * forwardVel / 10;
		}
		else {
			rBody.velocity = Vector3.zero;
		}
	}

	bool IsFacingRightDirection()
	{
		float heading = Mathf.Atan2(verticalInput, horizontalInput);
		float charFrontHeading = Mathf.Atan2(transform.forward.z, -transform.forward.x);
		if(Mathf.Abs(charFrontHeading) - Mathf.Abs(heading) <= 0.5f)
			return true;
		else
			return false;
	}

	void Slide()
	{

		RotateCharacterToGround();
		if(InGameManager.Instance.HasGameStarted() && isControlledByPlayer)
			rBody.velocity = transform.right * slidingVel;

	}

	void RotateCharacterToGround(){
		Vector3 fwd = transform.TransformDirection(Vector3.right);
		Debug.DrawRay(transform.position,fwd*3f, Color.green);
		if (Physics.Raycast(transform.position, fwd, out hit, 3f))
     	{
			Debug.DrawLine(hit.transform.localPosition, hit.normal*3f,Color.red, 3f);
			if(Vector3.Angle(transform.up, hit.normal) <= 60f){
				Debug.Log(Vector3.Angle(transform.up, hit.normal));	
				transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;}
			else if(Vector3.Angle(transform.up, hit.normal) >= 90f && hit.transform.gameObject.tag == "Wall"){
				transform.rotation = Quaternion.FromToRotation(transform.right, hit.normal) * transform.rotation;
			} 
		}else{
			Vector3 dwn = transform.TransformDirection(Vector3.down);
			if (Physics.Raycast(transform.position, dwn, out hit, 3f)){
				transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
			}
			else{
				// Vector3 newRight = new Vector3(transform.right.x, -transform.right.y, transform.right.z);
				// transform.rotation = Quaternion.FromToRotation(transform.right, newRight) *transform.rotation;
			}

			
		}
	}

	void Turn()
	{
		if (Mathf.Abs(horizontalInput) > inputDelay || Mathf.Abs(verticalInput) > inputDelay) {
			//move
			float rotateVel = 0;
			if(isGrounded)
				rotateVel = normalRotateVel;
			if(isSliding)
				rotateVel = slideRotateVel;
			
			float heading = Mathf.Atan2(verticalInput, horizontalInput);
			targetRotation = Quaternion.Lerp (transform.rotation, Quaternion.Euler (new Vector3 (0, -heading*Mathf.Rad2Deg,0 )), Time.deltaTime * rotateVel);

		
		}
		transform.rotation = targetRotation;
	}


	/// <summary>
	/// OnCollisionEnter is called when this collider/rigidbody has begun
	/// touching another rigidbody/collider.
	/// </summary>
	/// <param name="col">The Collision data associated with this collision.</param>
	void OnCollisionEnter(Collision col)
	{
		if(col.gameObject.layer == 9){
			isGrounded = true;
			isSliding = false;
		}
		if(col.gameObject.layer == 10){
			isSliding = true;
			isGrounded = false;
		}
		//Point cube
		if(col.gameObject.layer == 11){
			controllingPlayer.points++;
			Destroy(col.gameObject);
		}
	}

	/// <summary>
	/// OnCollisionExit is called when this collider/rigidbody has
	/// stopped touching another rigidbody/collider.
	/// </summary>
	/// <param name="col">The Collision data associated with this collision.</param>
	void OnCollisionExit(Collision col)
	{
		if(col.gameObject.layer == 9){
			isGrounded = false;
		}
		if(col.gameObject.layer == 10){
			//rBody.velocity = Vector3.zero;
			isSliding = false;
		}
	}
}
