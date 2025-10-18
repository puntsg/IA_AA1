using System.Collections.Generic;
using UnityEngine;

public class FlockManager : MonoBehaviour
{
    public List<GameObject> flockingGameObjects;
    public List<Flocking> flockingObjects = new List<Flocking>();
    public static FlockManager instance;

    private void Awake()
    {
        instance = this;
        flockingObjects.Clear();

        foreach (GameObject go in flockingGameObjects)
        {
            if (!go) continue;
            var movement = go.GetComponent<Movement>();
            if (movement == null || movement.movingBehaviours == null) continue;

            foreach (var mt in movement.movingBehaviours)
            {
                if (mt != null && mt.movementBehaviour is Flocking f)
                {
                    // Asignamos el transform actual por si aún no se ha llamado desde Movement.Update
                    f.AssignTransforms(go.transform, Player.instance != null ? Player.instance.transform : null);
                    flockingObjects.Add(f);
                }
            }
        }
    }
}
