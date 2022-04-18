using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneMovementScript : MonoBehaviour
{
    Rigidbody dron;

    void Awake()
    {
        dron = GetComponent<Rigidbody>();
        dronSonido = gameObject.transform.Find("drone_sound").GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        MovAbajoArriba();
        MovAdelante();
        Rotacion();
        VariacionVelocidad();
        Desvio();
        DronSonido();

        dron.AddRelativeForce(Vector3.up * fuerza);
        dron.rotation = Quaternion.Euler(
            new Vector3(inclinacionAdelante, rotacionYActual, dron.rotation.z)
            ); 
    }

    public float fuerza;

    void MovAbajoArriba()
    {
        if(Mathf.Abs(Input.GetAxis("Vertical")) > 0.2f || Mathf.Abs(Input.GetAxis("Horizontal")) > 0.2f )
        {
            if(Input.GetKey(KeyCode.I) || Input.GetKey(KeyCode.K)){
                dron.velocity = dron.velocity;
            }
            if(!Input.GetKey(KeyCode.I) && !Input.GetKey(KeyCode.K) && !Input.GetKey(KeyCode.J) && !Input.GetKey(KeyCode.L)){
                dron.velocity = new Vector3(dron.velocity.x, Mathf.Lerp(dron.velocity.y, 0, Time.deltaTime * 5), dron.velocity.z);
                fuerza = 281;
            }
            if(!Input.GetKey(KeyCode.I) && !Input.GetKey(KeyCode.K) && (Input.GetKey(KeyCode.J)) || Input.GetKey(KeyCode.L)) {
                dron.velocity = new Vector3(dron.velocity.x, Mathf.Lerp(dron.velocity.y, 0, Time.deltaTime * 5), dron.velocity.z);
                fuerza = 110;
            }
            if(Input.GetKey(KeyCode.J) || Input.GetKey(KeyCode.L)){
                fuerza = 410;
            }

        }

        if(Mathf.Abs(Input.GetAxis("Vertical"))<0.2f && Mathf.Abs(Input.GetAxis("Horizontal")) > 0.2f)
        {
            fuerza = 135;
        }

        if (Input.GetKey(KeyCode.I))
        {
            fuerza = 450;
            if(Mathf.Abs(Input.GetAxis("Horizontal")) > 0.2f)
            {
                fuerza = 500;
            }
        }
        else if (Input.GetKey(KeyCode.K))
        {
            fuerza = -200;
        }
        else if (!Input.GetKey(KeyCode.I) && !Input.GetKey(KeyCode.K) && (Mathf.Abs(Input.GetAxis("Vertical")) < 0.2f && Mathf.Abs(Input.GetAxis("Horizontal")) < 0.2f)) 
        {
            fuerza = 98.1f;
        }
    }

    private float movAdelante_vel = 500.0f;
    private float inclinacionAdelante = 0;
    private float inclinacionAdelante_vel;

    void MovAdelante()
    {
        if(Input.GetAxis("Vertical") != 0)
        {
            dron.AddRelativeForce(Vector3.forward * Input.GetAxis("Vertical") * movAdelante_vel);
            inclinacionAdelante = Mathf.SmoothDamp(inclinacionAdelante, 20 * Input.GetAxis("Vertical"), ref inclinacionAdelante_vel, 0.1f);
        }
    }

    private float rotacionYReq;
    [HideInInspector] public float rotacionYActual;
    private float rotacionPorTecla = 2.5f;
    private float rotacionY_vel;

    void Rotacion()
    {
        if (Input.GetKey(KeyCode.J))
        {
            rotacionYReq -= rotacionPorTecla;
        }
        if (Input.GetKey(KeyCode.L))
        {
            rotacionYReq += rotacionPorTecla;
        }

        rotacionYActual = Mathf.SmoothDamp(rotacionYActual, rotacionYReq, ref rotacionY_vel, 0.25f);
    }

    private Vector3 velocidadFreno;
    void VariacionVelocidad()
    {
        if (Mathf.Abs(Input.GetAxis("Vertical")) > 0.2f && Mathf.Abs(Input.GetAxis("Horizontal")) > 0.2f) {
            dron.velocity = Vector3.ClampMagnitude(dron.velocity, Mathf.Lerp(dron.velocity.magnitude, 10.0f, Time.deltaTime * 5f));
        }

        if (Mathf.Abs(Input.GetAxis("Vertical")) > 0.2f && Mathf.Abs(Input.GetAxis("Horizontal")) < 0.2f)
        {
            dron.velocity = Vector3.ClampMagnitude(dron.velocity, Mathf.Lerp(dron.velocity.magnitude, 10.0f, Time.deltaTime * 5f));
        }

        if(Mathf.Abs(Input.GetAxis("Vertical")) < 0.2f && Mathf.Abs(Input.GetAxis("Horizontal")) > 0.2f)
        {
            dron.velocity = Vector3.ClampMagnitude(dron.velocity, Mathf.Lerp(dron.velocity.magnitude, 5.0f, Time.deltaTime * 5f));
        }

        if(Mathf.Abs(Input.GetAxis("Vertical")) < 0.2f && Mathf.Abs(Input.GetAxis("Horizontal")) < 0.2f)
        {
            dron.velocity = Vector3.SmoothDamp(dron.velocity, Vector3.zero, ref velocidadFreno, 0.95f);
        }

    }

    private float cantMovimientoLateral = 300.0f;
    private float cantInclinacionLateral = 0;
    private float velocidadInclinacionLateral;
    private void Desvio()
    {
        if(Mathf.Abs(Input.GetAxis("Horizontal"))> 0.2f)
        {
            dron.AddRelativeForce(Vector3.right * Input.GetAxis("Horizontal") * cantMovimientoLateral);
            cantInclinacionLateral = Mathf.SmoothDamp(cantInclinacionLateral, -20 * Input.GetAxis("Horizontal"), ref velocidadInclinacionLateral, 0.1f);
        }
        else
        {
            cantInclinacionLateral = Mathf.SmoothDamp(cantInclinacionLateral, 0, ref velocidadInclinacionLateral, 0.1f);
        }
    }

    private AudioSource dronSonido;
    void DronSonido()
    {
        dronSonido.pitch = 1 + (dron.velocity.magnitude / 100);
    }

}
