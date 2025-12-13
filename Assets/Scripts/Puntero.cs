using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class Puntero : MonoBehaviour
{
    Camera cam;
    Rigidbody2D rb;
    bool isMousePressed;
    public Vector2 posicion;


    [HideInInspector] public UnityEvent Pressed;
    [HideInInspector] public UnityEvent Released;

    [HideInInspector] public UnityEvent<Barra> PunteroTriggerEnter;
    [HideInInspector] public UnityEvent<Barra> PunteroTriggerStay;
    [HideInInspector] public UnityEvent<Barra> PunteroTriggerExit;



    private void Awake()
    {
        Pressed = new UnityEvent();
        Released = new UnityEvent();
        PunteroTriggerEnter = new UnityEvent<Barra>();
        PunteroTriggerStay = new UnityEvent<Barra>();
        PunteroTriggerExit = new UnityEvent<Barra>();
    }

    void Start()
    {
        cam = Camera.main;
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            GameManager.instance.hayInterccion = true;
            isMousePressed = true;
            Pressed.Invoke();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isMousePressed = false;
            transform.position = new Vector3(-15, 0, 0);
            GameManager.instance.hayInterccion = false;
            Released.Invoke();
        }

        if (isMousePressed)
        {
            posicion = cam.ScreenToWorldPoint(Input.mousePosition);
            GameManager.instance.punteroPosition = posicion;
            rb.MovePosition(posicion);
        }

#endif

#if UNITY_ANDROID
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            GameManager.instance.punteroPosition = cam.ScreenToWorldPoint(touch.position);

            switch (touch.phase)
            {
                // Record initial touch position.
                case TouchPhase.Began:
                    Vector3 position = cam.ScreenToWorldPoint(touch.position);
                    GameManager.instance.hayInterccion = true;
                    rb.MovePosition(position);
                    Pressed.Invoke();
                    break;

                // Determine direction by comparing the current touch position with the initial one.
                case TouchPhase.Moved:
                    Vector3 positionMoved = cam.ScreenToWorldPoint(touch.position);
                    GameManager.instance.hayInterccion = true;
                    rb.MovePosition(positionMoved);
                    break;

                // Report that a direction has been chosen when the finger is lifted.
                case TouchPhase.Ended:
                    isMousePressed = false;
                    transform.position = new Vector3(-15, 0, 0);
                    GameManager.instance.hayInterccion = false;
                    Released.Invoke();
                    break;
            }
        }
#endif
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Barra>() != null)
        {
            Barra barraScript = collision.GetComponent<Barra>();
            PunteroTriggerEnter.Invoke(barraScript);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.GetComponent<Barra>() != null)
        {
            Barra barraScript = collision.GetComponent<Barra>();
            PunteroTriggerStay.Invoke(barraScript);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Barra>() != null)
        {
            Barra barraScript = collision.GetComponent<Barra>();
            PunteroTriggerExit.Invoke(barraScript);
        }
    }
}
