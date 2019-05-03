using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Automaton : MonoBehaviour 
{
    /* Automaton :
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
    public List<Bone> Bones;
    public List<Joint> Joints;
    public List<Muscle> Muscles;
    public Brain Brain;

    [SerializeField] private bool isAlive = false; 
    public bool IsAlive
    {
        get { return isAlive;  }
        set
        {
            isAlive = value;
            Bones.ForEach(b => b.IsAlive = isAlive);
            Joints.ForEach(j => j.IsAlive = isAlive);
            Muscles.ForEach(m => m.IsAlive = isAlive);
            Brain.IsAlive = isAlive; 
        }
    }
    #endregion

    #region Methods

    #region Original Methods
    public void Reset()
    {
        IsAlive = false; 
        Bones.ForEach(b => b.Reset());
        Joints.ForEach(b => b.Reset());
        Muscles.ForEach(b => b.Reset());
    }

    /// <summary>
    /// Get brain Inputs
    /// 
    /// DistanceFromFloor 
    /// VelocityX
    /// VelocityY
    /// VelocityZ
    /// AngularVelocity 
    /// GroundedPointsCounts
    /// Rotation
    /// 
    /// </summary>
    /// <returns></returns>
    public BrainInputs UpdateBrainInputs()
    {
        float minJointY = Joints[0].transform.position.y;
        float maxJointY = Joints[0].transform.position.y;
        float velocityX = 0f;
        float velocityY = 0f;
        float velocityZ = 0f; 
        float angularVelZ = 0f;
        int jointsCountTouchingGround = 0;
        float rotationZ = 0f;

        int jointCount = Joints.Count;
        int boneCount = Bones.Count;

        //var jointRadius = joints[0].GetComponent<Collider>().bounds.size.y / 2;

        for (int i = 0; i < jointCount; i++)
        {

            Joint joint = Joints[i];
            Vector3 jointPos = joint.transform.position;

            // Determine lowest and highest joints
            if (jointPos.y > maxJointY)
                maxJointY = jointPos.y;
            else if (jointPos.y < minJointY)
                minJointY = jointPos.y;

            // Accumulate the velocity
            velocityX += joint.Body.velocity.x;
            velocityY += joint.Body.velocity.y;
            velocityZ += joint.Body.velocity.z; 

            // Check if the joint is touching the ground
            jointsCountTouchingGround += joint.IsCollidingWithGround ? 1 : 0;
        }

        for (int i = 0; i < boneCount; i++)
        {
            Bone bone = Bones[i];

            angularVelZ += bone.Body.angularVelocity.z;
            // Not actually the rotation value I originally wanted to have 
            // but I have to keep this for the sake of not breaking existing
            // creature behaviour
            rotationZ += bone.transform.rotation.z;
        }

        float distFromFloor = minJointY;
        velocityX /= jointCount;
        velocityY /= jointCount;
        angularVelZ /= boneCount;
        rotationZ /= boneCount;


        return new BrainInputs()
        {
            DistanceFromFloor = distFromFloor,
            VelocityX = velocityX,
            VelocityY = velocityY,
            VelocityZ = velocityZ,
            AngularVelocity = angularVelZ,
            GroundedPointsCounts = jointsCountTouchingGround,
            Rotation = rotationZ
        };
    }
	#endregion

	#region Unity Methods
	// Awake is called when the script instance is being loaded
    private void Awake()
    {

    }

	// Use this for initialization
    private void Start()
    {
		
    }
	
	// Update is called once per frame
	private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && IsAlive)
            Reset();
        if (Input.GetKeyDown(KeyCode.Space))
            IsAlive = true; 
	}
	#endregion

	#endregion
}
