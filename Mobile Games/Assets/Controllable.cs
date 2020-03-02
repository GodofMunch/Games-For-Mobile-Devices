using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class Controllable : MonoBehaviour
{
    private Renderer myRenderer;
    private Vector3 initialScale;
    private Vector3 newScale;
    private Quaternion newOrientation;
    private Quaternion originalOrientation;

    public bool isSelected;
    // Start is called before the first frame update
    void Start()
    {
        myRenderer = GetComponent<Renderer>();
        isSelected = false;
        initialScale = transform.localScale;
        originalOrientation = transform.rotation;
        newScale = initialScale;
        newOrientation = originalOrientation;
    }

    // Update is called once per frame
    void Update()
    {
        if (isSelected)
        {
            myRenderer.material.color = Color.red;
            
        }
        else
        {
            myRenderer.material.color = Color.white;
        }
    }

    public void move(Vector3 direction)
    {
        transform.position = direction;
    }

    public void scale(float scale)
    {
        if (scale >= .5 && scale <= 3.00)
        {
            transform.localScale = newScale * scale;
        }
    }
    private bool objectIsScalable()
    {
        Vector3 maxSize = new Vector3(3,3,3);
        Vector3 minSize = new Vector3(.5f,.5f,.5f);
        Vector3 actualSize = transform.localScale;
        if (actualSize.x > maxSize.x)
        {
            transform.localScale = maxSize;
            return false;
        }
        else if (actualSize.x < minSize.x)
        {
            transform.localScale = minSize;
            return false;
        }
        else return true;

    }

    public void rotate(float angle)
    {
        transform.rotation = Quaternion.AngleAxis(Mathf.Rad2Deg * (angle), transform.up);
        transform.rotation = Quaternion.AngleAxis(Mathf.Rad2Deg * (angle), transform.right);

        newOrientation = transform.rotation;
    }
}
