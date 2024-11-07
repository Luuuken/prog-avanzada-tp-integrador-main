using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Personaje : MonoBehaviour
{
    public int vida;

    public void RecibirDano(int dano)
    {
        vida -= dano;

        if(vida <= 0)
        {
            Destroy(gameObject);
        }
    }
}
