using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockMovement : MonoBehaviour
{
    List<Transform> flock;
    delegate Vector2 flockingComponents();
    public float minSeperation = 3.0f;
    public string flockTag;
    float[] weights;

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
        //Add component of flocking movement functions to a delegate list for iteration
        List<flockingComponents> flockingComponents = new List<flockingComponents>();
        flockingComponents.Add(Seperate);
        flockingComponents.Add(Cohesion);
        flockingComponents.Add(Alignment);

        //set up move
        Vector2 move = Vector2.zero;
        
        //iterate through behaviors
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

        //Apply move to the gameobject
        GetComponent<Transform>().position += (Vector3)move * Time.deltaTime / 3;
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
                avoidanceMove += (Vector2)(GetComponent<Transform>().position - flock[i].GetComponent<Transform>().position);
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
            cohesionMove += (Vector2)flock[i].GetComponent<Transform>().position;
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
            alignmentMove += (Vector2)flock[i].GetComponent<Transform>().up;
        }
        alignmentMove /= flock.Count;

        return alignmentMove;
    }

    void Seek()
    {
        //Steering behavior
    }

    void Wander()
    {
        //Steering behavior
    }
}
