using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
  [Header("Movement")]
  public float moveSpeed = 6f;
  public float gravity = -9.81f;
  public float mouseSens = 100f;

  [Header("References")]
  public Camera cam;
  public Camera currentCamera;
  public Transform camPitchRoot;
  public Transform camPivot;
  public Transform muzzlePoint;
  public GameObject crosshair;
  public Animator anim;

  [Header("Shooting")]
  public GameObject bulletPrefab;
  public GameObject muzzleFlashPrefab;
  public float bulletSpeed = 60f;
  public float fireRate = 0.25f;

  private CharacterController cc;
  private Vector3 velocity;
  private float pitch;
  private float nextFire;
  private bool isAiming;
  private bool isDead = false;
  private PlayerHealth health;

  void Start()
  {
    DontDestroyOnLoad(gameObject);

    cc = GetComponent<CharacterController>();
    Cursor.lockState = CursorLockMode.Locked;
    Cursor.visible = false;

    pitch = camPitchRoot.localEulerAngles.x;
    if (pitch > 180f) pitch -= 360f;

    health = GetComponent<PlayerHealth>();
    currentCamera = cam;

    GameObject cp = GameObject.FindGameObjectWithTag("Checkpoint");
    if (cp != null)
      transform.position = cp.transform.position;

    bool isRespawn = PlayerPrefs.GetInt("IsRespawn", 0) == 1;

    // ✅ ניהול התחלה רק עבור KillCount; את ה-HP מנהל PlayerHealth כדי למנוע התנגשויות סדר אתחול
    if (!isRespawn)
    {
      int kills = PlayerPrefs.GetInt("LastLevelKillCount", 0);
      PlayerPrefs.SetInt("LevelStartKillCount", kills);
    }

    // לאחר שהסצנה עלתה, כבר לא במצב Respawn
    PlayerPrefs.SetInt("IsRespawn", 0);
  }

  void Update()
  {
    if (isDead) return;

    LookYaw();
    Move();
    Aim();
    Shoot();
  }

  void LateUpdate()
  {
    if (isDead) return;
    LookPitch();
  }

  void LookYaw()
  {
    float mx = Input.GetAxis("Mouse X") * mouseSens * Time.deltaTime;
    transform.Rotate(Vector3.up * mx);
  }

  void LookPitch()
  {
    float my = Input.GetAxis("Mouse Y") * mouseSens * Time.deltaTime;
    pitch -= my;
    pitch = Mathf.Clamp(pitch, -80f, 80f);
    camPitchRoot.localEulerAngles = new Vector3(pitch, 0f, 0f);
  }

  void Move()
  {
    float h = Input.GetAxis("Horizontal");
    float v = Input.GetAxis("Vertical");

    Vector3 forward = cam.transform.forward;
    Vector3 right = cam.transform.right;
    forward.y = right.y = 0f;
    forward.Normalize();
    right.Normalize();

    Vector3 moveDir = (right * h + forward * v).normalized;
    cc.Move(moveDir * moveSpeed * Time.deltaTime);

    if (cc.isGrounded && velocity.y < 0)
      velocity.y = -2f;

    velocity.y += gravity * Time.deltaTime;
    cc.Move(velocity * Time.deltaTime);

    if (anim)
      anim.SetFloat("Speed", new Vector2(h, v).magnitude);
  }

  void Aim()
  {
    isAiming = Input.GetMouseButton(1);
    if (anim) anim.SetBool("isAiming", isAiming);
    if (crosshair) crosshair.SetActive(isAiming);
  }

  void Shoot()
  {
    bool fireInput = Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.Space);
    if (!isAiming || !fireInput || Time.time < nextFire || currentCamera == null) return;

    nextFire = Time.time + fireRate;
    if (anim) anim.SetTrigger("Shoot");

    Ray ray = currentCamera.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
    Vector3 targetPoint;

    if (Physics.Raycast(ray, out RaycastHit hit, 100f))
      targetPoint = hit.point;
    else
      targetPoint = ray.GetPoint(100f);

    Vector3 dir = (targetPoint - muzzlePoint.position).normalized;

    GameObject bullet = Instantiate(bulletPrefab, muzzlePoint.position, Quaternion.LookRotation(dir));
    if (bullet.TryGetComponent(out Rigidbody rb))
      rb.linearVelocity = dir * bulletSpeed;

    if (muzzleFlashPrefab)
      Destroy(Instantiate(muzzleFlashPrefab, muzzlePoint.position, muzzlePoint.rotation, muzzlePoint), 0.05f);
  }

  void OnTriggerEnter(Collider other)
  {
    if (other.attachedRigidbody != null && other.attachedRigidbody.GetComponent<EnemyScript>() == null)
    {
      health.TakeDamage(10);
      Destroy(other.gameObject);
    }
  }

  public void MarkAsDead()
  {
    isDead = true;
    if (anim) anim.SetTrigger("Die");
    Invoke("RespawnAtCheckpoint", 1.5f);
  }

  void RespawnAtCheckpoint()
  {
    // ✅ Respawn בתוך אותו שלב: להחזיר ל-HP של תחילת השלב
    PlayerPrefs.SetInt("IsRespawn", 1);
    int levelStartHp = PlayerPrefs.GetInt("LevelStartHP", 100);
    PlayerPrefs.SetInt("PlayerHealth", levelStartHp);

    // אפס הריגות לרמת תחילת השלב
    KillCounter.Instance?.ResetToLevelStart();

    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
  }
}