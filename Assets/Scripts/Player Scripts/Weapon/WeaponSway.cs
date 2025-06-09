using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    [SerializeField] private float smooth;
    [SerializeField] private float multiplier;
    private AllInputManager allInputManager;
    private void Start()

    {
        allInputManager = AllInputManager.Instance;
    }
    private void Update()
    {
        Vector2 mouse = allInputManager.MouseLook();
        float mouseX = mouse.x * multiplier;
        float mouseY = mouse.y * multiplier;

        Quaternion rotationX = Quaternion.AngleAxis(-mouseY, Vector3.right);
        Quaternion rotationY = Quaternion.AngleAxis(mouseX, Vector3.up);

        Quaternion targetRotation = rotationX * rotationY;

        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, smooth * Time.deltaTime);
    }
}
