using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joint : BodyPart 
{
    /* Joint :
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
    [SerializeField] private Dictionary<Bone, HingeJoint> joints = new Dictionary<Bone, HingeJoint>();
    #endregion

    #region Methods

    #region Original Methods
    /// <summary>
    /// Create a joint between the a bone and this joint
    /// Create a joint and connect it to the bone Body
    /// Add it to the joints List
    /// </summary>
    /// <param name="_boneToConnect"></param>
    public void ConnectBone(Bone _boneToConnect)
    {
        HingeJoint _joint = gameObject.AddComponent<HingeJoint>();
        _joint.anchor = Vector3.zero;
        _joint.axis = Vector3.forward;
        _joint.autoConfigureConnectedAnchor = true;
        _joint.useSpring = false;
        _joint.enableCollision = false;
        _joint.enablePreprocessing = true;
        _joint.connectedBody = _boneToConnect.Body;

        joints.Add(_boneToConnect, _joint); 
    }

    protected override void Reset()
    {
        base.Reset();
        body.velocity = Vector3.zero; 
    }
    #endregion

    #region Unity Methods
    // Awake is called when the script instance is being loaded
    protected override void Awake()
    {
        base.Awake(); 
    }

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        if (!body) body = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    private void Update()
    {
        
	}
	#endregion

	#endregion
}
