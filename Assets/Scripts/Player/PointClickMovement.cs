using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CharacterController))]
public class PointClickMovement : MonoBehaviour
{
    public float rotSpeed = 15.0f;
    public float jumpSpeed = 15.0f;
    public float moveSpeed = 6.0f;
    public float gravity = -9.8f;
    public float terminalVelocity = -10.0f;
    public float minFall = -1.5f;
    public float pushForce = 3.0f;

    public float deceleration = 25.0f;
    public float targetBuffer = 1.75f;
    public float _curSpeed = 0f;
    private Vector3 _targetPos = Vector3.one;

    private float _vertSpeed;
    private CharacterController _characterController;

    private ControllerColliderHit _contact;

    private Animator _animator;

    void Start()
    {
        _characterController = this.GetComponent<CharacterController>();

        _vertSpeed = minFall;

        _animator = this.GetComponent<Animator>();
    }

    void Update()
    {
        Vector3 movement = Vector3.zero;

        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit mouseHit;

            if (Physics.Raycast(ray, out mouseHit))
            {
                GameObject hitObject = mouseHit.transform.gameObject;

                if (hitObject.layer == LayerMask.NameToLayer("Ground"))
                {
                    _targetPos = mouseHit.point;
                    _curSpeed = moveSpeed;
                }
            }
        }

        if (_targetPos != Vector3.one)
        {
            Vector3 adjustedPos = new Vector3(_targetPos.x, this.transform.position.y, _targetPos.z);
            Quaternion targetRot = Quaternion.LookRotation(adjustedPos - this.transform.position);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, targetRot, rotSpeed * Time.deltaTime);

            movement = _curSpeed * Vector3.forward;
            movement = this.transform.TransformDirection(movement);

            if (Vector3.Distance(_targetPos, this.transform.position) < targetBuffer)
            {
                _curSpeed = _curSpeed - (deceleration * Time.deltaTime);

                if (_curSpeed <= 0)
                {
                    _targetPos = Vector3.one;
                }
            }
        }

        _animator.SetFloat("Speed", movement.sqrMagnitude);

        bool hitGround = false;
        RaycastHit hit;
        bool isHit = Physics.Raycast(this.transform.position, Vector3.down, out hit);

        if (_vertSpeed < 0 && isHit)
        {
            float check = (_characterController.height + _characterController.radius) / 1.9f;
            hitGround = (hit.distance <= check);
        }

        if (hitGround)
        {
            if (Input.GetButtonDown("Jump"))
            {
                _vertSpeed = jumpSpeed;
            }
            else
            {
                _vertSpeed = -0.1f;
                _animator.SetBool("Jumping", false);
            }
        }
        else
        {
            _vertSpeed = _vertSpeed + gravity * 5 * Time.deltaTime;
            if (_vertSpeed < terminalVelocity)
            {
                _vertSpeed = terminalVelocity;
            }

            if (null != _contact)
            {
                _animator.SetBool("Jumping", true);
            }

            if (_characterController.isGrounded)
            {
                if (Vector3.Dot(movement, _contact.normal) < 0)
                {
                    movement = _contact.normal * moveSpeed;
                }
                else
                {
                    movement = movement + _contact.normal * moveSpeed;
                }
            }
        }
        movement.y = _vertSpeed;

        movement = movement * Time.deltaTime;
        _characterController.Move(movement);
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        _contact = hit;

        Rigidbody body = hit.collider.attachedRigidbody;
        if (body != null && !body.isKinematic)
        {
            body.velocity = hit.moveDirection * pushForce;
        }
    }
}
