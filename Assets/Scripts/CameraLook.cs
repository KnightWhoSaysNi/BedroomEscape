using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraLook : MonoBehaviour
{    
    private Transform myTransform;

    [SerializeField, Range(1, 100)] private float rotationSpeed = 45;
    [SerializeField, Range(-90, 0)] private float minX = -85;
    [SerializeField, Range(0, 90)] private float maxX = 85;
    private Quaternion newRotation; 
    private float minXAngle;
    private float maxXAngle;
    private float horizontalInput;
    private float verticalInput;
    private float yRotation;
    private float xRotation;

    private void Awake()
    {
        myTransform = transform;
        
        // If transform's x rotation value is 0 and the player looks down the angle is increasing and going from 0 to 360
        // If the player looks up it is decreasing and going from 360 to 0 (it doesn't start from 0 and go into the negatives)
        minXAngle = 0 + maxX;   // Down angle   0 + [0, 90]
        maxXAngle = 360 + minX; // Up angle   360 + [-90, 0]
    }

    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        if (horizontalInput != 0 || verticalInput != 0)
        {
            // Y rotation doesn't need to be capped. It's just added to the current value
            yRotation = horizontalInput  * rotationSpeed * Time.deltaTime;
            yRotation += myTransform.rotation.eulerAngles.y;

            xRotation = -verticalInput * rotationSpeed * Time.deltaTime;
            xRotation += myTransform.rotation.eulerAngles.x;            

            // Limit X rotation value if it's in illegal angle
            if (xRotation > minXAngle && xRotation < maxXAngle)
            {
                // Rotation angle is between 90 and 270 degrees, the middle being 180
                if (myTransform.rotation.eulerAngles.x < 180)
                {
                    // Angle is 90 - 180 degrees. The player tried to look down too far
                    xRotation = maxX;
                }
                else
                {    
                    // Angle is 270 - 180 degrees. The player tried to look up too far
                    xRotation = minX;
                }

            }

            newRotation = Quaternion.Euler(xRotation, yRotation, 0);
            myTransform.rotation = newRotation;
        }
    }
}
