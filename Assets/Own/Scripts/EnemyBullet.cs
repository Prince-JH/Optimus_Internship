﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private float speed = 250.0f;
    private Rigidbody rig;
    //라이트
    private GameObject light;
    private Animator lightAnimator;
    private GameObject player;


    void Start()
    {
        light = GameObject.Find("Sun");
        rig = GetComponent<Rigidbody>();
        lightAnimator = light.GetComponent<Animator>();
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        rig.AddForce(gameObject.transform.localRotation * Vector3.forward * speed + new Vector3(10, 0, 0));
        if (PlayerMove.healthPoint <= 0)
            lightAnimator.enabled = false;
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.transform.tag == "Player")
        {
            lightAnimator.SetTrigger("Hit");
            OnDamaged();
        }
    }
    private void OnDamaged()
    {
        Invoke("OffDamaged", 1);
        player.gameObject.layer = 13;
        PlayerMove.healthPoint -= 10;
        
    }
    private void OffDamaged()
    {
        player.gameObject.layer = 12;
    }
}
