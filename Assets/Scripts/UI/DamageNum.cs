using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// credit to https://www.youtube.com/watch?v=iD1_JczQcFY
public class DamageNum : MonoBehaviour
{
    public Transform cam;
    // create a damage number
    public static DamageNum Create(Vector3 pos, int damage) {
        Transform damageNumTransform = Instantiate(GameAssets.i.pfDamagePopup, pos, Quaternion.identity);
        DamageNum damageNum = damageNumTransform.GetComponent<DamageNum>();
        damageNum.Setup(damage);
        
        return damageNum;
    }
    private TextMeshPro textMesh;
    private float disappearTimer;
    private const float DISAPPEAR_TIMER_MAX = 1f;
    private Color textColor;
    private void Awake()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
        textMesh = transform.GetComponent<TextMeshPro>();
        textColor = textMesh.color;
        disappearTimer = 1f;
    }
    public void Setup(int damageAmount) { 
        textMesh.SetText(damageAmount.ToString());
        transform.LookAt(transform.position + cam.forward);
    }

    private void Update() {
        float ySpeed = 20f;
        transform.position += new Vector3(0, ySpeed) * Time.deltaTime;

        if (disappearTimer > DISAPPEAR_TIMER_MAX * .5f) // first half of life time -> bigger number
        {
            float increaseScaleAmount = 1f;
            transform.localScale += Vector3.one * increaseScaleAmount * Time.deltaTime;
        }
        else { // second half of life time -> smaller number
            float decreaseScaleAmount = 1f;
            transform.localScale -= Vector3.one * decreaseScaleAmount * Time.deltaTime;
        }

        disappearTimer -= Time.deltaTime;
        if (disappearTimer < 0) {
            // start fading
            float disappearSpeed = 3f;
            textColor.a -= disappearSpeed * Time.deltaTime;
            textMesh.color = textColor;

            if (textColor.a < 0) {
                Destroy(gameObject);
            }
        }
    }
}
