﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Movement : MonoBehaviour
{
    [SerializeField] private int speed = 10;
    [SerializeField] private bool usePhysics = true;
    public float jumpSpeed = 5000f;

    private Camera _mainCamera;
    private Rigidbody _rb;
    private Controls _controls;
    private Animator _animator;
    bool canJump;
    private static readonly int IsWalking = Animator.StringToHash("isWalking");

    private void Awake()
    {
        _controls = new Controls();
    }

    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _controls.Enable();
    }

    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
        _controls.Disable();
    }

    private void Start()
    {
        _mainCamera = Camera.main;
        _rb = gameObject.GetComponent<Rigidbody>();
        _animator = gameObject.GetComponentInChildren<Animator>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Floor")
        {
            canJump = true;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag == "Floor")
        {
            canJump = false;
        }
    }

    private void Update()
    {
        if (usePhysics)
        {
            return;
        }
        
        if (_controls.Player.Move.IsPressed())
        {
            _animator.SetBool(IsWalking, true);
            Vector2 input = _controls.Player.Move.ReadValue<Vector2>();
            Vector3 target = HandleInput(input);
            Move(target);
        }
        else
        {
            _animator.SetBool(IsWalking, false);
        }

        if (_controls.Player.Run.IsPressed())
        {
            speed = 30;
            CamShake.Instance.ShakeCam(5f, 1f);
        }
        else
        {
            speed = 10;
        }

        //Movement Script
    }

    private void FixedUpdate()
    {
        if (!usePhysics)
        {
            return; 
        }

        if (_controls.Player.Jump.IsPressed() & canJump)
        {
            _rb.AddForce(1f, jumpSpeed * Time.deltaTime, 2f);
        }

        if (_controls.Player.Run.IsPressed())
        {
            speed = 30;
            CamShake.Instance.ShakeCam(10f, 1f);
        }
        else
            speed = 10;

        if (_controls.Player.Move.IsPressed())
        {
            _animator.SetBool(IsWalking, true);
            Vector2 input = _controls.Player.Move.ReadValue<Vector2>();
            Vector3 target = HandleInput(input);
            MovePhysics(target);
        }
        else
        {
            _animator.SetBool(IsWalking, false);
        }
    }

    private Vector3 HandleInput(Vector2 input)
    {
        Vector3 forward = _mainCamera.transform.forward;
        Vector3 right = _mainCamera.transform.right;

        forward.y = 0;
        right.y = 0;
        
        forward.Normalize();
        right.Normalize();

        Vector3 direction = right * input.x + forward * input.y;
        
        return transform.position + direction * speed * Time.deltaTime;
    }

    private void Move(Vector3 target)
    {
        transform.position = target;
    }

    private void MovePhysics(Vector3 target)
    {
        _rb.MovePosition(target); 
    }

    public void ResetTheGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
