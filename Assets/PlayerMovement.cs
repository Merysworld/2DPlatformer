using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D body;
    private BoxCollider2D boxCollider;
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;

    [SerializeField] private float dashSpeed = 15f;

    private bool isDashing = false;
    


    private float wallJumpCooldown;
    private float horizontalInput;

    private void Awake(){
        body = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update(){

    horizontalInput = Input.GetAxis("Horizontal");

    if (horizontalInput > 0.01f)
        transform.localScale = Vector3.one;
    else if (horizontalInput < -0.01f)
        transform.localScale = new Vector3(-1, 1, 1);


    if (Input.GetKey(KeyCode.LeftShift))
    {
        isDashing = true;
        body.velocity = new Vector2(transform.localScale.x * dashSpeed, 0f);
        body.gravityScale = 0; 
        return; 
    }
    else if (isDashing)
    {
        isDashing = false;
        body.gravityScale = 3; 
    }

    if (wallJumpCooldown > 0.2f)
    {
        body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

        if (onWall() && !isGrounded())
        {
            body.gravityScale = 0;
            body.velocity = Vector2.zero;
        }
        else
            body.gravityScale = 3;

        if (Input.GetKeyDown(KeyCode.Space))
            Jump();
    }
    else
        wallJumpCooldown += Time.deltaTime;
    }

    private void Jump(){
        if(isGrounded()){
            body.velocity = new Vector2(body.velocity.x, jumpPower);
        }
        else if(onWall() && !isGrounded()){
            if(horizontalInput == 0){
                body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x)*10, 0);
                transform.localScale = new Vector3(-Mathf.Sign(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else{
                body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x)*3, 6);
            }
            wallJumpCooldown = 0;
            
        }
        
    }

    private bool isGrounded(){
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }

    private bool onWall(){
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }

}
