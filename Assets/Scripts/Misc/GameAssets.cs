using Mono.Cecil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// credit to: https://www.youtube.com/watch?v=iD1_JczQcFY
public class GameAssets : MonoBehaviour
{
    // Class that "holds references to assets that developers can use anywhere in their code"
    // "One object that holds all your references, and lets you access those references anywhere else in your code"
    // currently only holds damage popup prefab and used in DamageNum.cs
    // Game Assets prefab has to be in Resources folder for Unity reasons, otherwise will crash.
    private static GameAssets _i;

    public static GameAssets i
    {
        get
        {
            if (_i == null) _i = Instantiate(Resources.Load<GameAssets>("GameAssets"));
            return _i;
        }
    }

    public Transform pfDamagePopup;
    public Transform pfProjectile;
}
