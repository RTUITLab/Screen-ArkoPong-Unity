﻿using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Ball : MonoBehaviour
{
    [Header("Ball parts:")] 
    [SerializeField] private Rigidbody2D rigidbody;
    [SerializeField] private CircleCollider2D collider;
    [SerializeField] private SpriteRenderer sprite;

    [Header("Settings:")]
    [SerializeField] private int speed = 0;
    [SerializeField] [Range(5, 30)] private int timeToLive = 5; //Seconds
    [SerializeField] private AudioSource audioSourceDeath;
    [SerializeField] private AudioSource audioSourceBonk;

    private int remainingTime;
    private TextScore textScore;
    private BallController controller;
        
    private void Awake()
    {
        Network.instance.onGamePaused.AddListener(() =>
        {
            rigidbody.simulated = false;
        });
        Network.instance.onGameStart.AddListener(() =>
        {
            rigidbody.simulated = true;
        });
    }

    public void OnSpawn(TextScore textScore, BallController controller)
    {
        this.textScore = textScore;
        this.controller = controller;
        addForce();
        StartCoroutine(TimeToLiveCoroutine());
    }

    //Массив возможных сил, которые рандомно назначаются объекту при начале игры или же полсе пропущенного гола
    private Vector2[] initialForces = {new Vector2(100f, 100f), new Vector2(-100f, -100f), new Vector2(-100f, 100f), new Vector2(100f, -100f)};

    private void OnCollisionEnter2D(Collision2D collision)
    {
        clearTime();
        // При попадании в левую и правую стены ставим мяч в центр и снова запускаем
        if (collision.gameObject.name == "LeftWall" || collision.gameObject.name == "RightWall")
        {
            if (collision.gameObject.name == "LeftWall")
            {
                textScore.AddRight();
            }
            else
            {
                textScore.AddLeft();
            }

            rigidbody.simulated = false;
            collider.enabled = false;
            sprite.enabled = false;

            audioSourceDeath.pitch = Random.Range(1f, 1.5f);
            audioSourceDeath.Play();
            Invoke("RemoveBall", 1f);
        }
        else
        {
            audioSourceBonk.pitch = Random.Range(1f, 1.5f);
            audioSourceBonk.Play();
        }
    }

    public void addForce()
    {
        //Выбираем одну из сил, которые также задают траекторию движения (Random.Range(int,int) берет правую границу не включительно)
        GetComponent<Rigidbody2D>().AddForce(initialForces[Random.Range(0, 4)]);
    }

    private void RemoveBall()
    {
        controller.ballDestroyed();
        Destroy(gameObject);
    }

    private IEnumerator TimeToLiveCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            if (--remainingTime == 0)
            {
                RemoveBall();
            }
        }
    }

    private void clearTime()
    {
        remainingTime = timeToLive;
    }
}
