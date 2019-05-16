using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIv2 : MonoBehaviour {
    readonly int ballsCount = 10;
    readonly float degree = Mathf.PI / 360;
    readonly Vector3 hole = new Vector3(100, 100, 0);

    bool finalGame;
    int currentMove, currentAngle;
    int[] bestMoves, bestScore;

    GameObject[] balls;
    GameObject cueBall;

    Transform[] ballsTransform;
    Transform cueBallTransform;

    Rigidbody2D[] ballsRigidBody2D;
    Rigidbody2D cueBallRigidBody2D;

    Vector3[,] ballLocationBeforeMove;
    Vector3[] cueBallLocationBeforeMove;



    void ResetBallLocationsBeforeMove(int move)
    {
        for(int i = 0; i < ballsCount; i++)
        {
            ballsTransform[i].position = ballLocationBeforeMove[move, i];
        }
        cueBallTransform.position = cueBallLocationBeforeMove[move];
    }

    void ShootCueBall(int angle, int speed)
    {
        float angleOfShot = angle * degree;
        Vector2 shot = new Vector2(speed * Mathf.Cos(angleOfShot), speed * Mathf.Sin(angleOfShot));
        cueBall.GetComponent<Rigidbody2D>().velocity = shot;
    }

    void SaveBestBallLocationsForMove(int move)
    {
        for(int i = 0; i < ballsCount; i++)
        {
            ballLocationBeforeMove[move, i] = ballsTransform[i].position;
        }
        cueBallLocationBeforeMove[move] = cueBallTransform.position;
    }

    bool BallsStopped()
    {
        bool allStopped = true;
        if (cueBallRigidBody2D.velocity.magnitude != 0)
        {
            return false;
        }
        else
        {
            for(int i = 0; i < ballsCount; i++)
            {
                if(ballsRigidBody2D[i].velocity.magnitude != 0)
                {
                    allStopped = false;
                    break;
                }
            }
        }
        return allStopped;
    }

    int GetScore()
    {
        if((cueBallTransform.position - hole).magnitude < 10)
        {
            return -10;
        }
        int tempScore = 0;
        for(int i = 0; i < ballsCount; i++)
        {
            if ((ballsTransform[i].position - hole).magnitude < 10)
            {
                tempScore++;
            }
        }
        return tempScore;
    }

	// Use this for initialization
	void Start () {
        Time.timeScale = 100.0f;
        balls = GameObject.FindGameObjectsWithTag("ball");
        cueBall = GameObject.FindGameObjectWithTag("cball");

        bestMoves = new int[100];
        bestScore = new int[100];

        ballLocationBeforeMove = new Vector3[100, ballsCount];
        cueBallLocationBeforeMove = new Vector3[100];

        ballsTransform = new Transform[ballsCount];
        ballsRigidBody2D = new Rigidbody2D[ballsCount];
        for(int i = 0; i < ballsCount; i++)
        {
            ballsTransform[i] = balls[i].GetComponent<Transform>();
            ballsRigidBody2D[i] = balls[i].GetComponent<Rigidbody2D>();
        }
        cueBallTransform = cueBall.GetComponent<Transform>();
        cueBallRigidBody2D = cueBall.GetComponent<Rigidbody2D>();
        
        cueBallTransform.position = new Vector3(-4, 0, 0);
        ballsTransform[0].position = new Vector3(2, 0, 0);

        ballsTransform[1].position = new Vector3(2.3f, 0.22f, 0);
        ballsTransform[2].position = new Vector3(2.3f, -0.22f, 0);

        ballsTransform[3].position = new Vector3(2.6f, 0.44f, 0);
        ballsTransform[4].position = new Vector3(2.6f, 0, 0);
        ballsTransform[5].position = new Vector3(2.6f, -0.44f, 0);

        ballsTransform[6].position = new Vector3(2.9f, 0.66f, 0);
        ballsTransform[7].position = new Vector3(2.9f, 0.22f, 0);
        ballsTransform[8].position = new Vector3(2.9f, -0.22f, 0);
        ballsTransform[9].position = new Vector3(2.9f, -0.66f, 0);

        SaveBestBallLocationsForMove(0);

        finalGame = false;
        currentMove = 0;
        currentAngle = -1;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            finalGame = true;
        }

        if (BallsStopped())
        {
            if (!finalGame)
            {
                if (currentAngle != -1)
                {
                    int score = GetScore();
                    if (score > bestScore[currentMove])
                    {
                        bestScore[currentMove] = score;
                        bestMoves[currentMove] = currentAngle;
                        SaveBestBallLocationsForMove(currentMove + 1);
                    }
                }
                currentAngle += 1;
                if (currentAngle >= 360)
                {
                    if (bestScore[currentMove] == 10)
                    {
                        Time.timeScale = 1.0f;
                        currentMove = 0;
                        finalGame = true;
                        ResetBallLocationsBeforeMove(0);
                        return;
                    }
                    Debug.Log("*************************************************************");
                    Debug.Log("best " + currentMove + " : " + bestMoves[currentMove] + " with score : " + bestScore[currentMove]);
                    currentAngle = 0;
                    currentMove++;
                }
                ResetBallLocationsBeforeMove(currentMove);
                ShootCueBall(currentAngle, 20);
            }
            else
            {
                ResetBallLocationsBeforeMove(currentMove);
                Debug.Log("final game shooting at angle : " + bestMoves[currentMove]);
                ShootCueBall(bestMoves[currentMove], 20);
                currentMove++;
            }
        }
	}
}
