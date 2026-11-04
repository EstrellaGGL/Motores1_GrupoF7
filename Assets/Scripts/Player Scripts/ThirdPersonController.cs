using System.Diagnostics;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
using UnityEngine.Windows;
#endif

namespace StarterAssets
{
    [RequireComponent(typeof(CharacterController))]
#if ENABLE_INPUT_SYSTEM
    [RequireComponent(typeof(PlayerInput))]
#endif
    public class ThirdPersonController : MonoBehaviour
    {
        [Header("Player")]

        [SerializeField] private PowerUpAnimation powerUpAnimation;

        [Tooltip("Move speed of the character in m/s")]
        public float MoveSpeed = 2.0f;

        [Tooltip("Sprint speed of the character in m/s")]
        public float SprintSpeed = 5.335f;

        [Tooltip("How fast the character turns to face movement direction")]
        [Range(0.0f, 0.3f)]
        public float RotationSmoothTime = 0.12f;

        [Tooltip("Acceleration and deceleration")]
        public float SpeedChangeRate = 10.0f;

        public AudioSource AudioFootsteps;
        public AudioSource LandingAudio;
        public AudioSource AudioFoley;
        public AudioClip LandingAudioClip;
        public AudioClip[] FootstepAudioClips;

        [Range(0, 1)]
        public float FootstepAudioVolume = 0.5f;

        [SerializeField] private AudioJugador audioJugador;
        

        [Space(10)]
        [Tooltip("The height the player can jump")]
        public float JumpHeight = 1.2f;

