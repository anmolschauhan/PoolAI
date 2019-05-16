using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour {

    Vector3[,] ballLocationsPostBestMoves;
    Vector3[] ballLocations, bestBallLocations, cueBallLocationsPostBestMoves;
    Vector3 cueBallLocation, bestCueBallLocation;
    //GameObject[] balls;
    //GameObject cueBall;
    Transform[] balls;
    Transform cueBall;
    float degree = Mathf.PI / 360;
    int angle, move, bcounts;
    int[] score, bestShot;
    Vector3 destroyed;
    bool start = false, allIn, proceed = true;


    void SetBallsToOriginalLocations()
    {
        Debug.Log("Balls set to their original locations");
        balls[0].position = new Vector3(2, 0, 0);

        balls[1].position = new Vector3(2.3f, 0.22f, 0);
        balls[2].position = new Vector3(2.3f, -0.22f, 0);

        balls[3].position = new Vector3(2.6f, 0.44f, 0);
        balls[4].position = new Vector3(2.6f, 0, 0);
        balls[5].position = new Vector3(2.6f, -0.44f, 0);

        balls[6].position = new Vector3(2.9f, 0.66f, 0);
        balls[7].position = new Vector3(2.9f, 0.22f, 0);
        balls[8].position = new Vector3(2.9f, -0.22f, 0);
        balls[9].position = new Vector3(2.9f, -0.66f, 0);

        cueBall.position = new Vector3(-4, 0, 0);

        
    }

	// Use this for initialization
	void Start () {
        Time.timeScale = 100.0f;
        allIn = false;
        proceed = true;
        GameObject[] temp = GameObject.FindGameObjectsWithTag("ball");
        for(int i = 0; i < bcounts; i++)
        {
            balls[i] = temp[i].GetComponent<Transform>();
        }
        cueBall = GameObject.FindGameObjectWithTag("cball").GetComponent<Transform>();
        destroyed = new Vector3(50, 50, 0);
        score = new int[10];
        bestShot = new int[10];
        bcounts = 10;
        angle = -1;
        move = 0;

        ballLocationsPostBestMoves = new Vector3[100, bcounts];
        cueBallLocationsPostBestMoves = new Vector3[100];
        bestBallLocations = new Vector3[bcounts];
        ballLocations = new Vector3[bcounts];
        bestCueBallLocation = new Vector3();

        SetBallsToOriginalLocations();
        // Storing the location of all the balls
        cueBallLocation = new Vector3();
        cueBallLocation = new Vector3(-4, 0, 0);
        ballLocations[0] = new Vector3(2, 0, 0);

        ballLocations[1] = new Vector3(2.3f, 0.22f, 0);
        ballLocations[2] = new Vector3(2.3f, -0.22f, 0);

        ballLocations[3] = new Vector3(2.6f, 0.44f, 0);
        ballLocations[4] = new Vector3(2.6f, 0, 0);
        ballLocations[5] = new Vector3(2.6f, -0.44f, 0);

        ballLocations[6] = new Vector3(2.9f, 0.66f, 0);
        ballLocations[7] = new Vector3(2.9f, 0.22f, 0);
        ballLocations[8] = new Vector3(2.9f, -0.22f, 0);
        ballLocations[9] = new Vector3(2.9f, -0.66f, 0);
        cueBallLocation = cueBall.GetComponent<Transform>().position;
	}
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            proceed = true;
        }
        bool ballStopped = true;
        if (cueBall.GetComponent<Rigidbody2D>().velocity.magnitude != 0)
        {
            ballStopped = false;
        }
        else
        {
            foreach (Transform ball in balls)
            {
                Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();
                if (rb.velocity.magnitude != 0)
                {
                    ballStopped = false;
                    break;
                }
            }
        }

        if (ballStopped && proceed)
        {
            if (!allIn)
            {
                if (angle != -1)
                {
                    int ballsIn = 0;
                    Vector3 loc = cueBall.GetComponent<Transform>().position;
                    if ((loc - destroyed).magnitude < 1)
                    {
                        ballsIn = -1;
                    }
                    else
                    {
                        foreach (Transform ball in balls)
                        {
                            loc = ball.GetComponent<Transform>().position;
                            if ((loc - destroyed).magnitude < 1)
                            {
                                ballsIn++;
                            }
                        }
                    }

                    // need to store locaiton of balls for best move
                    if (ballsIn > score[move])
                    {
                        score[move] = ballsIn;
                        bestShot[move] = angle;
                        for (int i = 0; i < bcounts; i++)
                        {
                            bestBallLocations[i] = balls[i].GetComponent<Transform>().position;
                        }
                        bestCueBallLocation = cueBall.GetComponent<Transform>().position;
                    }
                }
                angle++;
                // if angle == 360 then reset angle to 0 and increase move by 1
                if (angle == 360)
                {
                    Debug.Log(move);
                    Debug.Log(bestShot[move]);
                    Debug.Log(score[move]);

                    for(int i = 0; i < bcounts; i++)
                    {
                        ballLocationsPostBestMoves[move, i] = bestBallLocations[i];
                        cueBallLocationsPostBestMoves[move] = bestCueBallLocation;
                    }

                    if (score[move] >= 10)
                    {
                        Time.timeScale = 1.0f;
                        allIn = true;
                        Debug.Log("Best moves");
                        for(int i = 0; i <= move; i++)
                        {
                            Debug.Log(bestShot[i]);
                        }
                        move = 0;
                        proceed = false;
                        SetBallsToOriginalLocations();
                        return;
                    }
                    angle = 0;
                    move++;
                    for (int i = 0; i < bcounts; i++)
                    {
                        ballLocations[i] = bestBallLocations[i];
                    }
                    cueBallLocation = bestCueBallLocation;
                }
                // reset all the balls to their locations after best shot
                cueBall.GetComponent<Transform>().position = cueBallLocation;
                for (int i = 0; i < bcounts; i++)
                {
                    balls[i].GetComponent<Transform>().position = ballLocations[i];
                }
                float angleOfShot = angle * degree;
                Vector2 shot = new Vector2(20 * Mathf.Cos(angleOfShot), 20 * Mathf.Sin(angleOfShot));
                cueBall.GetComponent<Rigidbody2D>().velocity = shot;
            }
            else
            {
                proceed = false;
                if (move != 0)
                {
                    cueBall.GetComponent<Transform>().position = cueBallLocationsPostBestMoves[move - 1];
                    for (int j = 0; j < bcounts; j++)
                    {
                        balls[j].GetComponent<Transform>().position = ballLocationsPostBestMoves[move - 1, j];
                    }
                }
                float angleOfShot = bestShot[move] * degree;
                Debug.Log(bestShot[move]);
                move++;
                Vector2 shot = new Vector2(20 * Mathf.Cos(angleOfShot), 20 * Mathf.Sin(angleOfShot));
                cueBall.GetComponent<Rigidbody2D>().velocity = shot;

            }
        }
        
    }
}
