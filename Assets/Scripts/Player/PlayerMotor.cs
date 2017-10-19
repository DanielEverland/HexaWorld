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
	private bool _isFlying = true;

    private float yRotation, xRotation;

	private void Update()
	{
		PollKeys ();
		DoMovement ();
		MoveCamera ();
	}
	private void PollKeys()
	{
		_usingBoost = Input.GetKey (KeyCode.LeftShift);

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
            yRotation -= Input.GetAxis("Mouse Y") * _mouseSensitivity;
            xRotation += Input.GetAxis("Mouse X") * _mouseSensitivity;

            yRotation = Utility.ClampEulerAngle(yRotation, -90, 90);

            Quaternion yQuaternion = Quaternion.AngleAxis(yRotation, Vector3.right);
            Quaternion xQuaternion = Quaternion.AngleAxis(xRotation, Vector3.up);

            transform.rotation = Quaternion.identity * xQuaternion * yQuaternion;
        }
	}
	private void DoMovement()
	{
		transform.position += _movementDirection * ((_movementSpeed * (_usingBoost ? 2 : 1)) * Time.deltaTime);
	}
}
