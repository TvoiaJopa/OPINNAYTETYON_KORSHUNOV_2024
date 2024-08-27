using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RollObject : MonoBehaviour
{
    // Speed of rotation
    // Speed of rotation
    public bool activeOn = true;
    public float rotationSpeed = 90f;

    // Scale factor to apply after rotating a full 360 degrees
    public float scaleChange = -1f;

    public float totalRotation = 0f;

    // Target rotation
    public Quaternion targetRotation;

    public float animspeed = 1f;
    public bool scaled;
    public bool roatteRight;

    public bool changeScale;
    public int newScale;
    public float changeScaleSpeed = 1;
    // Flag to indicate if rotation is in progress
    private bool isRotating = false;
    public float straSize;

    public PuzzleController controller;
    public int num;

    void Start()
    {
        straSize = transform.localScale.x;
        // Randomly rotate the object at startup
        int randomRotationCount = Random.Range(1, 5); // Randomly select rotation count from 1 to 4 (90, 180, 270, 360)
        float randomRotation = randomRotationCount * 90f;
        transform.Rotate(Vector3.forward * randomRotation);
        totalRotation = randomRotation;

        // Randomly change scale to -1
        if (Random.value < 0.5f)
        {
            ScaleObject();
        }
        CheckComplited();
    }

    public void CheckComplited()
    {
        float rotationThreshold = 0.1f; // Adjust as needed
        float scaleThreshold = 0.01f; // Adjust as needed

        //Debug.Log("CheckComplited " + (Mathf.Abs(transform.localRotation.eulerAngles.z) < rotationThreshold) + " / " + (transform.localScale.x > scaleThreshold));

        if (Mathf.Abs(transform.localRotation.eulerAngles.z) < rotationThreshold && transform.localScale.x > scaleThreshold)
        {
            activeOn = false;
        }
        else
        {
            activeOn = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (activeOn && !changeScale && !isRotating)
        {
            // Check for mouse click on collider
            if (Input.GetMouseButtonDown(0)) // Left mouse button
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit) && hit.collider.gameObject == gameObject)
                {
                    RotateObjectBy90();
                }
            }
            else if (Input.GetMouseButtonDown(1)) // Right mouse button
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit) && hit.collider.gameObject == gameObject)
                {
                    RotateObjectByMinus90();
                }
            }
        }

        /* if (changeScale)
         {
             if ((int)transform.localScale.x != newScale* straSize)
             {
                 Vector3 currentScale = transform.localScale;
                 currentScale.x += Time.deltaTime * currentScale.x*newScale / changeScaleSpeed;
                 transform.localScale = currentScale;
             }
             else
             {
                 changeScale = false;
                 Vector3 currentScale = transform.localScale;
                 currentScale.x = newScale;
                 transform.localScale = currentScale;
             }
         }*/
        // Rotate object towards the target rotation


        if (isRotating)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime * animspeed);

            // Check if the target rotation is reached
            if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
            {
                transform.rotation = targetRotation;
                isRotating = false;
                totalRotation = Mathf.RoundToInt(transform.rotation.eulerAngles.z);
                CheckComplited();
            }
        }

        if (changeScale)
        {
            Vector3 currentScale = transform.localScale;
            float targetScaleX = newScale * straSize;
            float scaleChange = Time.deltaTime * changeScaleSpeed * Mathf.Sign(targetScaleX - currentScale.x);

            // Apply scale change
            currentScale.x += scaleChange;
            transform.localScale = currentScale;

            // Check if the scaling is completed
            if (Mathf.Abs(targetScaleX - currentScale.x) < 1f)
            {
                currentScale.x = targetScaleX;
                transform.localScale = currentScale;
                changeScale = false;
                scaled = true;

                CheckComplited();
            }
        }

    }
    // Rotate the object by 90 degrees around the z-axis
    public void RotateObjectBy90()
    {
        if (!isRotating)
        {
            targetRotation = Quaternion.Euler(0, 0, totalRotation + 90f);
            isRotating = true;
            scaled = false;
        }
    }

    // Rotate the object by -90 degrees around the z-axis
    public void RotateObjectByMinus90()
    {
        if (!isRotating)
        {
            targetRotation = Quaternion.Euler(0, 0, totalRotation - 90f);
            isRotating = true;
            scaled = false;
        }
    }

  
    // Scale the object by applying the scale change factor
    void ScaleObject()
    {
        Vector3 currentScale = transform.localScale;
        currentScale.x = currentScale.x * -1;
        transform.localScale = currentScale;
        scaled = true;

    }
}
