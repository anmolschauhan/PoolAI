using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shot : MonoBehaviour
{
    Vector3[] ballLocations, bestBallLocations;
    Vector3 cueBallLocation, bestCueBallLocation;
    GameObject[] balls;
    GameObject cueBall;
    float degree = Mathf.PI / 360;
    int angle, move, bcounts;
    int[] score, bestShot;
    Vector3 destroyed;
    bool start = false, allIn, proceed = true;


    void SetBallsToOriginalLocations()
    {
        Debug.Log("Balls set to their original locations");
        balls = GameObject.FindGameObjectsWithTag("ball");
        cueBall = GameObject.FindGameObjectWithTag("cball");
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
    }
    // Use this for initialization
    void Start () {
        ballLocations = new Vector3[bcounts];
        cueBallLocation = new Vector3();
        SetBallsToOriginalLocations();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButton(0))
        {
            float angleOfShot = 40 * degree;
            Vector2 shot = new Vector2(20 * Mathf.Cos(angleOfShot), 20 * Mathf.Sin(angleOfShot));
            cueBall.GetComponent<Rigidbody2D>().velocity = shot;
        }
    }
}
