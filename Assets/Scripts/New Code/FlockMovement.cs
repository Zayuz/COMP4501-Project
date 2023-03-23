using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class is very broadly inspired by https://github.com/boardtobits/flocking-algorithm with many significant differences in structure and function
public class FlockMovement : MonoBehaviour
{
    List<Transform> flock;
    delegate Vector2 flockingComponents();
    public float minSeperation = 30.0f;
    public string flockTag;
    float[] weights;
    public bool seek;

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
        if (seek)
        {
            Seek();
        }
        else
        {
            Wander();
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

    void Seek()
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
        Vector3 correctedMove;
        correctedMove.x = move.x;
        correctedMove.y = 0;
        correctedMove.z = move.y;
        GetComponent<Transform>().position += correctedMove * Time.deltaTime;
    }

    void Wander()
    {
        Debug.Log("WANDER");
        //Steering behavior
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

        // Wander

        // Find center of circle
        //direction = velocity.normalized();
        //center = position + direction * length;
        // Random walk
        //wdelta += random(-Rrad, Rrad) // Lazy...
        //x = Vrad * cos(wdelta);
        //y = Vrad * sin(wdelta);
        //offset = vec3(x, y);
        //target = center + offset;
        //steer = seek(target);

        //Apply move to the gameobject
        //GetComponent<Transform>().position += (Vector3)move * Time.deltaTime * 3;
        Vector3 correctedMove;
        correctedMove.x = move.x;
        correctedMove.y = 0;
        correctedMove.z = move.y;
        GetComponent<Transform>().position += correctedMove * Time.deltaTime * 3;
    }
}
