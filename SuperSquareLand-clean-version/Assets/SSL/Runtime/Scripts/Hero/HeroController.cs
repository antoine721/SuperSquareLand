using System.Collections;
using UnityEngine;

public class HeroController : MonoBehaviour
{
    [Header("Entity")]
    [SerializeField] private HeroEntity _entity;
    private bool _entityWasTouchingGround = false;

    [Header("Debug")]
    [SerializeField] private bool _guiDebug = false;

    [Header("Jump Buffer")]
    [SerializeField] private float _jumpBufferDuration = 0.2f;
    private float _jumpBufferTimer = 0f;
    [Header("Coyote Time")]
    [SerializeField] private float _coyoteTimeDuration = 0.2f;
    private float _coyoteTimeCountdown = -1f;



    [SerializeField] private HeroDashSettings _dashSettings;



    private void OnGUI()
    {
        if (!_guiDebug) return;

        GUILayout.BeginVertical(GUI.skin.box);
        GUILayout.Label(gameObject.name);
        GUILayout.Label($"Jump Buffer Timer = {_jumpBufferTimer}");
        GUILayout.Label($"CoyoteTime Countdown = {_coyoteTimeCountdown}");
        GUILayout.EndVertical();
    }
    private void Update()
    {
        _UpdateJumpBuffer();
        _entity.SetMoveDirX(GetInputMoveX());

        if (_EntityHasExitGround())
        {
            _ResetCoyoteTime();
        }
        else
        {
            _UpdateCoyoteTime();
        }

        if (_GetInputDownJump())
        {
            if ((_entity.IsTouchingGround || _isCoyoteTileActive()) && !_entity.IsJumping)
            {
                _entity.JumpStart();
            }
        }
        if (_entity.IsJumpingImpulsing)
        {
            if (!_GetInputJump() && _entity.IsJumpMinDurationReached)
            {
                _entity.StopJumpImpulsion();
            }
            else
            {
                _ResetJumpBuffer();
            }
        }
        if (IsJumpBufferActive())
        {
            if ((_entity.IsTouchingGround || _isCoyoteTileActive()) && !_entity.IsJumping)
            {
                _entity.JumpStart();
            }
        }
        _entityWasTouchingGround = _entity.IsTouchingGround;
    }
    private bool _GetInputJump()
    {
        return Input.GetKey(KeyCode.Space);
    }
    private bool _GetInputDownJump()
    {
        return Input.GetKeyDown(KeyCode.Space);
    }
    private float GetInputMoveX()
    {
        float InputMoveX = 0f;

        if (Input.GetKey(KeyCode.A))
        {
            InputMoveX = -1f;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            InputMoveX = 1f;
        }

        return InputMoveX;
    }

    private void _ResetJumpBuffer()
    {
        _jumpBufferTimer = 0f;
    }
    private bool IsJumpBufferActive()
    {
        return _jumpBufferTimer < _jumpBufferDuration;
    }
    private void _UpdateJumpBuffer()
    {
        if (!IsJumpBufferActive()) return;
        _jumpBufferTimer += Time.deltaTime;
    }
    private void _CancelJumpBuffer()
    {
        _jumpBufferTimer = _jumpBufferDuration;
    }
    private void _UpdateCoyoteTime()
    {
        if (!_isCoyoteTileActive()) return;
        _coyoteTimeCountdown -= Time.deltaTime;
    }
    private bool _isCoyoteTileActive()
    {
        return _coyoteTimeCountdown > 0f;
    }
    private void _ResetCoyoteTime()
    {
        _coyoteTimeCountdown = _coyoteTimeDuration;
    }
    private bool _EntityHasExitGround()
    {
        return _entityWasTouchingGround && !_entity.IsTouchingGround;
    }

   
    }





