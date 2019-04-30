using System; 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class InputNeuron : Neuron
{
    #region Field and properties
    public List<Synapse> OutputSynapses { get; private set; }
    #endregion

    #region Constructor
    public InputNeuron()
    {
        OutputSynapses = new List<Synapse>();
    }
    #endregion

    #region Methods
    /// <summary>
    /// Set the value of the neuron
    /// Used only when the Neuron is in the input Layer
    /// </summary>
    /// <param name="_value"></param>
    public void SetValue(float _value)
    {
        Value = _value;
    }
    #endregion 
}
