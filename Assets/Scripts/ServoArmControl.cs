using UnityEngine;

public class ServoArmControl : MonoBehaviour
{
    public GameObject mainController;

    private MainControl MainControlClass;
    private HingeJoint2D jointRef;
    private JointMotor2D motorRef;
    public int maxTorque = 10000;


    void Start()
    {
        MainControlClass = mainController.GetComponent<MainControl>();

        //Reference 2D hinge component attached to the platform
        jointRef = GetComponent<HingeJoint2D>();
        motorRef = jointRef.motor;

        motorRef.maxMotorTorque = maxTorque;
        motorRef.motorSpeed = 0;
        //motorRef.freeSpin = false;
        jointRef.motor = motorRef;
        jointRef.useMotor = true;


    }
    
    private void FixedUpdate()
    {
        float desiredSpeed;

        desiredSpeed = MainControlClass.motorSpeed;
        SetMotorSpeed(desiredSpeed);

    }

    private void SetMotorSpeed(float speed)
    { 
        motorRef.motorSpeed = speed;
        jointRef.motor = motorRef;
    }

}

