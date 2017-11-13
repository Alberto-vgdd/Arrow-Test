using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowMovement : MonoBehaviour 
{
	[Header("Arrow Transforms")]
	public Transform m_ArrowTransform;
	public Transform m_ArrowBaseTransform;
	public Transform m_ArrowBodyTransform;
	public Transform m_ArrowTipTransform;

	[Header("Scene Camera")]
	public Camera m_SceneCamera;

	[Header("Arrow Parameters")]
	public float m_RotationSpeedInDegrees;
	public float m_ScaleTime;
	public float m_MinimumBodyScale;
	public float m_MaximumBodyScale;
	

	// Default Arrow lengths
	private float m_DefaultBodyLength;
	private float m_DefaultBaseLength;
	private float m_DefaultTipLength;

	// Variables to smoothly rotate and scale
	private Vector3 m_TargetArrowUp;
	private float m_TargetBodyScale;
	private float m_BodyScaleVelocity;
	private float m_RotationSpeedInRadians;

	// Input variables
	private Vector3 m_MousePositionInWorldCoordinates;



	void Awake() 
	{
		m_DefaultBodyLength = m_ArrowBodyTransform.GetComponent<BoxCollider2D>().size.y;
		m_DefaultBaseLength = m_ArrowBaseTransform.GetComponent<BoxCollider2D>().size.y;
		m_DefaultTipLength = m_ArrowTipTransform.GetComponent<BoxCollider2D>().size.y;

		m_RotationSpeedInRadians = Mathf.Deg2Rad * m_RotationSpeedInDegrees;
	}
	

	void Update ()
	{
		TurnArrow();
		ScaleArrow();
	}


	void TurnArrow()
	{
		// Read mouse inputs
		m_MousePositionInWorldCoordinates = m_SceneCamera.ScreenToWorldPoint( Input.mousePosition + Vector3.forward*m_SceneCamera.nearClipPlane);
		m_TargetArrowUp = (m_MousePositionInWorldCoordinates - m_ArrowTransform.position); m_TargetArrowUp.z = 0;

		// Turn the arrow.
		m_ArrowTransform.up = Vector3.RotateTowards( m_ArrowTransform.up,m_TargetArrowUp,m_RotationSpeedInRadians*Time.deltaTime,0.0f);

	}

	void ScaleArrow()
	{
		if (Input.GetMouseButton(0))
		{
			
			m_TargetBodyScale = (m_TargetArrowUp.magnitude -(m_DefaultBaseLength+m_DefaultTipLength))/m_DefaultBodyLength;
			m_TargetBodyScale = Mathf.Clamp(m_TargetBodyScale, m_MinimumBodyScale, m_MaximumBodyScale);
		}
		else
		{
			m_TargetBodyScale = 1f;
		}

		// Scale the arrow.
		m_ArrowBodyTransform.localScale = new Vector3(1f,Mathf.SmoothDamp(m_ArrowBodyTransform.localScale.y,m_TargetBodyScale,ref m_BodyScaleVelocity,m_ScaleTime),1f); 
		m_ArrowTipTransform.localPosition = Vector3.up*( m_ArrowBodyTransform.localPosition.y+m_ArrowBodyTransform.localScale.y*m_DefaultBodyLength);
	}
}
