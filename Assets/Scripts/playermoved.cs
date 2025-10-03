using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class playermoved : MonoBehaviour
{
    [Header("Movimiento")]
    public float speed = 5f;
    public float rotationSpeed = 300f;

    [Header("Vida")]
    [SerializeField] private int currentLife = 20;
    public int maxLife = 20;
    public Slider lifeSlider;

    [Header("Dash")]
    public float dashSpeed = 20f;
    public float dashDuration = 0.3f;
    public float dashCooldown = 1f;
    public float dashStaminaCost = 25f;

    [Header("Estamina")]
    public float stamina = 100f;
    public float maxStamina = 100f;
    public float staminaRegenRate = 15f;
    public Slider staminaSlider;

    [Header("Audio")]

    private weapon currentWeapon;
    private int weaponIndex = 0;
    public weapon[] weapons;

    private bool isDashing = false;
    private float dashTimeLeft = 0f;
    private float lastDashTime = -999f;

    private PlayerInput playerInput;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private bool isHiding = false;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    void Start()
    {
        if (staminaSlider != null)
        {
            staminaSlider.maxValue = maxStamina;
            staminaSlider.value = stamina;
        }
        if (lifeSlider != null)
        {
            lifeSlider.maxValue = maxLife;
            lifeSlider.value = currentLife;
        }
    }

    void Update()
    {
        if (isDashing)
        {
            DoDash();
        }
        else if (!isHiding)
        {
            Moved();
            PlayerRotation();
            RegenerateStamina();
        }

        UpdateStaminaUI();
        UpdateLifeUI();

        if (Input.GetMouseButtonDown(0))
        {
            TryInteractMouse();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) { 
            SelectWeapon(0); 
        }


        if (Input.GetKeyDown(KeyCode.R) && currentWeapon != null)
        {
            currentWeapon.Reload();
        }
    }

    public void OnMove(InputAction.CallbackContext ctx) {
        moveInput = ctx.ReadValue<Vector2>(); 
    }
    public void OnLook(InputAction.CallbackContext ctx) { 
        lookInput = ctx.ReadValue<Vector3>();
    }

    public void OnDash(InputAction.CallbackContext ctx)
    {
        if (ctx.started && !isDashing && stamina >= dashStaminaCost && Time.time >= lastDashTime + dashCooldown)
            StartDash();
    }

    public void OnFire(InputAction.CallbackContext ctx)
    {
        if (ctx.started && currentWeapon != null)
            currentWeapon.Fire();
    }

    void Moved()
    {
        float currentSpeed = speed * ((float)currentLife / maxLife);
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y).normalized;
        transform.position += move * currentSpeed * Time.deltaTime;
    }

    void PlayerRotation()
    {
        if (lookInput.sqrMagnitude > 0.01f)
        {
            Vector3 direction = new Vector3(lookInput.x, 0, lookInput.y);
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    void StartDash()
    {
        isDashing = true;
        dashTimeLeft = dashDuration;
        stamina -= dashStaminaCost;
        lastDashTime = Time.time;
    }

    void DoDash()
    {
        transform.position += transform.forward * dashSpeed * Time.deltaTime;
        dashTimeLeft -= Time.deltaTime;
        if (dashTimeLeft <= 0f) isDashing = false;
    }

    void RegenerateStamina()
    {
        if (stamina < maxStamina)
        {
            stamina += staminaRegenRate * Time.deltaTime;
            if (stamina > maxStamina) stamina = maxStamina;
        }
    }

    void UpdateStaminaUI()
    {
        if (staminaSlider != null) staminaSlider.value = stamina;
    }

    void UpdateLifeUI()
    {
        if (lifeSlider != null) lifeSlider.value = currentLife;
    }

    void TryInteractMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 3f))
        {
            if (hit.collider.CompareTag("Weapon"))
            {
                WeaponPickup wp = hit.collider.GetComponent<WeaponPickup>();
                if (wp != null) AddWeapon(wp.weaponPrefab);
                Destroy(hit.collider.gameObject);
                Debug.Log("Arma recogida");
            }
            else if (hit.collider.CompareTag("Ammo"))
            {
                AmmoPickup ap = hit.collider.GetComponent<AmmoPickup>();
                if (ap != null && currentWeapon != null)
                {
                    currentWeapon.AddAmmo(ap.amount);
                    Destroy(hit.collider.gameObject);
                    Debug.Log("Munición recogida");
                }
            }
            else if (hit.collider.CompareTag("HideSpot"))
            {
                isHiding = !isHiding;
                gameObject.GetComponent<MeshRenderer>().enabled = !isHiding;
                Debug.Log(isHiding ? "Jugador escondido" : "Jugador salió");
            }
        }
    }

    void AddWeapon(GameObject weaponPrefab)
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            if (weapons[i] == null)
            {
                GameObject newWeapon = Instantiate(weaponPrefab, transform);
                weapons[i] = newWeapon.GetComponent<weapon>();
                SelectWeapon(i);
                return;
            }
        }
    }

    void SelectWeapon(int index)
    {
        if (index < 0 || index >= weapons.Length) return;
        if (weapons[index] == null) return;

        for (int i = 0; i < weapons.Length; i++)
        {
            if (weapons[i] != null)
                weapons[i].gameObject.SetActive(false);
        }

        weaponIndex = index;
        currentWeapon = weapons[index];
        currentWeapon.gameObject.SetActive(true);

        Debug.Log("Arma seleccionada: " + currentWeapon.weaponName);
    }
    public void TakeDamage(int damage)
    {
        currentLife -= damage;
        if (currentLife <= 0) Die();
    }

    void Die()
    {
        Debug.Log("Jugador muerto");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Vidrio"))
        {
            TakeDamage(5);

            GameObject noiseZone = new GameObject("NoiseZone");
            SphereCollider sc = noiseZone.AddComponent<SphereCollider>();
            sc.isTrigger = true;
            sc.radius = 5f;
            noiseZone.transform.position = transform.position;
            Destroy(noiseZone, 1f);
        }
    }
}
