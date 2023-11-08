
using System;
using UnityEngine;
namespace TPShooter.Player
{
    public class PlayerView : MonoBehaviour
    {
        private PlayerController playerController;

        public CharacterController Controller;
        public Transform Camera;
        public Animator PlayerAnimator;
        public Transform GroundCheck;
        public LayerMask GroundMask;
        public LayerMask InteractorSource;
        public float InteractorRange;

        [Header("Movement")]
        [SerializeField]
        private float horizontal;
        [SerializeField]
        private float vertical;
        [SerializeField]
        private Vector3 direction;
        [SerializeField]
        private Vector3 Movedirection;
        [Header("Jump")]
        [SerializeField]
        private float groundDistance = 0.4f;
        [SerializeField]
        private bool isGrounded;
        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        void Update()
        {
            inputHandle();
            setAnimationParameters();
            checks();
            PhysicsExternal();
        }

        private void PhysicsExternal()
        {
            Vector3 velocity = playerController.PhysicsExternal(isGrounded);
            Controller.Move(velocity);
        }

        private void checks()
        {
            isGrounded = Physics.CheckSphere(GroundCheck.position, groundDistance, GroundMask);
            if (direction.magnitude > 0)
            {
                Vector3 Movement = playerController.tPMovement(direction, Movedirection);
                Controller.Move(Movement);
            }
        }

        private void setAnimationParameters()
        {
            float MovementSpeed = playerController.CurrentSpeed() * direction.magnitude;
            PlayerAnimator.SetFloat("speed", MovementSpeed);
            PlayerAnimator.SetBool("isGrounded", isGrounded);
        }

        private void inputHandle()
        {
            horizontal = Input.GetAxisRaw("Horizontal");
            vertical = Input.GetAxisRaw("Vertical");
            direction = new Vector3(horizontal, 0f, vertical).normalized;
            if (isGrounded)
            {
                if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
                {
                    PlayerAnimator.SetTrigger("Jump");
                    playerController.Jump();
                }
                if (Input.GetKeyDown(KeyCode.LeftShift) && isGrounded)
                {
                    playerController.Changespeed();
                }
                if (Input.GetKeyDown(KeyCode.E))
                {
                    interact();
                }
            }
            

        }

        private void interact()
        {
            Collider[] colliderArray = Physics.OverlapSphere(transform.position, InteractorRange, InteractorSource);
            foreach (Collider collider in colliderArray)
            {
                collider.gameObject.GetComponent<IInteractable>().OnInteract();
            }
        }

        public void SetPlayerController(PlayerController _playerController)
        {
            playerController = _playerController;
            Debug.Log("Controller-View Connection");
        }

        public void RotatePlayer(float angleRotation)
        {
            transform.rotation = Quaternion.Euler(0f, angleRotation, 0f);
        }

    }
    

}