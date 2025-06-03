using UnityEngine;
using Cinemachine;

public class CinemachinePOVExtension : CinemachineExtension
{
    [SerializeField] private float clampAngle = 80f;
    [SerializeField] private float horizonalSpeed = 10f;
    [SerializeField] private float verticalSpeed = 10f;
    private AllInputManager inputManager;
    private Vector3 startRotation;
    protected override void Awake()
    {
        inputManager = AllInputManager.Instance;
        base.Awake();
    }
    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (vcam.Follow)
        {
            if (stage == CinemachineCore.Stage.Aim)
            {
                if (startRotation == null)
                {
                    startRotation = transform.localRotation.eulerAngles;
                    Vector2 deltaInput = inputManager.MouseLook();
                    startRotation.x += deltaInput.x * verticalSpeed * Time.deltaTime;
                    startRotation.y += deltaInput.y * horizonalSpeed * Time.deltaTime;
                    startRotation.y = Mathf.Clamp(startRotation.y, -clampAngle, clampAngle);
                    state.RawOrientation = Quaternion.Euler(startRotation.y, startRotation.x, 0f);
                }
            }
        }
    }
}
