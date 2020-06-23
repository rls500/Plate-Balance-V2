using UnityEngine;
using System;

public class MainControl : MonoBehaviour
{
    public int controlMode = 1;
    public float setPlatformAngle = 0;
    public int maxTorque = 10000;
    [Range(-0.45f, 0.45f)]
    public float desiredBallPosition = 0;
    public Vector3 ballSpawnPosition = new Vector3(-0.003f, 0.02f, 0);

    public float PVel = 0; //Decent value 5
    public float DVel = 0;
    public float PPos = 0;  //Decent Value 10;
    public float DPos = 0; //Decent value 300;
    public float IPos = 0;
    public float motorSpeed;

    public GameObject prefabBall;
    public GameObject plate;
    public GameObject Slider;

    private GameObject ballInstance;

    private Transform ballTransform;
    private Transform plateTransform;
    private PlateControl plateControlClass;


    private float ballYBound = -10;

    private float ballXPos;
    private float ballYPos;
    //private bool ballContact;
    private float prevAngleError = 0;
    private float prevXPos;

    private bool ballLost = false;
    private bool newBallContact = true;
    
    private float ballPosITerm;


    private float maxITerm;
    private bool overrideBallPosition;

    public float positionError;
    public float velocityError;
    public float integralError;




    void Start()
    {

        //Spawn a new ball instance from the ball prefab
        ballInstance = GameObject.Find("Ball");

            //COMMENTED FOR TESTING, UNCOMMENT AFTER
            //Instantiate(prefabBall, ballSpawnPosition, Quaternion.identity);


        //Get Transform component for the ball instance (used to get position)
        ballTransform = ballInstance.GetComponent<Transform>();

        //Get Transform component for the plate (used to get plate orientation);
        plateTransform = plate.GetComponent<Transform>();
        //Get PlateControl class to access its variables
        plateControlClass = plate.GetComponent<PlateControl>();

        //Checking here to initialize previous ball position at current ball position
        CheckBallPosition();
        prevXPos = ballXPos;

       
    }


    private void FixedUpdate()
    {

        //If ball hasn't fallen, check its position
        if (!ballLost)
        {
            //Debug.Log("Checking Ball Position");
            CheckBallPosition();
        }

        //If fallen, reset plate and get new ball
        if (ballLost)
        {
            //Use PID control loop to try to get plate to 0 rotation
            SetMotorControl(true);
            //If plate orientation gets close enough to 0
            if (Mathf.Abs(GetPlatformAngle()) < 1)
            {
                //Destroy old ball instance
                Destroy(ballInstance);
                //Debug.Log("Creating new ball instance");

                //Create new ball instance at ballSpawnPosition
                ballInstance = Instantiate(prefabBall, ballSpawnPosition, Quaternion.identity);
                ballTransform = ballInstance.GetComponent<Transform>();
                newBallContact = true; //Used to fixed ball launching issues
                desiredBallPosition = 0; //reset desired position

                //Have new instance, no longer lost
                ballLost = false;
            
            }
        }


        else if (plateControlClass.ballContact)
        {
            if (newBallContact)
            {
                CheckBallPosition();
                prevXPos = ballXPos;
                newBallContact = false;
            }
            SetMotorControl(false);
        }
        
    }

    private void CheckBallPosition()
    {
        //Get ball position
        ballXPos = ballTransform.position.x;
        ballYPos = ballTransform.position.y;

        //if ball has fallen below a certain point, ball is lost
        if (ballYPos < ballYBound)
        {
            ballLost = true;
        }
    }

    public void SetMotorControl(bool overrideBallPosition)
    {
        float speed;
        float desiredPlatformAngle;
        float currentPlatformAngle;
        float angleError;
        


        //Get the current platform angle
        currentPlatformAngle = GetPlatformAngle();

        //Calculate PID Errors
        positionError = (ballXPos - desiredBallPosition)*PPos;
        velocityError = ballXPos - prevXPos;
        velocityError *= DPos;
        integralError += (ballXPos - desiredBallPosition) * IPos;

        //Set desired Platform angle based on ball position PID
        desiredPlatformAngle = positionError + velocityError + integralError;
        prevXPos = ballXPos;

        //If we want to set platform angle regaurdless of ball (for velocity gain tuning or platform resetting)
        if (overrideBallPosition)
        {
            desiredPlatformAngle = setPlatformAngle;
        }

        //Calculate the error in the current plate position based on ball position PID
        angleError = currentPlatformAngle - desiredPlatformAngle;

        //Use angular velocity PD to set motor speed
        speed = angleError * PVel + (angleError - prevAngleError) * DVel;
        prevAngleError = angleError;


        motorSpeed = speed;
        //Debug.LogFormat("Pos: {0} Angle: {1}, DesiredAngle: {2} Motor Speed: {3}", ballXPos, currentPlatformAngle, desiredPlatformAngle, speed);
    }

    public float GetPlatformAngle()
    {
        float angle;

        angle = plateTransform.eulerAngles.z;
        //This scales rotation to be CCW rotation 0:180, CW rotation 0:-180
        if (angle > 180)
        {
            angle -= 360;
        }
        return angle;
    }

    // The following 3 functions get the three slider positions from the interactable UI
    public void GetSliderValue(float value)
    {
        desiredBallPosition = value;
       
    }

    public void GetPFromInput(string P)
    {

        PPos = float.Parse(P);
    }
    public void GetIFromInput(string I)
    {

        IPos = float.Parse(I);
    }
    public void GetDFromInput(string D)
    {

        DPos = float.Parse(D);
    }
}

