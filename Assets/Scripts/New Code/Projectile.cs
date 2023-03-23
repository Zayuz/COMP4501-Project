using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Start is called before the first frame update

    private Vector3 pos_;
    private Vector3 dir_;
    private float damage_;
    private float range_;
    private float scale_;
    private float speed_;
    public static Projectile Create(Vector3 pos, float damage, float range, float scale, float speed, Vector3 dir)
    {
        Transform projTransform = Instantiate(GameAssets.i.pfProjectile, pos, Quaternion.identity);
        Projectile proj = projTransform.GetComponent<Projectile>();
        

        return proj;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position + (dir_ * speed_) * Time.deltaTime;
    }
}
