using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Character : MonoBehaviour
{
    private int hp;
    [SerializeField] private Animator anim;
    private bool isDead => hp <= 0;
    private string currentAnimName = "idle";
    void Start()
    {
        OnInit();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public virtual void OnInit()
    {
        hp = 100;
        currentAnimName = "idle";
    }

    public virtual void OnDespawn()
    {

    }
    protected virtual void OnDeath()
    {

    }
    protected void ChangeAnim(string animName)
    {
        if (currentAnimName != animName)
        {
            anim.ResetTrigger(currentAnimName);
            currentAnimName = animName;
            anim.SetTrigger(currentAnimName);
        }
    }

    public void OnHit(int damage)
    {
        if (hp > damage)
            hp -= damage;
        else OnDeath();
    }

     
}
