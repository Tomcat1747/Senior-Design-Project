using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public SaveManager x;

    // Config
    [SerializeField] float runSpeed = 5f;

    // Cached Component References
    Rigidbody2D myRigidbody2D;
    Animator myAnimator;
    CapsuleCollider2D myBodyCollider;

    private void Awake()
    {
        /*var x = GameObject.FindGameObjectWithTag("SaveManager").GetComponent<SaveManager>();
        if (x is SaveManager)
        {
            x.SavingGame += OnGameSaved;
        }
        else
        {
            Debug.Log(gameObject.name + " cannot find SaveManager! x is " + x.name + ".");
        }*/

    }

    private void OnGameSaved(object sender, EventArgs e)
    {
        Debug.Log("Hello");
        // Upload Save Data
    }


    // Movement
    void Start()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
    }

    void Update()
    {
        Run();
    }

    private void Run() 
    {
        //TODO: Look into Cross Platform support
        Vector2 movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (movement != Vector2.zero) {
            myAnimator.SetFloat("XInput", movement.x);
            myAnimator.SetFloat("YInput", movement.y);
        } else {
            myAnimator.SetFloat("XInput", 0);
            myAnimator.SetFloat("YInput", 0);
        }
        myRigidbody2D.MovePosition(myRigidbody2D.position + movement * runSpeed * Time.deltaTime);
    }
}
