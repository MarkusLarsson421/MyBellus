using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
public class FinalDash : MonoBehaviour
{
    [SerializeField] private bool canDash = true;
    [SerializeField] private bool stopGravityWhileDashing = true;
    private bool isDashing;
    private bool isFacingRight = true;
    [SerializeField] private float dashingDistace = 24f;
    [SerializeField] private float dashingDuration = 0.2f;
    [SerializeField] private float dashingActivationCooldown = 0.01f;
    //[SerializeField] private TrailRenderer tr; // these variable makes visual effect

    private Vector3 velocity;
    private PlayerMovement movement;
    private float currentDashingDistace;

    private void Start()
    {
        currentDashingDistace = dashingDistace;
        movement = GetComponent<PlayerMovement>();
    }
    
    void Update()
    {
        Flip();
        //DashWithKeyboard();
    }
    
    private void FixedUpdate()
    {
        if (isDashing)
        {
            transform.position += velocity * Time.deltaTime;
            return;
        }
        velocity = new Vector3(0, velocity.y, velocity.z);
    }
    float temp;
    private void DashWithKeyboard()
    {
        if (isDashing)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash) // INPUT
        {
            StartCoroutine(DashAction());
        }
    }
    public void DashWithJoystick(InputAction.CallbackContext context)
    {
        if (canDash)
        {
            StartCoroutine(DashAction());
        }
    }

    private IEnumerator DashAction()
    {
        CheckForCollision();
        canDash = false;
        isDashing = true;
        temp = movement.GetDownwardForce();
        if (stopGravityWhileDashing) movement.SetDownwardForce(0);
        if (movement.velocity.x > 0.1)
        {
            velocity = new Vector3(currentDashingDistace - movement.velocity.x, 0f, 0f);
        }
        else if (movement.velocity.x < 0.1)
        {
            velocity = new Vector3(-currentDashingDistace - movement.velocity.x, 0f, 0f);
        }

        //tr.emitting = true; //See variable TrailRenderer tr
        yield return new WaitForSeconds(dashingDuration);
        //tr.emitting = false; //See variable TrailRenderer tr
        currentDashingDistace = dashingDistace;
        movement.SetDownwardForce(temp);
        isDashing = false;
        yield return new WaitForSeconds(dashingActivationCooldown);
        canDash = true;

    }
    private void Flip()
    {
        if (isFacingRight && movement.velocity.x < 0f || !isFacingRight && movement.velocity.x > 0f)
        {
            Vector3 localScale = transform.localScale;
            isFacingRight = !isFacingRight;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private void CheckForCollision()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, movement.velocity, out hit, 5f, movement.WallLayer))
        {
            currentDashingDistace = hit.distance - 0.5f; /// 0.5f �r spelarens storlek

        }
    }


}
