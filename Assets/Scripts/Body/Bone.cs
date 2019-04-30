using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bone : BodyPart 
{
    /* Bone :
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
    [SerializeField] private Joint startingJoint;
    [SerializeField] private Joint endingJoint;
    #endregion

    #region Methods

    #region Original Methods
    public void ConnectWithJoint()
    {
        if (startingJoint)
        {
            startingJoint.ConnectBone(this);
        }
        if (endingJoint)
        {
            endingJoint.ConnectBone(this); 
        }
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
        ConnectWithJoint();
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
