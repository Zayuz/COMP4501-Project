using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = System.Random;

//This class is very broadly inspired by https://github.com/boardtobits/flocking-algorithm with many significant differences in structure and function
public class FlockMovement : MonoBehaviour
{
    List<Transform> flock;
    delegate Vector2 flockingComponents();
    public float minSeperation = 30.0f;
    public string flockTag;
    float[] weights;
    public bool seek;
    public bool leader;

    // Start is called before the first frame update
    void Start()
    {
        flock = new List<Transform>();

        //Assign all flockmates to complete list flock
        foreach (Transform tagged in GameObject.FindWithTag(flockTag).transform)
        {
            if (tagged.position != GetComponent<Transform>().position)
            {
                flock.Add(tagged);
            }
        }

        //Assign weight for each component
        weights = new float[3];
        weights[0] = 1.0f; //Seperate
        weights[1] = 1.0f; //Cohesion
        weights[2] = 1.0f; //Alignment
    }

    // Update is called once per frame
    void Update()
    {
        //Steering behavior changes the movement pattern
        //Add component of flocking movement functions to a delegate list for iteration
        List<flockingComponents> flockingComponents = new List<flockingComponents>();
        flockingComponents.Add(Seperate);
        flockingComponents.Add(Cohesion);
        flockingComponents.Add(Alignment);

        //set up move
        Vector2 move = Vector2.zero;

        //iterate through flocking behaviors
        for (int i = 0; i < flockingComponents.Count; i++)
        {
            Vector2 partialMove = flockingComponents[i]() * weights[i];

            if (partialMove != Vector2.zero)
            {
                if (partialMove.sqrMagnitude > weights[i] * weights[i])
                {
                    partialMove.Normalize();
                    partialMove *= weights[i];
                }

                move += partialMove;

            }
        }
        //Add random wandering to prior flock movement
        Random rnd = new Random();
        Vector3 unitDest = GetComponent<Unit>().destination;
        float destDist = Mathf.Sqrt(Mathf.Pow(unitDest.x - transform.position.x, 2) + Mathf.Pow(unitDest.z - transform.position.z, 2));
        Vector3 correctedMove;
        float wDelta = rnd.Next(1, 361);

        //Only change the movement pattern if the unit has not already decided to go somewhere and wander is true
        if (destDist < 2 && seek != true && leader)
        {
            //When the leader arrives at the seek destination is plots a new course for the entire herd
            correctedMove.x = (float)(3 * Math.Cos(wDelta) + move.x);
            correctedMove.y = 0;
            correctedMove.z = (float)(3 * Math.Sin(wDelta) + move.y);
            GetComponent<Transform>().position += correctedMove * Time.deltaTime;
            GetComponent<Unit>().destination.x = transform.position.x + (correctedMove.x * 15);
            GetComponent<Unit>().destination.z = transform.position.z + (correctedMove.z * 15);
            seek = true;

            foreach (Transform tagged in GameObject.FindWithTag(flockTag).transform)
            {
                Unit taggedUnit = tagged.GetComponent<Unit>();
                taggedUnit.destination.x = transform.position.x + (correctedMove.x * 15);
                taggedUnit.destination.z = transform.position.z + (correctedMove.z * 15);
            }
        }
        else if(destDist < 2 && seek != true)
        {
            //Non leaders will wander out of the way so that the leader can get to the destination and set a new course
            correctedMove.x = (float)(3 * Math.Cos(wDelta) + move.x);
            correctedMove.y = 0;
            correctedMove.z = (float)(3 * Math.Sin(wDelta) + move.y);
            GetComponent<Transform>().position += correctedMove * Time.deltaTime;
            GetComponent<Unit>().destination.x = transform.position.x + (correctedMove.x * 15);
            GetComponent<Unit>().destination.z = transform.position.z + (correctedMove.z * 15);
            seek = true;
        }
        else
        {
            //Not close enough to 'wander' in a new direction
            correctedMove.x = move.x;
            correctedMove.y = 0;
            correctedMove.z = move.y;
            GetComponent<Transform>().position += correctedMove * Time.deltaTime;
        }
    }

    Vector2 Seperate()
    {
        //Seperation
        //if no neighbors, return no adjustment
        if (flock.Count <= 1)
            return Vector2.zero;

        //add all points together and average
        Vector2 avoidanceMove = Vector2.zero;
        int nAvoid = 0;

        for (int i = 0; i < flock.Count; i++)
        {
            if (Vector2.SqrMagnitude(flock[i].GetComponent<Transform>().position - GetComponent<Transform>().position) < minSeperation)
            {
                nAvoid++;
                avoidanceMove.x += GetComponent<Transform>().position.x - flock[i].GetComponent<Transform>().position.x;
                avoidanceMove.y += GetComponent<Transform>().position.x - flock[i].GetComponent<Transform>().position.z;
            }
        }

        if (nAvoid > 0)
        {
            avoidanceMove /= nAvoid;
        }

        return avoidanceMove;
    }

    Vector2 Cohesion()
    {
        //Cohesion
        //if no neighbors, return no adjustment
        if (flock.Count <= 1)
            return Vector2.zero;

        //add all points together and average
        Vector2 cohesionMove = Vector2.zero;

        for (int i = 0; i < flock.Count; i++)
        {
            cohesionMove.x += flock[i].GetComponent<Transform>().position.x;
            cohesionMove.y += flock[i].GetComponent<Transform>().position.z;
        }
        cohesionMove /= flock.Count;

        //create offset from agent position
        cohesionMove -= (Vector2)GetComponent<Transform>().position;
        return cohesionMove;
    }

    Vector2 Alignment()
    {
        //Alignment
        //if no neighbors, maintain current alignment
        if (flock.Count <= 1)
            return GetComponent<Transform>().up;

        //add all points together and average
        Vector2 alignmentMove = Vector2.zero;

        for (int i = 0; i < flock.Count; i++)
        {
            alignmentMove.x += flock[i].GetComponent<Transform>().position.x;
            alignmentMove.y += flock[i].GetComponent<Transform>().position.z;
        }
        alignmentMove /= flock.Count;

        return alignmentMove;
    }
}
