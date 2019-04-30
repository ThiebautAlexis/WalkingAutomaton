using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq; 
using UnityEngine;

[Serializable]
public abstract class Neuron 
{
	/* Neuron :
	 *
	 *	#####################
	 *	###### PURPOSE ######
	 *	#####################
	 *
	 *	[Contains all values and Behaviour for the neurons of a neural Network]
     *	    - Value of the neuron -> Output value of the neuron
	 *
	 *	#####################
	 *	### MODIFICATIONS ###
	 *	#####################
	 *
	 *	Date :			[06/03/2019]
	 *	Author :		[Alexis Thiébaut]
	 *
	 *	Changes : 
	 *
	 *	[Modification of the class]
     *	    - Set the Neuron class as abstract and create inheritance for other type of neurons
     *	    - Keep the Value variable and the OutputSynapses list
	 *
	 *	-----------------------------------
	*/

	#region Fields / Properties
    public float Value { get; protected set; }
    public float LocalGradient { get; protected set; }
    #endregion
}

[Serializable]
public class Synapse
{
    #region Fields and properties
    public Neuron InputNeuron { get; set; }
    public Neuron OutPutNeuron { get; set; }
    public float Weight { get; set; }
    public float WeightDelta { get; set; }
    #endregion

    #region Constructor
    public Synapse(Neuron _input, Neuron _output)
    {
        InputNeuron = _input;
        OutPutNeuron = _output;
        Weight = NeuralMath.GetRandomValue(true);
        WeightDelta = 0; 
    }
    #endregion

}
