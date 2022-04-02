using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Rendering.VirtualTexturing;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public int GridX;
    public int GridY;

    private bool _canMove = true;
    private bool _isMoving = false;
    private float _moveTime = .333f;

    [Header("Animation")]
    public Animator Animator;

    private Transform _player_ctrl;

    void Start()
    {
        _player_ctrl = transform.Find("player_ctrl").GetComponent<Transform>();

        SnapToGridPosition(GridX, GridY);
    }

    void Update()
    {
        HandleMovement();
    }

    /// <summary>
    /// Handle player input for movement.
    /// </summary>
    void HandleMovement()
    {
        if (_canMove && !_isMoving)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                MoveDirection(Direction.North);
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                MoveDirection(Direction.South);
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                MoveDirection(Direction.NorthWest);
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                MoveDirection(Direction.NorthEast);
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                MoveDirection(Direction.SouthWest);
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                MoveDirection(Direction.SouthEast);
            }
        }
    }

    /// <summary>
    /// Snap to a grid position, with no animation.
    /// </summary>
    public void SnapToGridPosition(int gx, int gy)
    {
        transform.position = Utilities.GridToWorldPosition(gx, gy);
    }

    /// <summary>
    /// Move in a direction.
    /// </summary>
    public void MoveDirection(Direction direction)
    {
        SetFacingDirection(direction);
        var newPos = Utilities.TranslatePosition(GridX, GridY, direction);
        GridX = newPos.x;
        GridY = newPos.y;
        Animator.Play("move");
        StartCoroutine(Move(newPos.x, newPos.y));
    }

    /// <summary>
    /// Coroutine that moves and animates the player.
    /// </summary>
    private IEnumerator Move(int gx, int gy)
    {
        _isMoving = true;

        var startPos = transform.position;
        var endPos = Utilities.GridToWorldPosition(gx, gy);

        var timer = 0f;
        while (timer < _moveTime)
        {
            var t = timer / _moveTime;
            transform.position = Vector3.Lerp(startPos, endPos, t);
            timer += Time.deltaTime;
            yield return null;
        }
        
        SnapToGridPosition(gx, gy);
        
        _isMoving = false;
    }

    /// <summary>
    /// Rotate the player to face a direction.
    /// </summary>
    public void SetFacingDirection(Direction direction)
    {
        _player_ctrl.rotation = Utilities.RotationFromDirection(direction);
    }
}