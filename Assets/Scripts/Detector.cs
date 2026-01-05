using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour
{
    public List<GameObject> nearbyObjects;
    private void OnTriggerEnter2D(Collider2D collision){
        if(!nearbyObjects.Contains(collision.gameObject))
            nearbyObjects.Add(collision.gameObject);
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if(nearbyObjects.Contains(other.gameObject))
            nearbyObjects.Remove(other.gameObject);
    }
}
