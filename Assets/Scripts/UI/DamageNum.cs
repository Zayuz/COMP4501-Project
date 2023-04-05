using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// credit to https://www.youtube.com/watch?v=iD1_JczQcFY
public class DamageNum : MonoBehaviour
{
    public enum colors
    {
        orange, // dmg
        green, // healing
        pink,
        red // crits? not implemented yet
    } // popup text colors

    public Transform cam;

    private TextMeshPro textMesh;
    private float disappearTimer;
    private const float DISAPPEAR_TIMER_MAX = 1f;
    private Color textColor;

    // create a damage number
    public static DamageNum Create(Vector3 pos, string text, colors col) 
    {
        Transform damageNumTransform = Instantiate(GameAssets.i.pfDamagePopup, pos, Quaternion.identity);
        DamageNum damageNum = damageNumTransform.GetComponent<DamageNum>();
        damageNum.Setup(text, col);
        
        return damageNum;
    }

    private void Awake()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
        textMesh = transform.GetComponent<TextMeshPro>();
    }

    public void Setup(string text, colors col) 
    { 
        textMesh.SetText(text);
        transform.LookAt(transform.position + cam.forward);
        Color newCol;

        if (col == colors.orange)
        {
            if (ColorUtility.TryParseHtmlString("orange", out newCol))
            {   
                textColor = newCol;
            }
        }
        else if (col == colors.green)
        {
            if (ColorUtility.TryParseHtmlString("lime", out newCol))
            {
                textColor = newCol;
            }
        }
        else if (col == colors.pink)
        {
            if (ColorUtility.TryParseHtmlString("fuchsia", out newCol))//#fc03d3
            {
                textColor = newCol;
            }
        }
        else 
        {
            if (ColorUtility.TryParseHtmlString("red", out newCol))
            {
                textColor = newCol;
            }
        }

        textMesh.color = textColor;
        textMesh.faceColor = textColor;
        disappearTimer = 1f;
    }

    private void Update() 
    {
        float ySpeed = 20f;
        transform.position += new Vector3(0, ySpeed) * Time.deltaTime;

        if (disappearTimer > DISAPPEAR_TIMER_MAX * .5f) // first half of life time -> bigger number
        {
            float increaseScaleAmount = 1f;
            transform.localScale += Vector3.one * increaseScaleAmount * Time.deltaTime;
        }
        else 
        { // second half of life time -> smaller number
            float decreaseScaleAmount = 1f;
            transform.localScale -= Vector3.one * decreaseScaleAmount * Time.deltaTime;
        }

        disappearTimer -= Time.deltaTime;

        if (disappearTimer < 0) 
        {
            // start fading
            float disappearSpeed = 3f;
            textColor.a -= disappearSpeed * Time.deltaTime;
            textMesh.color = textColor;

            if (textColor.a < 0) 
            {
                Destroy(gameObject);
            }
        }
    }
    
}
