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
    #endregion

    #region Methods

    #region Original Methods
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
            neuralNetwork.InputLayer.ForEach(i => Debug.Log(i.OutputSynapses.Count)); 
            return;
        }
        Debug.Log("Init NN");
        neuralNetwork = new NeuralNetwork(7, 12, muscles.Length, 2);
        string _data = neuralNetwork.ConvertNeuralNetworkIntoString();
        File.WriteAllText(Path.Combine(ResourcesPath, owner.name)+".txt", _data); 
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
        if(Input.GetKeyDown(KeyCode.A))
        {
            string _data = neuralNetwork.ConvertNeuralNetworkIntoString();
            Debug.Log("Test");
            File.WriteAllText(Path.Combine(ResourcesPath, owner.name) + ".txt", _data);
        }
	}
	#endregion

	#endregion
}

public struct BrainInputs
{
    public float DistanceFromFloor { get; set; }
    public float VelocityX { get; set; }
    public float VelocityY { get; set; }
    public float VelicityZ { get; set; }
    public float AngularVelocity { get; set; }
    public int GroundedPointsCounts { get; set; }
    public float Rotation { get; set; }
}