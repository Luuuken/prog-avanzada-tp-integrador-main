using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemigoController : Personaje
{
    Transform puntoDePivote;
    public float velocidad = 1;
    GameObject jugador;



    void Start()
    {
        puntoDePivote = transform.GetChild(0);
        jugador = GameObject.Find("NaveJugador");
    }


    void Update()
    {
        if(Vector3.Distance(puntoDePivote.position, jugador.transform.position) <= 0.001)
        {
            transform.RotateAround(puntoDePivote.position, Vector3.forward, velocidad * Time.deltaTime);
        } else
        {

        }
        transform.LookAt(jugador.transform);
    }
}
