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
	void Update () {
        Vector3 targetPosition = player.transform.position;
        float offsetX = Random.Range(-2.0f, -2.0f);
        float offsetY = Random.Range(-2.0f, -2.0f);
        targetPosition = player.transform.position;
        targetPosition.x += offsetX;
        targetPosition.y += offsetY;

        target.transform.position = targetPosition;
    }
}
