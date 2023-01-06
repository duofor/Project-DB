using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class MonsterAIFollow : MonoBehaviour {
    Util util = new Util();


    private Transform target;

    public float speed = 50f;
    private float nextWaypointDistance = 0.05f;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;
    bool shouldPath = true;

    Seeker seeker;
    Rigidbody2D rb;

    //cast
    float collisionOffset = 0.05f;
    public ContactFilter2D movementFilter;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();

    float localScale_X;
    float localScale_Y;

    float flipTime = 0.5f;
    float countTime = 0f;

    void Start() {
        localScale_X = transform.localScale.x;
        localScale_Y = transform.localScale.y;

        seeker = GetComponent<Seeker>();
        target = GameObject.Find("Player").transform; 

        rb = GetComponent<Rigidbody2D>();
        rb.drag = 1.5f; //linear drag so it stops
        InvokeRepeating("UpdatePath", 0f, 2f);
    }

    void UpdatePath() {
        if ( shouldPath ) {
            if ( seeker.IsDone() ) { //if its not calculating a path now
                seeker.StartPath( rb.position, target.position, OnPathComplete );
            }
        }


    }

    void OnPathComplete(Path p) {
        if (!p.error) {
            path = p;
            currentWaypoint = 0;
        }
    }

    void FixedUpdate() {
        if ( path == null )
            return;
        
        if ( currentWaypoint >= path.vectorPath.Count ) {
            reachedEndOfPath = true;
            return;
        } else {
            reachedEndOfPath = false;
        }

        //move
        Vector2 direction = ((Vector2) path.vectorPath[currentWaypoint] - rb.position).normalized; // for exact movemement change the normalise
        Vector2 force = direction * speed * Time.fixedDeltaTime;
        
        rb.AddForce( force );

        float distance = Vector2.Distance( rb.position, path.vectorPath[currentWaypoint] );
        if ( distance < nextWaypointDistance ) { //reached
            currentWaypoint++;
        }

        countTime += Time.deltaTime;

        if ( countTime > flipTime ) { //atempt to make sprite not flip every milisec
            countTime = 0f;

            if (rb.velocity.x >= 0.01f && force.x > 0f) {
                transform.localScale = new Vector3( - localScale_X, localScale_Y, 1f);
            }
            else if (rb.velocity.x <= -0.01 && force.x < 0f) {
                transform.localScale = new Vector3(localScale_X, localScale_Y, 1f);
            }
        }


    }

    private bool tryMove(Vector2 direction) {
        //check colision
        int count = rb.Cast(
            direction,
            movementFilter,
            castCollisions,
            1f * Time.fixedDeltaTime + collisionOffset
        );
        if ( count == 0 ) {
            return true;
        }
        return false;
    }

    public void enablePathFinding() {
        shouldPath = true;
    }
    public void disablePathFinding() {
        shouldPath = false;
    }
}
