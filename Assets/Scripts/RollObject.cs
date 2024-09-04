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


    }
    [SerializeField]private AudioSource audioS;
    public AudioClip audioClip;

    public void PlaySound()
    {
        audioS.PlayOneShot(audioClip);
    }

    // Rotate the object by 90 degrees around the z-axis
    public void RotateObjectBy90()
    {
        if (!isRotating)
        {
            PlaySound();
            StartRotation(Quaternion.Euler(0, 0, totalRotation + 90f));
            isRotating = true;
            scaled = false;
        }
    }

    // Rotate the object by -90 degrees around the z-axis
    public void RotateObjectByMinus90()
    {
        if (!isRotating)
        {
            PlaySound();
            StartRotation(Quaternion.Euler(0, 0, totalRotation - 90f));
            isRotating = true;
            scaled = false;
        }
    }
    public void StartRotation(Quaternion newTargetRotation)
    {
        if (isRotating) return; // Prevent starting a new rotation if already rotating
        targetRotation = newTargetRotation;
        StartCoroutine(RotationCoroutine());
    }

    private IEnumerator RotationCoroutine()
    {
        isRotating = true;

        // Continue rotating until the target rotation is reached
        while (Quaternion.Angle(transform.rotation, targetRotation) >= 0.1f)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime * animspeed);
            yield return null; // Wait for the next frame
        }

        // Ensure the final rotation is exactly the target rotation
        transform.rotation = targetRotation;
        isRotating = false;

        // Assuming totalRotation is meant to be the z component of the Euler angles
        totalRotation = Mathf.RoundToInt(transform.rotation.eulerAngles.z);

        if (Mathf.Abs(transform.localRotation.eulerAngles.z) < 0.001f)
        {
            roatteRight = false;
        }
        // Method call when rotation is complete
        CheckComplited();
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
