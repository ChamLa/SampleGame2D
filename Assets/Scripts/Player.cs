using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

/*
 * Add tile map 
 * change layer thanh ground
 * add platform effector
 */

public class Player : Character
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float speed; 
    [SerializeField] private float jumpForce;

    private bool isGrounded;
    private bool isJumping;
    private bool isAttack;
    private bool isThrow;
    private bool isDeath;
    private float horizontal;

    private Vector3 savePoint;


    private int coin = 0;




    // Start is called before the first frame update
    void Start()
    { 
        ChangeSavePoint();
        OnInit();
        
    }

    // Update is called once per frame

    private void FixedUpdate()
    {

    }

    void Update()
    {
        if (isAttack || isThrow || isDeath)
            return;

        horizontal = Input.GetAxisRaw("Horizontal");
        if (isJumping)
        {
            if (rb.velocity.y < 0)
                Fall();
            if (Mathf.Abs(horizontal) > 0.1f) 
                Run();
            return;
        }

        if (Mathf.Abs(horizontal) > 0.1f)
        {
            Run();
            return;
        }

        isGrounded = CheckGrounded();

        if (isGrounded)  //co the chay, nhay, attack, throw
        {
            if (Input.GetKeyDown(KeyCode.Space))
                Jump();
            else if (Input.GetKeyDown(KeyCode.C))
                Attack();
            else if (Input.GetKeyDown(KeyCode.V))
                Throw();
            else
            {
                ChangeAnim("idle");
                rb.velocity = Vector2.zero;
            }
        }
        //Debug.Log("current anim " + currentAnimName);

    }

    public override void OnInit()
    {
        isDeath = false;
        isAttack = false;

        transform.position = savePoint;
        ChangeAnim("idle");
    }

    public override void OnDespawn()
    {
        base.OnDespawn();
    }

    protected override void OnDeath()
    {
        base.OnDeath();
    }

    internal void ChangeSavePoint()
    {
        savePoint = transform.position;
    }

    private bool CheckGrounded()
    {
        //Debug.DrawLine(transform.position, transform.position + Vector3.down * 1.1f, Color.red);
        //Debug.Log("check grounded");

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.1f, groundLayer);
        return hit.collider != null;
    }

    private void Jump()
    {
        isJumping = true;
        ChangeAnim("jump");
        rb.AddForce(jumpForce * Vector2.up);
    }

    private void Fall()
    {
        ChangeAnim("fall");
        isJumping = false;
    }

    private void Attack()
    {
        ChangeAnim("attack");
        isAttack = true;
        Invoke(nameof(ResetAttack), 0.7f);
    }
    
    private void Throw()
    {
        ChangeAnim("throw");
        isThrow = true;
        Invoke(nameof(ResetThrow), 0.7f);
    }

    private void Run()
    {
        ChangeAnim("run");
        transform.rotation = Quaternion.Euler(new Vector3(0, horizontal > 0.00001f ? 0 : 180, 0));
        rb.velocity = new Vector2(horizontal * Time.fixedDeltaTime * speed, rb.velocity.y);
    }

    private void ResetAttack()
    {
        ChangeAnim("idle");
        isAttack = false;
    }
    private void ResetThrow()
    {
        ChangeAnim("idle");
        isThrow = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Coin")
        {
            coin++;
            Destroy(collision.gameObject); 
            //Debug.Log("coin " + collision.gameObject.name);
        }
        if(collision.tag == "DeathZone")
        {
            isDeath = true;
            ChangeAnim("die");
            Invoke(nameof(OnInit), 1.0f);
        }
    }
}
