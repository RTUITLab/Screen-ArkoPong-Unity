using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
public class Ball : MonoBehaviour
{
    [SerializeField] private int speed = 0;
    private TextScore textScore;
    private BallController controller;

    public void OnSpawn(TextScore textScore, BallController controller)
    {
        this.textScore = textScore;
        this.controller = controller;
        addForce();
    }

    //Массив возможных сил, которые рандомно назначаются объекту при начале игры или же полсе пропущенного гола
    private Vector2[] initialForces = {new Vector2(100f, 100f), new Vector2(-100f, -100f), new Vector2(-100f, 100f), new Vector2(100f, -100f)};

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // При попадании в левую и правую стены ставим мяч в центр и снова запускаем
        if(collision.gameObject.name == "LeftWall" || collision.gameObject.name == "RightWall")
        {
            controller.ballDestroyed();
            Destroy(gameObject);
            if (collision.gameObject.name == "LeftWall")
            {
                textScore.AddRight();
            }
            else
            {
                textScore.AddLeft();
            }
        }
    }

    public void addForce()
    {
        //Выбираем одну из сил, которые также задают траекторию движения (Random.Range(int,int) берет правую границу не включительно)
        GetComponent<Rigidbody2D>().AddForce(initialForces[Random.Range(0, 4)]);
    }
}
