using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Brain : MonoBehaviour
{
    /* Brain :
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
    public Automaton owner;
    public Muscle[] muscles;
    public NeuralNetwork neuralNetwork;

    public static string ResourcesPath
    {
        get { return Path.Combine(Application.dataPath, "Resources"); }
    }

    private BrainInputs brainInputs;

    private float fitness = 0;
    private float[] inputs = new float[7]; 
    private float[] outputs; 
    #endregion

    #region Methods

    #region Original Methods
    private void ApplyOutputs()
    {
        float _percent = 0;
        Muscle _muscle; 
        for (int i = 0; i < outputs.Length; i++)
        {
            _muscle = muscles[0]; 
            // Remove .5f to shift the output between -.5f and .5f then multiply by two to set it between -1 and 1
            _percent = 2 * (outputs[i] - .5f);
            if (_percent < 0)
                _muscle.MuscleAction = MuscleAction.Contraction;
            else
                _muscle.MuscleAction = MuscleAction.Expansion;

            _muscle.UpdateCurrentForce(_percent); 
        }
    }

    /// <summary>
    /// Initialize the neural network
    /// If there is no Neural Network create a new one 
    /// else load the existing neural network
    /// </summary>
    private void InitNeuralNetwork()
    {
        if (!Directory.Exists(ResourcesPath))
        {
            Debug.Log("Create Directory");
            Directory.CreateDirectory(ResourcesPath);
        }
        TextAsset _datas = Resources.Load(owner.name, typeof(TextAsset)) as TextAsset;
        if (_datas != null)
        {
            Debug.Log("Load NN");
            neuralNetwork = new NeuralNetwork(_datas.text);
            return;
        }
        Debug.Log("Init NN");
        neuralNetwork = new NeuralNetwork(7, 12, muscles.Length, 2);
        string _data = neuralNetwork.ConvertNeuralNetworkIntoString();
        File.WriteAllText(Path.Combine(ResourcesPath, owner.name)+".txt", _data); 
    }

    private void FeedNeuralNetwork()
    {
        brainInputs = owner.UpdateBrainInputs();
        inputs[0] = brainInputs.DistanceFromFloor; 
        inputs[1] = brainInputs.VelocityX;
        inputs[2] = brainInputs.VelocityY;
        inputs[3] = brainInputs.VelocityZ;
        inputs[4] = brainInputs.AngularVelocity;
        inputs[5] = brainInputs.GroundedPointsCounts;
        inputs[6] = brainInputs.Rotation;

        outputs = neuralNetwork.Compute(inputs);

        ApplyOutputs(); 
    }
    #endregion

    #region Unity Methods
    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        InitNeuralNetwork(); 
    }

	// Use this for initialization
    private void Start()
    {
		
    }
	
	// Update is called once per frame
	private void Update()
    {
        FeedNeuralNetwork(); 
    }
	#endregion

	#endregion
}

public struct BrainInputs
{
    public float DistanceFromFloor { get; set; }
    public float VelocityX { get; set; }
    public float VelocityY { get; set; }
    public float VelocityZ { get; set; }
    public float AngularVelocity { get; set; }
    public int GroundedPointsCounts { get; set; }
    public float Rotation { get; set; }
}