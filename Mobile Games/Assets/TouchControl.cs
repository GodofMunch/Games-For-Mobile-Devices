using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchControl : MonoBehaviour
{
    private float timer = .3f;
    private Camera myCamera;
    private Controllable selected;
    private Controllable deSelected;
    private GameObject movableObject;
    private int noSelected = 0;
    private float dragDistance = 0;
    private Vector3 initialFirstPos;
    private Vector3 initialSecondPos;
    private float initialDistance = 0;
    private const int MAX_SELECTED = 1;
    private float angleBetweenTouches;
    private float cameraMoveSpeed = .01f;
    private FingerType holdOrTap;
    private float zoomSpeed = 5;
    private Quaternion originalRotation;
    public Texture flames;
    public GameObject[] walls;
    private bool wallsAreGreen = false;
    private float colorTimer = .5f;

    // Start is called before the first frame update
    void Start()
    {
        myCamera = Camera.main;
        originalRotation = myCamera.transform.rotation;
        walls = GameObject.FindGameObjectsWithTag("Wall");
        
        for(int i = 0; i < walls.Length; i++)
            walls[i].GetComponent<Renderer>().material.color = Color.black;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.touchCount == 3)
        {
            Touch t1 = Input.GetTouch(0);
            Touch t2 = Input.GetTouch(1);
            Touch t3 = Input.GetTouch(2);

            bool startTimer = false;

            if (((t1.phase == TouchPhase.Began) || (t2.phase == TouchPhase.Began) || (t3.phase == TouchPhase.Began)) &&
                colorTimer == .5f)
            {
                startTimer = true;

                foreach (GameObject g in walls)
                {
                    if (!wallsAreGreen)
                        g.GetComponent<Renderer>().material.color = Color.green;
                    else
                        g.GetComponent<Renderer>().material.color = Color.magenta;
                }
            }

            if (startTimer)
                colorTimer -= Time.deltaTime;

            if ((t1.phase == TouchPhase.Ended) || (t2.phase == TouchPhase.Ended) || (t3.phase == TouchPhase.Ended))
            {
                colorTimer = .5f;
                wallsAreGreen = !wallsAreGreen;
                startTimer = false;
            }
        }
        
        if (Input.touchCount == 0)
            timer = .3f;
        
        if (Input.touchCount > 0)
        {
            holdOrTap = FingerType.Tap;
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                holdOrTap = FingerType.Hold;
            }
        }
        if (Input.touchCount == 1)
        {
            timer -= Time.deltaTime;
            Touch firstTouch = Input.GetTouch(0);
            Ray myRay = myCamera.ScreenPointToRay(firstTouch.position);
            Debug.DrawRay(myRay.origin, 20 * myRay.direction);
            if (firstTouch.phase == TouchPhase.Moved && holdOrTap == FingerType.Hold && noSelected == 0)
            {
                Debug.Log("Camera should about to move");
                Vector2 touchPosition = firstTouch.deltaPosition;
                myCamera.transform.Translate(-touchPosition.x * cameraMoveSpeed, -touchPosition.y * cameraMoveSpeed,0);
            }

            RaycastHit infoOnHit;
            if (Physics.Raycast(myRay, out infoOnHit))
            {
                Controllable myScript = infoOnHit.transform.GetComponent<Controllable>();
                if (myScript != selected && holdOrTap == FingerType.Tap)
                {
                    if (noSelected < MAX_SELECTED)
                    {
                        noSelected = MAX_SELECTED;
                        deSelected = myScript;
                        selected = myScript;
                    }

                    deSelected = selected;
                    deSelected.isSelected = false;
                    selected = myScript;
                    selected.isSelected = true;
                    movableObject = selected.gameObject;

                    dragDistance = Vector3.Distance(movableObject.transform.position, myCamera.transform.position);
                }
                
                if (noSelected != 0 && holdOrTap == FingerType.Hold)
                {
                    if (firstTouch.phase == TouchPhase.Began)
                    {

                    }

                    if (firstTouch.phase == TouchPhase.Moved)
                    {
                        Ray dragRay = myCamera.ScreenPointToRay(firstTouch.position);
                        Vector3 newDestination = dragRay.GetPoint(dragDistance);
                        movableObject.GetComponent<Controllable>().move(newDestination);
                    }

                    if (firstTouch.phase == TouchPhase.Ended)
                    {
                            
                    }
                }
            }
        }

        if (Input.touchCount == 2)
        {
            Touch firstTouch = Input.GetTouch(0);
            Touch secondTouch = Input.GetTouch(1);

            if (noSelected == 0)
            {
                if (firstTouch.phase == TouchPhase.Began || secondTouch.phase == TouchPhase.Began)
                {
                    initialFirstPos = firstTouch.position;
                    initialSecondPos = secondTouch.position;
                    initialDistance = Vector3.Distance(initialFirstPos, initialSecondPos);
                    Vector2 distance = secondTouch.position - firstTouch.position;

                    angleBetweenTouches = Mathf.Atan2(distance.x, distance.y);
                    Quaternion originalRotation = myCamera.transform.rotation;
                }

                if (firstTouch.phase == TouchPhase.Moved || secondTouch.phase == TouchPhase.Moved)
                {
                    Vector3 firstPos = firstTouch.position;
                    Vector3 secondPos = secondTouch.position;
                    float movingDistance = Vector3.Distance(firstTouch.position, secondTouch.position);
                    float percentageOfScale = movingDistance / initialDistance;
                    if(percentageOfScale > 1)
                        myCamera.transform.position += Vector3.forward * (Time.deltaTime * zoomSpeed);
                    else
                        myCamera.transform.position -= Vector3.forward * (Time.deltaTime * zoomSpeed);
                    
                    Vector2 newDistance = secondTouch.position - firstTouch.position;
                    float newAngleBetweenTouches = Mathf.Atan2(newDistance.x, newDistance.y);

                    float differenceInAngles = newAngleBetweenTouches - angleBetweenTouches;

                    myCamera.transform.rotation = originalRotation * Quaternion.AngleAxis(Mathf.Rad2Deg * (newAngleBetweenTouches - angleBetweenTouches), myCamera.transform.up);
                }
            }

            else
            {
                if ((firstTouch.phase == TouchPhase.Began) || (secondTouch.phase == TouchPhase.Began))
                {
                    initialFirstPos = firstTouch.position;
                    initialSecondPos = secondTouch.position;
                    initialDistance = Vector3.Distance(initialFirstPos, initialSecondPos);

                    Vector2 distance = secondTouch.position - firstTouch.position;

                    angleBetweenTouches = Mathf.Atan2(distance.x, distance.y);
                }

                if ((firstTouch.phase == TouchPhase.Moved || secondTouch.phase == TouchPhase.Moved))
                {
                    Vector3 firstPos = firstTouch.position;
                    Vector3 secondPos = secondTouch.position;
                    float movingDistance = Vector3.Distance(firstTouch.position, secondTouch.position);
                    float percentageOfScale = movingDistance / initialDistance;
                    
                    selected.GetComponent<Controllable>().scale(percentageOfScale);

                    Vector2 newDistance = secondTouch.position - firstTouch.position;
                    float newAngleBetweenTouches = Mathf.Atan2(newDistance.x, newDistance.y);

                    float differenceInAngles = newAngleBetweenTouches - angleBetweenTouches;

                    selected.GetComponent<Controllable>().rotate(differenceInAngles);
                }
            }
        }
    }

    public void setNumberSelected(int num)
    {
        noSelected = num;
    }
}