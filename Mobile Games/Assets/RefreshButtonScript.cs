using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefreshButtonScript : MonoBehaviour
{
    private Controllable[] objectsInGame;
    private Vector3[] originalPositions;
    private Quaternion[] originalOrientations;
    private TouchControl god;
    
    // Start is called before the first frame update
    void Start()
    {
        objectsInGame = FindObjectsOfType<Controllable>();
        god = FindObjectOfType<TouchControl>();
        originalPositions = new Vector3[objectsInGame.Length];
        originalOrientations = new Quaternion[objectsInGame.Length];

        for (int i = 0; i < objectsInGame.Length; i++)
        {
            originalPositions[i] = objectsInGame[i].transform.position;
            originalOrientations[i] = objectsInGame[i].transform.rotation;
        }
    }
    public void refresh()
    {
        for (int i = 0; i < objectsInGame.Length; i++)
        {
            objectsInGame[i].transform.localScale = new Vector3(1,1,1);
            objectsInGame[i].transform.position = originalPositions[i];
            objectsInGame[i].transform.rotation = originalOrientations[i];
            objectsInGame[i].isSelected = false;
        }
        god.setNumberSelected(0);
    }
}
