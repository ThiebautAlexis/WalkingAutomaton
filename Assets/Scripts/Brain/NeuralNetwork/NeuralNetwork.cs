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
        // Initialise the layers list
        InputLayer = new List<InputNeuron>();
        HiddenLayers = new List<List<HiddenNeuron>>();
        OutputLayer = new List<OutputNeuron>();

        //Initialise Input layer        
        for (int i = 0; i < _inputLayerSize; i++)
        {
            InputLayer.Add(new InputNeuron()); 
        }
        //Initialise Hidden Layers

        List<HiddenNeuron> _previousLayer = null; 
        for (int i = 0; i < _numberHiddenLayers; i++)
        {
            if(i == 0)
            {
                HiddenLayers.Add(new List<HiddenNeuron>());
                for (int j = 0; j < _hiddenLayerSize; j++)
                {
                    HiddenLayers[0].Add(new HiddenNeuron(InputLayer));
                }
            }
            else
            {
                _previousLayer = HiddenLayers.Last();
                HiddenLayers.Add(new List<HiddenNeuron>());
                for (int j = 0; j < _hiddenLayerSize; j++)
                {
                    HiddenLayers[i].Add(new HiddenNeuron(_previousLayer));
                }
            }
        }
        //Initialise Output Layer
        List<HiddenNeuron> _lastHiddenLayer = HiddenLayers.Last();
        for (int i = 0; i < _outputLayerSize; i++)
        {
            OutputLayer.Add(new OutputNeuron(_lastHiddenLayer)); 
        }
    }

    public NeuralNetwork(string _datas)
    {

        InputLayer = new List<InputNeuron>();
        HiddenLayers = new List<List<HiddenNeuron>>();
        OutputLayer = new List<OutputNeuron>();

        string[] _splitedDatas = _datas.Split('\n');
        string[] _subDatas = _splitedDatas[0].Split('#');
        int _inputSize = int.Parse(_subDatas[0]);
        int _hiddenLayerCount = int.Parse(_subDatas[1]);
        int _outputSize = int.Parse(_subDatas[2]);
        for (int i = 0; i < _inputSize; i++)
        {
            InputLayer.Add(new InputNeuron());
        }
        List<HiddenNeuron> _previousLayer = null;
        float[] _weights = null; 
        for (int i = 1; i < _splitedDatas.Length -1 ; i++)
        {
            _subDatas = _splitedDatas[i].Split('#');
            if (i == 1)
            {
                HiddenLayers.Add(new List<HiddenNeuron>());
                for (int j = 0; j < _subDatas.Length; j++)
                {
                    _weights = _subDatas[j].Split('|').ToList().Select(w => float.Parse(w)).ToArray(); 
                    HiddenLayers[0].Add(new HiddenNeuron(InputLayer, _weights));
                }
            }
            else
            {
                _previousLayer = HiddenLayers.Last();
                HiddenLayers.Add(new List<HiddenNeuron>());
                for (int j = 0; j < _subDatas.Length; j++)
                {
                    _weights = _subDatas[j].Split('|').ToList().Select(w => float.Parse(w)).ToArray();
                    HiddenLayers[i-1].Add(new HiddenNeuron(_previousLayer, _weights));
                }
            }
        }
        //Initialise Output Layer
        List<HiddenNeuron> _lastHiddenLayer = HiddenLayers.Last();
        _subDatas = _splitedDatas[_splitedDatas.Length-1].Split('#');
        for (int i = 0; i < _subDatas.Length; i++)
        {
            _weights = _subDatas[i].Split('|').ToList().Select(w => float.Parse(w)).ToArray();
            OutputLayer.Add(new OutputNeuron(_lastHiddenLayer, _weights));
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

    public string ConvertNeuralNetworkIntoString()
    {
        string _infos = string.Empty;
        _infos += $"{InputLayer.Count}#{HiddenLayers.Count}#{OutputLayer.Count}\n";
        for (int i = 0; i < HiddenLayers.Count; i++)
        {
            for (int j = 0; j < HiddenLayers[i].Count; j++)
            {
                for (int k = 0; k < HiddenLayers[i][j].InputSynapses.Count; k++)
                {
                    _infos += k < HiddenLayers[i][j].InputSynapses.Count -1 ? $"{HiddenLayers[i][j].InputSynapses[k].Weight}|" : $"{HiddenLayers[i][j].InputSynapses[k].Weight}";
                }
                if(j < HiddenLayers[i].Count - 1 )_infos += '#'; 
            }
            _infos += '\n'; 
        }
        for (int i = 0; i < OutputLayer.Count; i++)
        {
            for (int j = 0; j < OutputLayer[i].InputSynapses.Count; j++)
            {
                _infos += j < OutputLayer[i].InputSynapses.Count - 1 ? $"{OutputLayer[i].InputSynapses[j].Weight}|" : $"{OutputLayer[i].InputSynapses[j].Weight}";
            }
            if (i < OutputLayer.Count - 1) _infos += '#';
        }
        return _infos; 
    }
    #endregion

}