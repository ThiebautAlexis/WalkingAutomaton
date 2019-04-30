using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Muscle : BodyPart 
{
    /* Muscle :
	 *
	 *	#####################
	 *	###### PURPOSE ######
	 *	#####################
	 *
	 *	[PURPOSE]
	 *
	 *	#####################
	 *	####### TO DO #######
	 *	#####################
	 *
	 *	[TO DO]
	 *
	 *	#####################
	 *	### MODIFICATIONS ###
	 *	#####################
	 *
	 *	Date :			[DATE]
	 *	Author :		[NAME]
	 *
	 *	Changes :
	 *
	 *	[CHANGES]
	 *
	 *	-----------------------------------
	*/

    #region Events

    #endregion

    #region Fields / Properties
    [SerializeField] private Bone startingBone;
    [SerializeField] private Bone endingBone;

    private MuscleAction muscleAction = MuscleAction.Contraction;

    private SpringJoint muscleJoint;

    [SerializeField] private float springStrength = 1000;
    [SerializeField] private float muscleForceMax = 1500;
    public float CurrentForce = 0;

    #endregion

    #region Methods

    #region Original Methods
    /// <summary>
    /// 
    /// </summary>
    /// <param name="_force"></param>
    /// <param name="_startingForce"></param>
    /// <param name="_endingForce"></param>
    private void ApplyForces(float _force, Vector3 _startingForce, Vector3 _endingForce)
    {
        //Set the scale of the vector to the _force 
        Vector3 _scale = new Vector3(_force, _force, _force);
        _endingForce.Scale(_scale);
        _startingForce.Scale(_scale);

        startingBone.Body.AddForceAtPosition(_startingForce, startingBone.transform.position);
        endingBone.Body.AddForceAtPosition(_endingForce, endingBone.transform.position);
    }

    /// <summary>
    /// Connect the muscle between two bones
    /// Place a spring joint between them
    /// </summary>
    public void ConnectToBones()
    {
        if (startingBone == null || endingBone == null) return;


        // connect the musclejoints with a spring joint
        muscleJoint = startingBone.gameObject.AddComponent<SpringJoint>();
        muscleJoint.spring = springStrength;
        muscleJoint.damper = 50;
        muscleJoint.minDistance = 0;
        muscleJoint.maxDistance = 0;
        //spring.autoConfigureConnectedAnchor = true;
        muscleJoint.anchor = startingBone.transform.position;
        muscleJoint.connectedAnchor = endingBone.transform.position;

        muscleJoint.connectedBody = endingBone.Body; // Connect to muscle joint (Default)

        muscleJoint.enablePreprocessing = true;
        muscleJoint.enableCollision = false;
    }

    /// <summary>
    /// Calculate forces to apply when the muscle has to contract
    /// </summary>
    private void Contract()
    {       
        Vector3 _startingPosition = startingBone.transform.position;
        Vector3 _endingPosition = endingBone.transform.position;

        // Apply a force on both connection joints.
        Vector3 midPoint = (_startingPosition + _endingPosition) / 2;

        Vector3 endingForce = (midPoint - _endingPosition).normalized;
        Vector3 startingForce = (midPoint - _startingPosition).normalized;

        Debug.Log("Contract"); 

        ApplyForces(CurrentForce, startingForce, endingForce);
    }

    /// <summary>
    /// Calculate forces to apply when the muscle has to expand
    /// </summary>
    private void Expand()
    {
        Vector3 _startingPosition = startingBone.transform.position;
        Vector3 _endingPosition = endingBone.transform.position;

        // Apply a force on both connection joints.
        Vector3 midPoint = (_startingPosition + _endingPosition) / 2;

        Vector3 endingForce = (_endingPosition - midPoint).normalized;
        Vector3 startingForce = (_startingPosition - midPoint).normalized;

        ApplyForces(CurrentForce, startingForce, endingForce);
    }

    /// <summary>
    /// Update the currentforce using a percentage
    /// </summary>
    /// <param name="_percent"></param>
    public void UpdateCurrentForce(float _percent)
    {
        CurrentForce = Mathf.Max(0.01f, Mathf.Min(muscleForceMax, _percent * muscleForceMax));
    }

    protected override void Reset()
    {
        base.Reset();
        CurrentForce = 0; 
    }
    #endregion

    #region Unity Methods
    // Awake is called when the script instance is being loaded
    protected override void Awake()
    {
        base.Awake();
        ConnectToBones();
    }


    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        body.isKinematic = true; 
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void OnDrawGizmos()
    {
        if (!startingBone || !endingBone) return;
        switch (muscleAction)
        {
            case MuscleAction.Contraction:
                Gizmos.color = Color.red; 
                break;
            case MuscleAction.Expansion:
                Gizmos.color = Color.blue; 
                break;
            default:
                Gizmos.color = Color.white; 
                break;
        }
        Gizmos.DrawLine(startingBone.Body.position, endingBone.Body.position);
    }
    #endregion

    #endregion
}

public enum MuscleAction
{
    Contraction, 
    Expansion
}