        [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
        public float Gravity = -15.0f;

        [Space(10)]
        [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
        public float JumpTimeout = 0.50f;

        private void OnTriggerEnter(Collider collision)
        {
            if (collision.gameObject.CompareTag("LongJump"))
            {
                powerUpAnimation.PlayRandomPowerUp();
                

                JumpHeight = 2.6f;
                Destroy(collision.gameObject);
            }

            if (collision.gameObject.CompareTag("DoubleJump"))
            {
                powerUpAnimation.PlayRandomPowerUp();
                

                doubleJumpUnlocked = true;
                Destroy(collision.gameObject);
            }

            if (collision.gameObject.CompareTag("Egg"))
            {
                powerUpAnimation.PlayRandomPowerUp();
                

                collision.transform.SetParent(BackObjectPosition);
                collision.transform.localPosition = Vector3.zero;
                collision.transform.localRotation = Quaternion.identity;
            }

            if (collision.gameObject.CompareTag("Shrink"))
            {
                powerUpAnimation.PlayRandomPowerUp();
                

                _canShrink = true;
                Destroy(collision.gameObject);
            }
        }

        [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
        public float FallTimeout = 0.15f;

        [SerializeField] private Transform BackObjectPosition;

        [Header("Shrink PowerUp")]
        [SerializeField] private Transform PlayerModel;
        [SerializeField] private float ShrinkMultiplier = 0.25f;

        private bool _canShrink = false;
        private bool _isShrunk = false;

        private Vector3 _originalModelScale;
        private float _originalControllerHeight;
        private float _originalControllerRadius;
        private Vector3 _originalControllerCenter;

        public bool _inShrinkZone = false;

        [SerializeField] private int maxJumps = 2;
        private int jumpsRemaining;

        [SerializeField] private bool doubleJumpUnlocked = false;

        private void DoubleJump()
        {
            if (!doubleJumpUnlocked)
                return;

            if (Grounded)
            {
                jumpsRemaining = maxJumps;
            }

            if (!Grounded && _input.jump && jumpsRemaining > 0)
            {
                _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);

                jumpsRemaining--;

                _input.jump = false;
            }
        }

        private void Shrink()
        {
            if (!_canShrink)
                return;

            bool shrinkPressed = _playerInput.actions["Shrink"].IsPressed();

            RaycastHit hit;

            bool inShrinkZoneByRaycast = Physics.Raycast(
                transform.position,
                Vector3.down,
                out hit,
                0.5f,
                LayerMask.GetMask("Shrink")
            );

            bool inShrinkZone = _inShrinkZone || inShrinkZoneByRaycast;

            _animator.SetBool(_animIDShrink, shrinkPressed);

            if (shrinkPressed || inShrinkZone)
            {
                if (!_isShrunk)
                {
                    _isShrunk = true;

                    float newHeight = _originalControllerHeight * ShrinkMultiplier;
                    float newRadius = _originalControllerRadius * ShrinkMultiplier;

                    float originalBottom = _originalControllerCenter.y - (_originalControllerHeight / 2f);

                    _controller.height = newHeight;
                    _controller.radius = newRadius;

                    Vector3 newCenter = _originalControllerCenter;
                    newCenter.y = originalBottom + (newHeight / 2f);
                    _controller.center = newCenter;

                    PlayerModel.localScale = _originalModelScale * ShrinkMultiplier;
                }
            }
            else
            {
                if (_isShrunk)
                {
                    _isShrunk = false;

                    PlayerModel.localScale = _originalModelScale;

                    _controller.height = _originalControllerHeight;
                    _controller.radius = _originalControllerRadius;
                    _controller.center = _originalControllerCenter;

                    _verticalVelocity = 0f;
                }
            }
        }

        public void ForceShrink(bool shrink)
        {
            if (shrink && !_isShrunk)
            {
                _isShrunk = true;

                float newHeight = _originalControllerHeight * ShrinkMultiplier;
                float newRadius = _originalControllerRadius * ShrinkMultiplier;

                float originalBottom = _originalControllerCenter.y - (_originalControllerHeight / 2f);

                _controller.height = newHeight;
                _controller.radius = newRadius;

                Vector3 newCenter = _originalControllerCenter;
                newCenter.y = originalBottom + (newHeight / 2f);
                _controller.center = newCenter;

                PlayerModel.localScale = _originalModelScale * ShrinkMultiplier;
            }
            else if (!shrink && _isShrunk)
            {
                _isShrunk = false;

                PlayerModel.localScale = _originalModelScale;

                _controller.height = _originalControllerHeight;
                _controller.radius = _originalControllerRadius;
                _controller.center = _originalControllerCenter;
            }
        }

        [Header("Player Grounded")]
        [Tooltip("If the character is grounded or not. Not part of the character controller built in grounded check")]
        public bool Grounded = true;

        [Tooltip("Useful for rough ground")]
        public float GroundedOffset = -0.14f;

        [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
        public float GroundedRadius = 0.28f;

        [Tooltip("What layers the character uses as ground")]
        public LayerMask GroundLayers;

        [Header("Cinemachine")]
        [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
        public GameObject CinemachineCameraTarget;

        [Tooltip("How far in degrees can you move the camera up")]
        public float TopClamp = 70.0f;

        [Tooltip("How far in degrees can you move the camera down")]
        public float BottomClamp = -30.0f;

        [Tooltip("Additional degrees to override the camera. Useful for fine tuning camera position when locked")]
        public float CameraAngleOverride = 0.0f;

        [Tooltip("For locking the camera position on all axis")]
        public bool LockCameraPosition = false;

        private float _cinemachineTargetYaw;
        private float _cinemachineTargetPitch;

        private float _speed;
        private float _animationBlend;
        private float _targetRotation = 0.0f;
        private float _rotationVelocity;
        private float _verticalVelocity;
        private float _terminalVelocity = 53.0f;

        private float _jumpTimeoutDelta;
        private float _fallTimeoutDelta;

        private int _animIDSpeed;
        private int _animIDGrounded;
        private int _animIDJump;
        private int _animIDFreeFall;
        private int _animIDMotionSpeed;
        private int _animIDShrink;

#if ENABLE_INPUT_SYSTEM
        private PlayerInput _playerInput;
#endif

        private Animator _animator;
        private CharacterController _controller;
        private StarterAssetsInputs _input;
        private GameObject _mainCamera;

        private const float _threshold = 0.01f;

        private bool _hasAnimator;

        private bool IsCurrentDeviceMouse
        {
            get
            {
#if ENABLE_INPUT_SYSTEM
                return _playerInput.currentControlScheme == "KeyboardMouse";
#else
                return false;
#endif
            }
        }

        private void Awake()
        {
            if (_mainCamera == null)
            {
                _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            }
        }

        private void Start()
        {
            _cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;

            _hasAnimator = TryGetComponent(out _animator);
            _controller = GetComponent<CharacterController>();

            _originalModelScale = PlayerModel.localScale;

            _originalControllerHeight = _controller.height;
            _originalControllerRadius = _controller.radius;
            _originalControllerCenter = _controller.center;

            _input = GetComponent<StarterAssetsInputs>();

#if ENABLE_INPUT_SYSTEM
            _playerInput = GetComponent<PlayerInput>();
#else
            UnityEngine.Debug.LogError("Starter Assets package is missing dependencies. Please use Tools/Starter Assets/Reinstall Dependencies to fix it");
#endif

            AssignAnimationIDs();

            _jumpTimeoutDelta = JumpTimeout;
            _fallTimeoutDelta = FallTimeout;
        }

        private void Update()
        {
            _hasAnimator = TryGetComponent(out _animator);

            if (powerUpAnimation != null && powerUpAnimation.IsPlayingPowerUp)
            {
                return;
            }

            if (_animator.GetCurrentAnimatorStateInfo(1).IsName("Dance"))
            {
                return;
            }

            JumpAndGravity();
            GroundedCheck();
            Shrink();
            Move();
            DoubleJump();
        }

        private void LateUpdate()
        {
            if (_isShrunk)
            {
                PlayerModel.localScale = _originalModelScale * ShrinkMultiplier;
            }
            else
            {
                PlayerModel.localScale = _originalModelScale;
            }

            CameraRotation();
        }

        private void AssignAnimationIDs()
        {
            _animIDSpeed = Animator.StringToHash("Speed");
            _animIDGrounded = Animator.StringToHash("Grounded");
            _animIDJump = Animator.StringToHash("Jump");
            _animIDFreeFall = Animator.StringToHash("FreeFall");
            _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
            _animIDShrink = Animator.StringToHash("Shrink");
        }

        private void GroundedCheck()
        {
            Vector3 spherePosition = new Vector3(
                transform.position.x,
                transform.position.y - GroundedOffset,
                transform.position.z
            );

            Grounded = Physics.CheckSphere(
                spherePosition,
                GroundedRadius,
                GroundLayers,
                QueryTriggerInteraction.Ignore
            );

            if (_hasAnimator)
            {
                _animator.SetBool(_animIDGrounded, Grounded);
            }
        }

        private void CameraRotation()
        {
            if (_input.look.sqrMagnitude >= _threshold && !LockCameraPosition)
            {
                float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

                _cinemachineTargetYaw += _input.look.x * deltaTimeMultiplier;
                _cinemachineTargetPitch += _input.look.y * deltaTimeMultiplier;
            }

            _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

            CinemachineCameraTarget.transform.rotation = Quaternion.Euler(
                _cinemachineTargetPitch + CameraAngleOverride,
                _cinemachineTargetYaw,
                0.0f
            );
        }

        private void Move()
        {
            float targetSpeed = _input.sprint ? SprintSpeed : MoveSpeed;

            if (_input.move == Vector2.zero)
                targetSpeed = 0.0f;

            float currentHorizontalSpeed = new Vector3(
                _controller.velocity.x,
                0.0f,
                _controller.velocity.z
            ).magnitude;

            float speedOffset = 0.1f;
            float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;

            if (currentHorizontalSpeed < targetSpeed - speedOffset ||
                currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                _speed = Mathf.Lerp(
                    currentHorizontalSpeed,
                    targetSpeed * inputMagnitude,
                    Time.deltaTime * SpeedChangeRate
                );

                _speed = Mathf.Round(_speed * 1000f) / 1000f;
            }
            else
            {
                _speed = targetSpeed;
            }

            _animationBlend = Mathf.Lerp(
                _animationBlend,
                targetSpeed,
                Time.deltaTime * SpeedChangeRate
            );

            if (_animationBlend < 0.01f)
                _animationBlend = 0f;

            Vector3 inputDirection = new Vector3(
                _input.move.x,
                0.0f,
                _input.move.y
            ).normalized;

            if (_input.move != Vector2.zero)
            {
                _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) *
                                  Mathf.Rad2Deg +
                                  _mainCamera.transform.eulerAngles.y;

                float rotation = Mathf.SmoothDampAngle(
                    transform.eulerAngles.y,
                    _targetRotation,
                    ref _rotationVelocity,
                    RotationSmoothTime
                );

                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }

            Vector3 targetDirection = Quaternion.Euler(
                0.0f,
                _targetRotation,
                0.0f
            ) * Vector3.forward;

            Vector3 playerMovement = targetDirection.normalized * (_speed * Time.deltaTime);

            _controller.Move(
                playerMovement + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime
            );

            if (_hasAnimator)
            {
                _animator.SetFloat(_animIDSpeed, _animationBlend);
                _animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
            }
        }

        public bool IsMoving()
        {
            return _input.move != Vector2.zero;
        }

        private void JumpAndGravity()
        {
            if (Grounded)
            {
                _fallTimeoutDelta = FallTimeout;

                if (_hasAnimator)
                {
                    _animator.SetBool(_animIDJump, false);
                    _animator.SetBool(_animIDFreeFall, false);
                }

                if (_verticalVelocity < 0.0f)
                {
                    _verticalVelocity = -2f;
                }

                if (_input.jump && _jumpTimeoutDelta <= 0.0f)
                {
                    _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);

                    if (audioJugador != null)
                        audioJugador.ReproducirSalto();

                    if (_hasAnimator)
                    {
                        _animator.SetBool(_animIDJump, true);
                    }
                }

                if (_jumpTimeoutDelta >= 0.0f)
                {
                    _jumpTimeoutDelta -= Time.deltaTime;
                }
            }
            else
            {
                _jumpTimeoutDelta = JumpTimeout;

                if (_fallTimeoutDelta >= 0.0f)
                {
                    _fallTimeoutDelta -= Time.deltaTime;
                }
                else
                {
                    if (_hasAnimator)
                    {
                        _animator.SetBool(_animIDFreeFall, true);
                    }
                }

                if (!doubleJumpUnlocked)
                {
                    _input.jump = false;
                }
            }

            if (_verticalVelocity < _terminalVelocity)
            {
                _verticalVelocity += Gravity * Time.deltaTime;
            }
        }

        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;

            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }

        private void OnDrawGizmosSelected()
        {
            Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
            Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

            if (Grounded)
                Gizmos.color = transparentGreen;
            else
                Gizmos.color = transparentRed;

            Gizmos.DrawSphere(
                new Vector3(
                    transform.position.x,
                    transform.position.y - GroundedOffset,
                    transform.position.z
                ),
                GroundedRadius
            );
        }

        private void OnFootstep(AnimationEvent animationEvent)
        {
            // Bloquear sonidos de pasos durante la animacion del PowerUp
            if (powerUpAnimation != null && powerUpAnimation.IsPlayingPowerUp)
                return;

            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                if (AudioFootsteps != null)
                    AudioFootsteps.Play();

                if (AudioFoley != null)
                    AudioFoley.Play();
            }
        }

        private void OnLand(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                if (LandingAudio != null)
                    LandingAudio.Play();
            }
        }
    }
}