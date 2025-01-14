using UnityEngine;

public class PlayerControllerIsometric : MonoBehaviour
{
	public float speed = 5f;
    public float speedBuff = 1f;

    [HideInInspector]
    public bool canMove;

    private Rigidbody2D rb;
	private Animator anim;
	private Vector2 moveVelocity;
    private bool isAttacking = false;

    private void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
        canMove = true;
    }

    private void Update()
    {
        anim.speed = speedBuff;
        if (canMove)
        {
            anim.SetBool("isHurt", false);

            if (Input.GetAxisRaw("attack") != 0 || Input.GetMouseButtonDown(0))
            {
                isAttacking = true;
            }
            else
            {
                // Movements
                moveVelocity = new Vector2(Input.GetAxisRaw("HorizontalMovement"), Input.GetAxisRaw("VerticalMovement")).normalized * speed * speedBuff;
                if (moveVelocity == Vector2.zero)
                {
                    anim.SetBool("isMoving", false);
                }
                else
                {
                    anim.SetBool("isMoving", true);
                    anim.SetFloat("moveX", moveVelocity.x / speed * speedBuff);
                    anim.SetFloat("moveY", moveVelocity.y / speed * speedBuff);
                    if (moveVelocity.x > 0)
                    {
                        transform.rotation = Quaternion.Euler(0f, 180f, 0f);
                    }
                    else if (moveVelocity.x < 0)
                    {
                        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                    }
                }
            }
        }
        else
        {
            anim.SetBool("isHurt", true);
        }

        if(isAttacking)
        {
            if(anim.GetBool("isAttacking"))
            {
                isAttacking = false;
                anim.SetBool("isAttacking", false);
            }
            else
            {
                anim.SetBool("isAttacking", true);
            }
        }
	}

	private void FixedUpdate()
	{
        if (canMove && !isAttacking)
        {
            rb.velocity = moveVelocity;
        }
	}
}
