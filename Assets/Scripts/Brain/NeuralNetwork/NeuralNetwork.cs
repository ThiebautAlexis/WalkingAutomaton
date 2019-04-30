using System; 
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

[Serializable]
public class NeuralNetwork
{
    #region Fields and properties
    [SerializeField, Range(0, 1)] private float learnRate = .5f;
    public List<InputNeuron> InputLayer { get; set; }
    public List<List<HiddenNeuron>> HiddenLayers { get; set; }
    public List<OutputNeuron> OutputLayer { get; set; }

    private float[] targets; 
    #endregion

    #region Constructor
    public NeuralNetwork(int _inputLayerSize, int _hiddenLayerSize, int _outputLayerSize, int _numberHiddenLayers)
    {
        // Initialise the Hidden layer list
        HiddenLayers = new List<List<HiddenNeuron>>(); 
        
        //Initialise Input layer
        InputLayer = new List<InputNeuron>();
        for (int i = 0; i < _inputLayerSize; i++)
        {
            InputLayer.Add(new InputNeuron()); 
        }
        //Initialise Hidden Layers
        for (int i = 0; i < _numberHiddenLayers; i++)
        {
            HiddenLayers.Add(new List<HiddenNeuron>());
            if (i == 0)
            {
                List<InputNeuron> _previousLayer = InputLayer;
                for (int j = 0; j < _hiddenLayerSize; j++)
                {
                    HiddenLayers[i].Add(new HiddenNeuron(_previousLayer));
                }
            }
            else
            {
                List<HiddenNeuron> _previousLayer = HiddenLayers.Last(); 
                for (int j = 0; j < _hiddenLayerSize; j++)
                {
                    HiddenLayers[i].Add(new HiddenNeuron(_previousLayer));
                }
            }

        }
        OutputLayer = new List<OutputNeuron>();
        List<HiddenNeuron> _lastHiddenLayer = HiddenLayers.Last();
        for (int i = 0; i < _outputLayerSize; i++)
        {
            OutputLayer.Add(new OutputNeuron(_lastHiddenLayer)); 
        }
    }
    #endregion

    #region Methods

    public float[] Compute(float[] inputs)
    {
        ForwardPropagate(inputs);
        return OutputLayer.Select(a => a.Value).ToArray();
    }


    private void ForwardPropagate(float[] _inputs)
    {
        var i = 0;
        foreach (InputNeuron neuron in InputLayer)
        {
            neuron.SetValue(_inputs[i]);
            i++; 
        }
        foreach (List<HiddenNeuron> neurons in HiddenLayers)
        {
            neurons.ForEach(a =>a.CalculateValue());
        }
        OutputLayer.ForEach(a => a.CalculateValue());
    }

    public IEnumerator BackPropagate(float[] _targets, MonoBehaviour _caster)
    {
        targets = _targets; 
        float _error = 0;
        for (int i = 0; i < OutputLayer.Count; i++)
        { 
           _error += OutputLayer[i].CalculateError(_targets[i]); 
        }
        Debug.Log("E = " + _error); 

        Coroutine _updateOutputCoroutine = _caster.StartCoroutine(UpdateOutPutLayer(_caster));
        yield return _updateOutputCoroutine;

        yield return new WaitForEndOfFrame();

        Coroutine _updatHiddenLayerCoroutine = _caster.StartCoroutine(UpdateHiddenLayers(_caster));
        yield return _updateOutputCoroutine;

        yield return new WaitForEndOfFrame();

        Coroutine _updateWeightsCoroutine = _caster.StartCoroutine(UpdateWeights());
        yield return _updateOutputCoroutine;
    }

    private IEnumerator UpdateOutPutLayer(MonoBehaviour _caster)
    {
        Coroutine _c; 
        for (int i = 0; i < OutputLayer.Count; i++)
        {
            _c  = _caster.StartCoroutine(OutputLayer[i].UpdateSynapses(learnRate, targets[i]));
            yield return _c; 
        }
    }

    private IEnumerator UpdateHiddenLayers(MonoBehaviour _caster)
    {
        Coroutine _c; 
        for (int i = HiddenLayers.Count - 1; i >= 0; i--)
        {
            List<HiddenNeuron> _layer = HiddenLayers[i];
            foreach (HiddenNeuron neuron in _layer)
            {
                _c = _caster.StartCoroutine(neuron.UpdateSynapses(learnRate));
                yield return _c;
            }
        }
    }

    private IEnumerator UpdateWeights()
    {
        OutputLayer.ForEach(n => n.InputSynapses.ForEach(s => s.Weight = s.WeightDelta));
        HiddenLayers.ForEach(l => l.ForEach(n => n.InputSynapses.ForEach(s => s.Weight = s.WeightDelta)));
        yield return new WaitForSeconds(1); 
        Debug.Log("End BP");
    }
    #endregion

}