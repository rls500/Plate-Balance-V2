using UnityEngine;
using UnityEngine.UI;

public class SliderDisplay2 : MonoBehaviour
{
    public int sliderID = 0;

    public GameObject mainController;
    


    private RectTransform sliderTransform;

    public float barSizeLimit = 0.25f;
    
    public float maxAngle = 30;
    public Vector3 scale;

    private MainControl mainControlClass;

    private float rawValueScale;


    private void Awake()
    {

        mainControlClass = mainController.GetComponent<MainControl>();

        sliderTransform = gameObject.GetComponent<RectTransform>();

        scale = sliderTransform.localScale;
        scale.x = barSizeLimit/3;
        sliderTransform.localScale = scale;

        


    }

    void Update()
    {

        float originalValue = 0;

        switch (sliderID){
            case 0:
                originalValue = mainControlClass.positionError;
                break;
                
            case 1:
                originalValue = mainControlClass.velocityError;
                break;

            case 2:
                originalValue = mainControlClass.integralError;
                break;

            default:
                Debug.Log("Slider ID Not Recognized");
                break;

        }

        //scale so that maximum bar size is at maximum angle
        scale.x = (originalValue / maxAngle )* barSizeLimit;

        if(Mathf.Abs(scale.x) > barSizeLimit)
        {
            scale.x = (scale.x / scale.x) * barSizeLimit;
        }
        sliderTransform.localScale = scale;
        
    }
}