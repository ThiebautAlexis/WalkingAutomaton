using System; 
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class HiddenNeuron : Neuron
{
    #region Fields and Properties
    public List<Synapse> InputSynapses { get; private set; }
    public List<Synapse> OutputSynapses { get; private set; }
    public float Bias { get; private set; }
    #endregion

    #region Constructor
    public HiddenNeuron(List<InputNeuron> _previousLayer)
    {
        Bias = 0; 
        InputSynapses = new List<Synapse>();
        OutputSynapses = new List<Synapse>();
        for (int i = 0; i < _previousLayer.Count; i++)
        {
            Synapse _s = new Synapse(_previousLayer[i], this);
            _previousLayer[i].OutputSynapses.Add(_s); 
            InputSynapses.Add(_s); 
        }
    }
    public HiddenNeuron(List<HiddenNeuron> _previousLayer)
    {
        Bias = 0;
        InputSynapses = new List<Synapse>();
        OutputSynapses = new List<Synapse>();
        for (int i = 0; i < _previousLayer.Count; i++)
        {
            Synapse _s = new Synapse(_previousLayer[i], this);
            _previousLayer[i].OutputSynapses.Add(_s);
            InputSynapses.Add(_s);
        }
    }
    #endregion

    #region Methods
    /// <summary>
    /// Calculate the value of the neuron based on the weights and values of the Previous Layer
    /// VALUE = SIGMOIDSQUISH(WEIGHT * VALUE) of every previous neuron + bias 
    /// </summary>
    /// <returns></returns>
    public float CalculateValue()
    {
        return Value = NeuralMath.SigmoidSquish(InputSynapses.Sum(s => s.Weight * s.InputNeuron.Value) + Bias);
    }

    /// <summary>
    /// Update the weights of the InputSynapses in order to gain in precision
    /// </summary>
    /// <param name="_learnRate">learning rate of the Neural Network</param>
    public IEnumerator UpdateSynapses(float _learnRate)
    {
        LocalGradient = NeuralMath.SigmoidDerivative(Value) * (OutputSynapses.Sum(s => (s.OutPutNeuron.LocalGradient * s.Weight)));
        foreach (Synapse synapse in InputSynapses)
        {
            synapse.WeightDelta = synapse.Weight - (_learnRate * (synapse.InputNeuron.Value * LocalGradient));
            yield return new WaitForEndOfFrame();
        }
    }

    public void SetValues(float _bias, float[] _inputWeight)
    {
        Bias = _bias;
        for (int i = 0; i < InputSynapses.Count; i++)
        {
            InputSynapses[i].Weight = _inputWeight[i];
        }
    }
    #endregion 
}
