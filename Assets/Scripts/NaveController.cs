using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaveController : Personaje
{
    public GameObject proyectilPrefab;
    public float velocidadProyectil = 10f;
    public float tiempoEntreDisparos = 0.5f;
    private float tiempoUltimoDisparo = 0f;

    public int cantidadDisparos = 1;

    // Start is called before the first frame update
    void Start()
    {

    }

    void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        mousePosition.z = 0;
        Vector3 direction = mousePosition - transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        if (Input.GetMouseButtonDown(0)) 
        {
            Disparar(mousePosition);
        }
    }

    void Disparar(Vector3 mousePosition)
    {
        if (Time.time - tiempoUltimoDisparo < tiempoEntreDisparos) return;

        Vector3 direccion = (mousePosition - transform.position).normalized;

        GameObject proyectil = Instantiate(proyectilPrefab, transform.position, Quaternion.identity);
        Rigidbody rb = proyectil.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = direccion * velocidadProyectil;
        }
        proyectil.transform.LookAt(mousePosition);
        Destroy(proyectil, 3);
        tiempoUltimoDisparo = Time.time;
    }

}
