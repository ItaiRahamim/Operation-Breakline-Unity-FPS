using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
  public Camera fpsCamera;
  public Camera tpsCamera;
  public KeyCode switchKey = KeyCode.V;
  public PlayerController playerController;

  private bool isThirdPerson = false;

  void Start()
  {
    SetCameraMode(isThirdPerson);
  }

  void Update()
  {
    if (Input.GetKeyDown(switchKey))
    {
      isThirdPerson = !isThirdPerson;
      SetCameraMode(isThirdPerson);
    }
  }

  void SetCameraMode(bool thirdPerson)
  {
    fpsCamera.enabled = !thirdPerson;
    tpsCamera.enabled = thirdPerson;

    if (playerController != null)
    {
      playerController.currentCamera = thirdPerson ? tpsCamera : fpsCamera;
    }
  }
}