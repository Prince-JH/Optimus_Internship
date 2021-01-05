﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    //플레이어 이동 관련 변수
    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float runSpeed;
    private float jumpForce = 7;
    private float speed;
    private Vector3 lastPos;

    //상태 변수
    private bool isWalk;
    private bool isRun;
    private bool isGround = true;

    //카메라
    private float lookSensitivity = 2.5f;
    private float cameraRotationLimit = 60;
    private float currentCameraRotationX = 0;

    //컴포넌트 
    [SerializeField]
    private Camera cam;
    private Rigidbody rig;
    private CapsuleCollider capsuleCollider;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        animator = GetComponent<Animator>();
        //초기화
        speed = walkSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        IsGround();
        MoveCheck();
        Move();
        Run();
        Jump();
        RotateLR();
        RotateUD();
    }
    //플레이어 이동
    private void Move()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        Vector3 moveHorizontal = transform.right * moveX;
        Vector3 moveVertical = transform.forward * moveZ;

        Vector3 velocity = (moveHorizontal + moveVertical).normalized * speed;

        rig.MovePosition(transform.position + velocity * Time.deltaTime);
        animator.SetBool("Run", isRun);
        animator.SetBool("Walk", isWalk);
    }
    //이동중인지 체크
    private void MoveCheck()
    {
        if(!isRun && isGround)
        {
            if (Vector3.Distance(lastPos, transform.position) >= 0.01)
                isWalk = true;
            else
            {
                isWalk = false;
            }
            lastPos = transform.position;
        }
    }
    //달리기
    private void Run()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            isWalk = false;
            isRun = true;
            speed = runSpeed;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isRun = false;
            speed = walkSpeed;
        }
    }
    //바닥에 있는지 확인
    private void IsGround()
    {
        isGround = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y + 0.1f);
    }
    //점프
    private void Jump()
    {
        if(Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            isWalk = false;
            rig.velocity = transform.up * jumpForce;
        }
    }
    //카메라 좌우 회전(캐릭터 회전)
    private void RotateLR()
    {
        float yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 rotateLR = new Vector3(0f, yRotation, 0f) * lookSensitivity;
        rig.MoveRotation(rig.rotation * Quaternion.Euler(rotateLR));
    }
    //카메라 위 아래 회전(카메라 회전)
    private void RotateUD()
    {
        float xRotation = Input.GetAxisRaw("Mouse Y");
        float rotateUD = xRotation * lookSensitivity;
        currentCameraRotationX -= rotateUD;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

        cam.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0, 0);
    }
}
