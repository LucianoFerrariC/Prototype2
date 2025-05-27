using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 3.0f;
    CharacterController _controller;
    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }
    private void Update()
    {
        IsometricMovement();
    }
    void IsometricMovement()
    {
        //Aqui se toman en una variable Vector3 los Input "Horizontal" & "Vertical" en una sola variable.
        Vector3 fullInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        //Si se presiona algun Input de movimento...
        if (fullInput != Vector3.zero)
        {
            //Se crea una variable que mueve nuestro eje Y en 45 grados.
            var matrix = Matrix4x4.Rotate(Quaternion.Euler(0,45,0));

            //Luego se aplica y remplaza por el "fullInput".
            var skewedInput = matrix.MultiplyPoint3x4(fullInput);

            //Y asi hace la conversion en nuestra posision acuual...
            var relative = (transform.position + skewedInput) - transform.position;
            var rotation = Quaternion.LookRotation(relative, Vector3.up);

            //Y la aplica en nuestro player.
            transform.rotation = rotation;
        }
        //Pero si no se presiona nada...
        else
        {
            //Dejara el Input en 0, para que el player no se deslize infinitamente.
            fullInput = Vector3.zero;
        }

        Vector3 moveDirection = transform.forward * fullInput.magnitude * speed;

        _controller.SimpleMove(moveDirection);

    }
}
