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
	public float slowSpellVel = 2;
	private float slidingVel;

	Quaternion targetRotation;
	Rigidbody rBody;
	float horizontalInput, verticalInput;

	private bool isGrounded = false;
	private bool isSliding = false;

	public bool isControlledByPlayer = false;
	public Joystick joystick;
	public Color color;

	public GameObject plane;

	private bool stun = false;

	Ray raycast;
	RaycastHit hit = new RaycastHit();

	public AudioClip pointSound;
	public AudioClip spellGetSound;

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
		if(!InGameManager.Instance.spell2Forward)
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
		if(InGameManager.Instance.spell3Slow){
			slidingVel = slowSpellVel;
		}
		if(rBody.velocity == Vector3.zero && InGameManager.Instance.HasGameStarted()){
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
		if(InGameManager.Instance.HasGameStarted() && isControlledByPlayer && !stun)
			rBody.velocity = transform.right * slidingVel;

	}

	void RotateCharacterToGround(){
		Vector3 fwd = transform.TransformDirection(Vector3.right);
		Debug.DrawRay(transform.position,fwd*3f, Color.green);
		if (Physics.Raycast(transform.position, fwd, out hit, 2.5f))
     	{
			Debug.DrawLine(hit.transform.localPosition, hit.normal*3f,Color.red, 3f);
			if(Vector3.Angle(transform.up, hit.normal) <= 80f ){
				Debug.Log("-60");
				transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;}
			else if(Vector3.Angle(transform.up, hit.normal) >= 90f && hit.transform.gameObject.tag == "Wall"){
				Debug.Log("+90");
				transform.rotation = Quaternion.FromToRotation(transform.right, hit.normal) * transform.rotation;
			}else if(hit.transform.gameObject.tag != "item"){
				transform.rotation = Quaternion.FromToRotation(transform.right, -transform.right) * transform.rotation;
				rBody.velocity = Vector3.zero;
			} 
		}else{
			Vector3 dwn = transform.TransformDirection(Vector3.down);
			if (Physics.Raycast(transform.position, dwn, out hit, 5f)){
				transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
			}
			else{
				transform.rotation = Quaternion.FromToRotation(transform.up, plane.transform.up) * transform.rotation;
			}
			
		}
	}

	void Turn()
	{
		if (Mathf.Abs(horizontalInput) > inputDelay || Mathf.Abs(verticalInput) > inputDelay) {
			//move
			float rotateVel = 0;

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
			AudioSource.PlayClipAtPoint(pointSound, transform.position);
			gameObject.transform.GetChild(7).GetComponent<ParticleSystem>().Play();
			controllingPlayer.points++;
			Destroy(col.gameObject);
		}

		if(col.gameObject.layer == 12){
			controllingPlayer.points = controllingPlayer.points+5;
			gameObject.transform.GetChild(8).GetComponent<ParticleSystem>().Play();
			Destroy(col.gameObject);
		}

		if(col.gameObject.layer == 13){
			controllingPlayer.points = controllingPlayer.points+20;
			gameObject.transform.GetChild(9).GetComponent<ParticleSystem>().Play();
			Destroy(col.gameObject);
		}


		//Spell
		if(col.gameObject.layer == 14){
			AudioSource.PlayClipAtPoint(spellGetSound, transform.position);
			controllingPlayer.spell = col.gameObject.GetComponent<GetSpellBehaviour>().spell;
			GameUIManager.Instance.SetSkill(controllingPlayer.gameUiPosition, controllingPlayer.spell.spellName, controllingPlayer.spell.id);
			Debug.Log(controllingPlayer.spell.spellName);
			Destroy(col.gameObject);
		}

	}

	/// <summary>
	/// OnTriggerEnter is called when the Collider other enters the trigger.
	/// </summary>
	/// <param name="other">The other Collider involved in this collision.</param>
	void OnTriggerEnter(Collider col)
	{			//Bomb
		if(col.gameObject.layer == 16){
			if(col.gameObject.GetComponent<Spell4BombBehaviour>().player != controllingPlayer){
				controllingPlayer.points -= 10;
				col.gameObject.GetComponent<Spell4BombBehaviour>().player.points += 10;
				stun = true;
				rBody.velocity= new Vector3(0f, 10f, 0f);
				StartCoroutine(RotateSelf());
				Destroy(col.gameObject);
			}else{
				
			}
			
		}

	}

	private IEnumerator RotateSelf(){
		gameObject.transform.GetChild(5).GetComponentInChildren<ParticleSystem>().Play();
		int i = 0;
		while (i < 80){
			transform.Rotate(Vector3.up * 80f * Time.deltaTime);
			yield return new WaitForFixedUpdate();
			i++;
		}
		Debug.Log(stun);
		stun = false;
		gameObject.transform.GetChild(5).GetComponentInChildren<ParticleSystem>().Stop();
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
