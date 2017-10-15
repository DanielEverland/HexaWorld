using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour {

	[SerializeField]
	private float _movementSpeed = 10;
	[SerializeField]
	private float _mouseSensitivity = 10;

	private bool _usingBoost;
	private Vector3 _movementDirection;
	private Vector2 _mouseDirection;
	private Vector3 _oldMousePosition;
	private bool _isFlying = true;

	private void Update()
	{
		PollKeys ();
		DoMovement ();
		MoveCamera ();

		_oldMousePosition = Input.mousePosition;
	}
	private void PollKeys()
	{
		_usingBoost = Input.GetKey (KeyCode.LeftShift);
		_mouseDirection = Input.mousePosition - _oldMousePosition;

		_movementDirection = Vector3.zero;

		_movementDirection += (Input.GetKey (KeyCode.W)) ? transform.forward : Vector3.zero;
		_movementDirection += (Input.GetKey (KeyCode.S)) ? -transform.forward : Vector3.zero;
		_movementDirection += (Input.GetKey (KeyCode.D)) ? transform.right : Vector3.zero;
		_movementDirection += (Input.GetKey (KeyCode.A)) ? -transform.right : Vector3.zero;

		if (_isFlying)
		{
			_movementDirection += (Input.GetKey (KeyCode.E)) ? Vector3.up : Vector3.zero;
			_movementDirection += (Input.GetKey (KeyCode.C)) ? -Vector3.up : Vector3.zero;
		}
	}
	private void MoveCamera()
	{
		if (Input.GetKey (KeyCode.Mouse1))
		{
			Vector3 euler = transform.eulerAngles;

			euler.y += _mouseDirection.x * (_mouseSensitivity * Time.deltaTime);
			euler.x = Mathf.Clamp (euler.x - _mouseDirection.y * (_mouseSensitivity * Time.deltaTime), -360, 360);

			transform.eulerAngles = euler;
		}
	}
	private void DoMovement()
	{
		transform.position += _movementDirection * ((_movementSpeed * (_usingBoost ? 2 : 1)) * Time.deltaTime);
	}
}
