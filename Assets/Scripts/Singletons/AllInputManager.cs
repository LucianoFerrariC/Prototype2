using UnityEngine;
using UnityEngine.SceneManagement;

public class AllInputManager : MonoBehaviour
{
    private static AllInputManager _instance;
    public static AllInputManager Instance
    {
        get
        {
            return _instance;
        }
    }
    private PlayerInputActions actions;
    private Scene scene;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
        actions = new PlayerInputActions();
        CheckStage();
    }
    public Vector2 IsometricMovement()
    {
        return actions.IsometricView.Movement.ReadValue<Vector2>();
    }
    public Vector2 MouseLook()
    {
        return actions.FirstPersonView.Look.ReadValue<Vector2>();
    }
    public float IsometricSwitch()
    {
        return actions.IsometricView.SwitchView.ReadValue<float>();
    }
    public float FirstPersonSwitch()
    {
        return actions.FirstPersonView.SwitchView.ReadValue<float>();
    }
    public void IsometricToFirstPersonView()
    {
        Cursor.lockState = CursorLockMode.Locked;
        actions.IsometricView.Disable();
        actions.FirstPersonView.Enable();
    }
    public void FirstPersonToIsometricView()
    {
        Cursor.lockState = CursorLockMode.None;
        actions.FirstPersonView.Disable();
        actions.IsometricView.Enable();
    }
    private void CheckStage()
    {
        int currentScene;
        scene = SceneManager.GetActiveScene();
        currentScene = scene.buildIndex;
        if(currentScene == 0)
        {
            actions.IsometricView.Enable();
        }
    }
}
