using UnityEngine;

public class MovingSphere : MonoBehaviour {

	[SerializeField, Range(0f, 100f)]
	float maxSpeed = 10f;

	[SerializeField, Range(0f, 100f)]
	float maxAcceleration = 10f, maxAirAcceleration = 1f;

	[SerializeField, Range(0f, 10f)]
	float jumpHeight = 2f;

	[SerializeField, Range(0, 5)]
	int maxAirJumps = 0;


	int jumpPhase;
	Vector3 velocity, desiredVelocity;
	bool desiredJump;
	bool onGround;
	Rigidbody body;
	

	void Awake()
	{
		body = GetComponent<Rigidbody>();
	}

	void Update()
	{
		Vector2 playerInput;
		playerInput.x = Input.GetAxis("Horizontal");
		playerInput.y = Input.GetAxis("Vertical");
		playerInput = Vector2.ClampMagnitude(playerInput, 1f);
		desiredVelocity = new Vector3(playerInput.x, 0f, playerInput.y) * maxSpeed;
		desiredJump |= Input.GetButtonDown("Jump");
	}

	void FixedUpdate()
	{
		UpdateState();
		float acceleration = onGround ? maxAcceleration : maxAirAcceleration;
		float maxSpeedChange = acceleration * Time.fixedDeltaTime;
		velocity.x =
			Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);
		velocity.z =
			Mathf.MoveTowards(velocity.z, desiredVelocity.z, maxSpeedChange);
		if (desiredJump)
		{
			desiredJump = false;
			Jump();
		}
		body.velocity = velocity;
		onGround = false;
	}

	void Jump(){
		if (onGround || jumpPhase < maxAirJumps)
		{
			jumpPhase++;
			float jumpSpeed = Mathf.Sqrt(-2f * Physics.gravity.y * jumpHeight);
			if (velocity.y > 0f)
			{
				jumpSpeed = Mathf.Max(jumpSpeed - velocity.y, 0f);
			}
			velocity.y += jumpSpeed;
		}
	}
	private void OnCollisionEnter(Collision collision)
	{
		EvaluateCollision(collision);
	}	
	private void OnCollisionStay(Collision collision)
	{	
		EvaluateCollision(collision);
	}

	private void EvaluateCollision(Collision c) 
	{
		for (int i = 0; i < c.contactCount; i++)
		{
			Vector3 normal = c.GetContact(i).normal;
			onGround |= normal.y >= 0.9f;
		}
	}

	private void UpdateState() 
	{
		velocity = body.velocity;
		if (onGround) 
		{
			jumpPhase = 0;
		}
	}
}