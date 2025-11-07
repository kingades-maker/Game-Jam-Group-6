using UnityEngine;

public class SortingZone : MonoBehaviour
{
    // Must be set to "RedShape" or "BlueShape" in the Inspector
    public string acceptedShapeTag;

    void OnTriggerEnter(Collider other)
    {
        // ⭐️ FIX: Check for EITHER new tag before proceeding
        if (other.CompareTag("RedShape") || other.CompareTag("BlueShape"))
        {
            MovingDraggableShape shapeMover = other.GetComponent<MovingDraggableShape>();

            if (shapeMover == null) return;

            ShapeSpawner spawner = shapeMover.spawner;
            if (spawner == null) return;

            // This checks if the shape's specific tag matches the zone's expected tag
            if (other.CompareTag(acceptedShapeTag))
            {
                // CORRECT SHAPE: Score + Destroy
                spawner.AddScore(10);
                Destroy(other.gameObject);
            }
            else
            {
                // WRONG SHAPE: Push Away
                Vector3 pushDirection = other.transform.position - transform.position;
                pushDirection.z = 0;

                shapeMover.PushAway(pushDirection.normalized);
            }
        }
    }
}