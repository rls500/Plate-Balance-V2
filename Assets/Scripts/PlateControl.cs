using UnityEngine;

public class PlateControl : MonoBehaviour
{
    public bool ballContact = false;
    
    private void OnCollisionEnter2D(Collision2D col)
    {
        ballContact = true;
        //Debug.Log("Contact");
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        ballContact = false;
    }

}

