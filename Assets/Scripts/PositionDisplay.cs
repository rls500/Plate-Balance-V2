using UnityEngine;
using UnityEngine.UI;

public class PositionDisplay : MonoBehaviour
{
    private GameObject ball;
    private Rigidbody ballRB;
    private GameObject objectiveRect;
    private MainControl mainControlScript;

    float desiredPosition = 0;
    
    

    void Start()
    {
        objectiveRect = GameObject.Find("Target Position");

        GameObject mainController = GameObject.Find("Main Controller");
        mainControlScript = mainController.GetComponent<MainControl>();
        objectiveRect.GetComponent<SpriteRenderer>().enabled = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ShowDesiredPosition();
        
    }
    void ShowDesiredPosition()
    {
        Vector3 rectPos;

        desiredPosition = mainControlScript.desiredBallPosition;
      
        rectPos = new Vector3(desiredPosition, 0, 0.1f);

        objectiveRect.transform.position = rectPos;
    }
}
