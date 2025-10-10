using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class CameraSystem : MonoBehaviour
{
    public CinemachineCamera[] securityCameras;
    private int currentCameraIndex = 0;

    [Header("Jugador")]
    public Camera playerCamera;
    private Vector3 savedPosition;
    private Quaternion savedRotation;
    private bool inSecurityMode = false;

    [Header("UI Seguridad")]
    public GameObject uiCanvas;
    public Image blackScreen;
    public Text errorText;
    public Image mapImage;
    public Transform buttonContainer;
    public Button cameraButtonPrefab;

    [Header("Cámara - Tiempo límite")]
    public float cameraActiveTime = 30f;
    private float cameraTimer = 0f;
    private bool cameraError = false;

    [Header("Reparación de cámaras")]
    public bool hasRepairTool = false;
    public float repairTime = 10f;
    private float repairTimer = 0f;
    private bool repairing = false;
    private int repairingCameraIndex = -1;

    void Start()
    {
        EnablePlayerCamera();

        if (blackScreen != null) blackScreen.enabled = false;
        if (errorText != null) errorText.enabled = false;
        if (mapImage != null) mapImage.enabled = false;

        // Crear botones para cada cámara
        if (cameraButtonPrefab != null && buttonContainer != null)
        {
            for (int i = 0; i < securityCameras.Length; i++)
            {
                int index = i;
                Button newButton = Instantiate(cameraButtonPrefab, buttonContainer);
                newButton.GetComponentInChildren<Text>().text = "Cam " + (i + 1);
                newButton.onClick.AddListener(() => SwitchToCamera(index));
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!inSecurityMode) EnterSecurityMode();
            else ExitSecurityMode();
        }

        if (inSecurityMode)
        {
            if (Input.GetKeyDown(KeyCode.E))
                SwitchToNextCamera();

            if (Input.GetKeyDown(KeyCode.Q))
                SwitchToPreviousCamera();

            if (!cameraError)
            {
                cameraTimer -= Time.deltaTime;
                if (cameraTimer <= 0f)
                {
                    CameraError();
                }
            }
        }

        if (repairing)
        {
            repairTimer -= Time.deltaTime;
            if (repairTimer <= 0f)
            {
                FinishRepair();
            }
        }
    }

    void EnterSecurityMode()
    {
        inSecurityMode = true;

        savedPosition = playerCamera.transform.position;
        savedRotation = playerCamera.transform.rotation;

        playerCamera.enabled = false;

        currentCameraIndex = 0;
        EnableOnlyCamera(securityCameras[currentCameraIndex]);

        ResetCameraTimer();

        if (uiCanvas != null) uiCanvas.SetActive(true);
        if (mapImage != null) mapImage.enabled = true;
    }

    void ExitSecurityMode()
    {
        inSecurityMode = false;
        DisableAllCinemachineCameras();

        playerCamera.enabled = true;
        playerCamera.transform.position = savedPosition;
        playerCamera.transform.rotation = savedRotation;

        if (uiCanvas != null) uiCanvas.SetActive(false);
        if (mapImage != null) mapImage.enabled = false;
    }

    void SwitchToNextCamera()
    {
        if (cameraError) return;

        securityCameras[currentCameraIndex].Priority = 0;

        currentCameraIndex++;
        if (currentCameraIndex >= securityCameras.Length)
            currentCameraIndex = 0;

        EnableOnlyCamera(securityCameras[currentCameraIndex]);
        ResetCameraTimer();
    }

    void SwitchToPreviousCamera()
    {
        if (cameraError) return;

        securityCameras[currentCameraIndex].Priority = 0;

        currentCameraIndex--;
        if (currentCameraIndex < 0)
            currentCameraIndex = securityCameras.Length - 1;

        EnableOnlyCamera(securityCameras[currentCameraIndex]);
        ResetCameraTimer();
    }

    void SwitchToCamera(int index)
    {
        if (cameraError) return;

        securityCameras[currentCameraIndex].Priority = 0;
        currentCameraIndex = index;
        EnableOnlyCamera(securityCameras[currentCameraIndex]);
        ResetCameraTimer();
    }

    void EnableOnlyCamera(CinemachineCamera cam)
    {
        DisableAllCinemachineCameras();
        cam.Priority = 10;
    }

    void DisableAllCinemachineCameras()
    {
        for (int i = 0; i < securityCameras.Length; i++)
        {
            if (securityCameras[i] != null)
                securityCameras[i].Priority = 0;
        }
    }

    void EnablePlayerCamera()
    {
        playerCamera.enabled = true;
        DisableAllCinemachineCameras();
    }

    void ResetCameraTimer()
    {
        cameraTimer = cameraActiveTime;
        cameraError = false;
        if (blackScreen != null) blackScreen.enabled = false;
        if (errorText != null) errorText.enabled = false;
    }

    void CameraError()
    {
        cameraError = true;
        if (blackScreen != null) blackScreen.enabled = true;
        if (errorText != null) errorText.enabled = true;
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("CameraRepair") && cameraError && hasRepairTool && !repairing)
        {
            repairing = true;
            repairTimer = repairTime;
            repairingCameraIndex = currentCameraIndex;
            Debug.Log("Reparando cámara " + repairingCameraIndex + "...");
        }
    }

    void FinishRepair()
    {
        repairing = false;
        cameraError = false;
        repairingCameraIndex = -1;

        if (blackScreen != null) blackScreen.enabled = false;
        if (errorText != null) errorText.enabled = false;

        ResetCameraTimer();
    }
}
