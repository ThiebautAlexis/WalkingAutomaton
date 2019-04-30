using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class BodyPart : MonoBehaviour 
{
    /* BodyPart :
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
    protected Vector3 initialPosition;
    protected Quaternion initialRotation;

    protected Rigidbody body;
    public Rigidbody Body
    {
        get
        {
            if (!body) body = GetComponent<Rigidbody>();
            return body;
        }
    }
    #endregion

    #region Methods

    #region Original Methods
    protected virtual void Reset()
    {
        transform.SetPositionAndRotation(initialPosition, initialRotation);
        body.velocity = Vector3.zero; 
    }
    #endregion

    #region Unity Methods
    // Awake is called when the script instance is being loaded
    protected virtual void Awake()
    {
        if (!body) body = GetComponent<Rigidbody>();
    }

    // Use this for initialization
    protected virtual void Start()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation; 
    }
	
	// Update is called once per frame
	private void Update()
    {
        
	}
	#endregion

	#endregion
}
