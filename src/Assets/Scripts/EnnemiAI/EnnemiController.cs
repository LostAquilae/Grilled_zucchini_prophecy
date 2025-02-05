using UnityEngine;

[RequireComponent(typeof(FieldOfView))]
public class EnnemiController : MonoBehaviour
{
    // Public
    [Header("Ennemi")]
    public float distanceMinOfTarget;
    public bool canMove;

    [Header("Speed")]
	public float walkSpeed = 2.5f;
	public float runSpeed = 3.5f;

	[Header("times")]
	public float minStopTime = 1f;
	public float maxStopTime = 3f;

	[Header("Spots")]
	public Transform[] patrolSpots;
	public bool randomSpots = false;
    public bool circlePatrol = true;

    [Header("Faces")]
    public SpriteRenderer ennemiFace;
    public Sprite normalFace;
    public Sprite chaseFace;
    public Sprite attackFace;
    public Sprite hurtFace;
    public Sprite lookForFace;

    // Private
    private Rigidbody2D rb;

    private Animator anim;

	private FieldOfView FOW;
	private Transform target;

	private int directionSpot = 1;
	private int nextSpot;

	private float waitCounter;

	private Vector2 moveVelocity;
	private Vector3 lastTargetPosition;

	private bool isChasing;

	void Start ()
	{
		rb = GetComponent<Rigidbody2D>();
		FOW = GetComponent<FieldOfView>();
        anim = GetComponent<Animator>();
        canMove = true;
	}

	void Update ()
    {
        if (canMove)
        {
            rb.angularVelocity = 0;
            target = FOW.target;

            if (target != null)
                isChasing = true;

            /* --- Patrol mode --- */
            if (!isChasing)
            {
                ennemiFace.sprite = normalFace;
                moveVelocity = Vector2.zero;

                if (Vector2.Distance(transform.position, patrolSpots[nextSpot].position) < 0.1f) // Is on spot
                {
                    if (randomSpots)
                    {
                        nextSpot = Random.Range(0, patrolSpots.Length - 1);
                    }
                    else
                    {
                        nextSpot += directionSpot;
                        if (nextSpot < 0 || nextSpot > patrolSpots.Length - 1)
                        {
                            if (circlePatrol)
                            {
                                nextSpot = 0;
                            }
                            else
                            {
                                directionSpot = -directionSpot;
                                nextSpot += directionSpot * 2;
                            }
                        }
                    }

                    anim.SetBool("isWalking", false);
                    waitCounter = Random.Range(minStopTime, maxStopTime);
                }
                else    // Go to spot
                {
                    if (waitCounter <= 0)
                    {
                        anim.SetBool("isWalking", true);
                        moveVelocity = new Vector2(patrolSpots[nextSpot].position.x - transform.position.x, patrolSpots[nextSpot].position.y - transform.position.y).normalized * walkSpeed;
                    }
                    else
                    {
                        waitCounter -= Time.deltaTime;
                    }
                }
            }

            /* --- Chasing target --- */
            else
            {
                if (target != null)
                {
                    if (Vector2.Distance(transform.position, lastTargetPosition) < 0.1f)
                    {
                        anim.speed = 1;
                        anim.SetBool("isWalking", false);
                        lastTargetPosition = target.position;
                        ennemiFace.sprite = attackFace;
                    }
                    else
                    {
                        lastTargetPosition = target.position;
                        ennemiFace.sprite = chaseFace;
                        if (Vector2.Distance(transform.position, target.transform.position) > distanceMinOfTarget)
                        {
                            anim.speed = 2;
                            anim.SetBool("isWalking", true);
                            moveVelocity = new Vector2(target.position.x - transform.position.x, target.position.y - transform.position.y).normalized * runSpeed;
                        }
                        else
                        {
                            anim.speed = 1;
                            anim.SetBool("isAttacking", true);
                            moveVelocity = Vector2.zero;
                        }
                    }
                }
                else
                {
                    ennemiFace.sprite = lookForFace;
                    anim.speed = 1;
                    if (Vector2.Distance(transform.position, lastTargetPosition) < 0.1f)
                    {
                        isChasing = false;
                        float minDistance = Vector3.Distance(transform.position, patrolSpots[0].position);
                        for (int i = 0; i < patrolSpots.Length; i++)
                        {
                            if (Vector2.Distance(transform.position, patrolSpots[i].position) < minDistance)
                            {
                                nextSpot = i;
                                minDistance = Vector3.Distance(transform.position, patrolSpots[i].position);
                            }
                        }
                        anim.SetBool("isWalking", false);
                        waitCounter = Random.Range(minStopTime, maxStopTime);
                    }
                    else
                    {
                        moveVelocity = new Vector2(lastTargetPosition.x - transform.position.x, lastTargetPosition.y - transform.position.y).normalized * walkSpeed;
                    }
                }
            }
        }
	}

	void FixedUpdate()
	{
        if (canMove)
        {
            if (moveVelocity.x > 0)
            {
                transform.rotation = Quaternion.Euler(0f, 180f, 0f);
                if (ennemiFace.transform.localPosition.z < 0)
                    ennemiFace.transform.localPosition = new Vector3(ennemiFace.transform.localPosition.x, ennemiFace.transform.localPosition.y, -ennemiFace.transform.localPosition.z);
            }
            else if (moveVelocity.x < 0)
            {
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                if (ennemiFace.transform.localPosition.z > 0)
                    ennemiFace.transform.localPosition = new Vector3(ennemiFace.transform.localPosition.x, ennemiFace.transform.localPosition.y, -ennemiFace.transform.localPosition.z);
            }
            rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);
        }
	}

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if(target != null)
        {
            Gizmos.DrawWireSphere(target.position, 0.02f);
        }
    }
}
