using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Panning : MonoBehaviour
{
    private Camera mainCamera;

    [SerializeField] private float panSpeed = 10;

    [Space(5)]
    [SerializeField]
    private SpriteRenderer leftImage;
    [SerializeField]
    private SpriteRenderer rightImage;
    private Sprite sprite;

    private float screenHeight;
    private float screenWidth;

    private float imageHeight;
    private float imageWidth;

    private void Awake()
    {
        mainCamera = Camera.main;

        sprite = leftImage.sprite;

        screenHeight = Camera.main.orthographicSize * 2;
        screenWidth = screenHeight * Camera.main.aspect;

        imageHeight = sprite.bounds.size.y;
        imageWidth = sprite.bounds.size.x;

        Vector3 startPosition = leftImage.transform.position;
        startPosition.x = leftImage.transform.position.x + imageWidth;
        rightImage.transform.position = startPosition;
    }

    void Update()
    {
        Pan();
    }

    private void Pan()
    {
        float horizontalPan = -Input.GetAxis("Horizontal") * panSpeed * Time.deltaTime;
        float verticalPan = -Input.GetAxis("Vertical") * panSpeed * Time.deltaTime;

        Vector3 leftImagePosition = leftImage.transform.position;
        leftImagePosition.x += horizontalPan;
        leftImagePosition.y += verticalPan;
        leftImage.transform.position = leftImagePosition;

        Vector3 rightImagePosition = rightImage.transform.position;
        rightImagePosition.x += horizontalPan;
        rightImagePosition.y += verticalPan;
        rightImage.transform.position = rightImagePosition;

        if ((leftImagePosition.x - imageWidth / 2) > -screenWidth / 2)
        {
            // right image needs to become a left image       
            rightImagePosition.x -= 2 * imageWidth;
            rightImage.transform.position = rightImagePosition;
            SwapImagePlaces();
        }
        else if ((rightImagePosition.x + imageWidth / 2) < screenWidth / 2)
        {
            // left image needs to become a right image
            leftImagePosition.x += 2 * imageWidth;
            leftImage.transform.position = leftImagePosition;
            SwapImagePlaces();
        }

        if (leftImagePosition.y - imageHeight / 2 > -screenHeight / 2)
        {
            leftImagePosition = leftImage.transform.position;
            rightImagePosition = rightImage.transform.position;

            leftImagePosition.y = (-screenHeight / 2) + (imageHeight / 2);
            rightImagePosition.y = (-screenHeight / 2) + (imageHeight / 2);

            leftImage.transform.position = leftImagePosition;
            rightImage.transform.position = rightImagePosition;
        }
        else if (leftImagePosition.y + imageHeight / 2 < screenHeight / 2)
        {
            leftImagePosition = leftImage.transform.position;
            rightImagePosition = rightImage.transform.position;

            leftImagePosition.y = screenHeight / 2 - imageHeight / 2;
            rightImagePosition.y = screenHeight / 2 - imageHeight / 2;

            leftImage.transform.position = leftImagePosition;
            rightImage.transform.position = rightImagePosition;
        }
    }

    private void SwapImagePlaces()
    {
        SpriteRenderer tempImage = leftImage;
        leftImage = rightImage;
        rightImage = tempImage;
    }
}
