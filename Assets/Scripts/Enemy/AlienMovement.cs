using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class AlienMovement : MonoBehaviour {

    public GameObject player;
    public GameObject target;

    // Use this for initialization
    void Start () {
        Random.InitState(5318008);
        Seeker seeker = GetComponent<Seeker>();
        seeker.StartPath(transform.position, target.transform.position);
    }

    // Update is called once per frame
    void Update() {
        RaycastHit2D hit = Physics2D.Raycast(target.transform.position, player.transform.position - target.transform.position);

        if (hit.transform.gameObject != player || Vector2.Distance(transform.position, player.transform.position) > 4)
        {

            Vector3 targetPosition = player.transform.position;

            for (int i = 0; i < 1000; i++)
            {
                float randomRange = 2;
                float offsetX = Random.Range(-randomRange, randomRange);
                float offsetY = Random.Range(-randomRange, randomRange);
                targetPosition = player.transform.position;
                targetPosition.x += offsetX;
                targetPosition.y += offsetY;

                //check to see if the player is within the line of sight of this new target point
                hit = Physics2D.Raycast(targetPosition, player.transform.position - target.transform.position);

                if (hit.transform.gameObject == player && Vector2.Distance(targetPosition, player.transform.position) > 2)
                {
                    break;
                }
            }

            //update target position for path finding
            target.transform.position = targetPosition;
        }
    }
}
