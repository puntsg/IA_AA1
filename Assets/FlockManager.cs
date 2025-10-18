using System.Collections.Generic;
using UnityEngine;

public class FlockManager : MonoBehaviour
{
    public List<GameObject> flockingObjects;
    public static FlockManager instance;
    private void Awake()
    {
        instance = this;
    }
    void Update()
    {
        foreach (GameObject obj in flockingObjects)
        {
            //actualizarFlocking

        }

    }
}
