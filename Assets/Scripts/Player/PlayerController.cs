using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerInput _playerInput;
    private FrameInput _frameInput;
    private Camera _mainCamera;
    
    
    // Visuals
    [SerializeField] private Sprite[] bodySprites;
    
    [SerializeField] private SpriteRenderer bodySpriteRenderer;
    [SerializeField] private SpriteRenderer burnerSpriteRenderer;
    
    [SerializeField] private Animator burnerAnimator;
    [SerializeField] private ParticleSystem trailPS;
    
    private float _playerRotationSpeed = 250f;
    private float _playerMoveSpeedLimit = 15f;
    
    private Vector2 _playerSpeed = Vector2.zero;
    private float _playerSpeedAcceleration = 20f;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _trailPSMain = trailPS.main;
    }

    private void FixedUpdate()
    {
        GatherInput();
            
        UpdatePlayerSprite(_currentAngle);
        UpdateBurnerSprite();
        UpdateTrailEffects();

        UpdatePlayerSpeed();
        UpdatePlayerTransform();
    }

    #region INPUT

    private float _currentAngle, _targetAngle;

    private void GatherInput()
    {
        _frameInput = _playerInput.frameInput;

        var position = transform.position;
        _targetAngle = CalculateAngleToTarget(new Vector2(position.x, position.y), _frameInput.MousePosition);
        _currentAngle = transform.rotation.eulerAngles.z;
    }
    
    #endregion

    #region VISUALS

    private void UpdatePlayerSprite(float playerAngle)
    {
        if (playerAngle < 0f)
            playerAngle += 360f;
        playerAngle = playerAngle % 360f + 22.5f;

        int index = (int)(playerAngle / 45f) % 8;
        // Debug.Log(index);

        bodySpriteRenderer.sprite = bodySprites[index];
        // _burnerSpriteRenderer.sprite = burnerSprites[index];
    }
    
    private static readonly int Pressed = Animator.StringToHash("Pressed");
    private void UpdateBurnerSprite()
    {
        burnerAnimator.SetBool(Pressed, _frameInput.ThrustHeld);
    }

    private float _currentThrust = 0f;
    private ParticleSystem.MainModule _trailPSMain;
    private void UpdateTrailEffects()
    {
        if (_frameInput.ThrustHeld)
            _currentThrust = Mathf.Clamp(_currentThrust + Time.deltaTime, 0f, 0.5f);
        else
        {
            _currentThrust = Mathf.Clamp(_currentThrust - Time.deltaTime, 0f, 0.5f);
            if (_currentThrust < 0.2f)
                _currentThrust = 0f;
        }

        float targetSize = _currentThrust * 2f * 0.13f;
        _trailPSMain.startSize = new ParticleSystem.MinMaxCurve(targetSize, targetSize * 1.2f);
    }

    #endregion

    #region MOVEMENT

    private void UpdatePlayerSpeed()
    {
        if (_frameInput.ThrustHeld)
        {
            _playerSpeed += (Vector2)transform.right * (_playerSpeedAcceleration * Time.deltaTime);
        }

        if (_playerSpeed.magnitude > _playerMoveSpeedLimit)
        {
            _playerSpeed = _playerSpeed.normalized * _playerMoveSpeedLimit;
        }
    }
    
    private void UpdatePlayerTransform()
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0f, 0f, _targetAngle), _playerRotationSpeed * Time.deltaTime);
        transform.position += new Vector3(_playerSpeed.x, _playerSpeed.y, 0f) * Time.deltaTime;
    }

    #endregion
    
    #region UTILS

    private float CalculateAngleToTarget(Vector2 origin, Vector2 target)
    {
        return Mathf.Atan2(target.y - origin.y, target.x - origin.x) * Mathf.Rad2Deg;
    }

    #endregion

    #region EXTERNAL

    public Vector2 GetPlayerSpeed()
    {
        return _playerSpeed;
    }

    #endregion
}
