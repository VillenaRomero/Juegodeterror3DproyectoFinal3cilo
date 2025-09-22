using UnityEngine;

public class weapon : MonoBehaviour
{
    [Header("Datos del arma")]
    public string weaponName = "Escopeta";
    public int ammo = 0;         
    public int maxAmmo = 8;      
    public int reserveAmmo = 20; 
    public int maxReserve = 50;  

    [Header("Disparo")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletForce = 20f;

    [Header("Recarga")]
    public float reloadTime = 2f;
    private bool isReloading = false;

    public void Fire()
    {
        if (isReloading) return;

        if (ammo > 0)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            rb.linearVelocity = firePoint.forward * bulletForce;

            ammo--;
            Debug.Log(weaponName + " disparó. Balas: " + ammo/reserveAmmo);
        }
        else
        {
            Debug.Log(weaponName + " sin balas en el cargador! Presiona R para recargar.");
        }
    }

    public void Reload()
    {
        if (isReloading) return;
        if (ammo == maxAmmo) return;
        if (reserveAmmo <= 0)
        {
            Debug.Log("No hay balas en reserva!");
            return;
        }

        isReloading = true;
        Debug.Log("Recargando " + weaponName + "...");
        Invoke(nameof(FinishReload), reloadTime);
    }

    void FinishReload()
    {
        int needed = maxAmmo - ammo;
        int toLoad = Mathf.Min(needed, reserveAmmo);
        ammo += toLoad;
        reserveAmmo -= toLoad;

        isReloading = false;
        Debug.Log("Recarga completa. Balas: " + ammo/reserveAmmo);
    }

    public void AddAmmo(int amount)
    {
        reserveAmmo = Mathf.Min(reserveAmmo + amount, maxReserve);
    }
}
