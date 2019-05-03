using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneticManager : MonoBehaviour 
{
    /* GeneticManager :
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
    public static GeneticManager Instance;

    [SerializeField] private GameObject instanciedAutomaton;

    [SerializeField, Range(10, 200)] private int numberOfInstances = 10;
    //public int NumberOfInstances { get { return numberOfInstances; } }

    [SerializeField, Range(1, 500)] private int distanceToTravel = 10;
    public int DistanceToTravel { get { return distanceToTravel; } }

    [SerializeField, Range(1, 10)] private float mutationRate = 10; 
    public float MutationRange { get { return mutationRate / 100; } }

    public List<Automaton> Population = new List<Automaton>(); 
	#endregion

	#region Methods

	#region Original Methods
    private void InitPopulation()
    {
        Automaton _automaton; 
        for (int i = 0; i < numberOfInstances; i++)
        {
            _automaton = (Instantiate(instanciedAutomaton, Vector3.zero, Quaternion.identity) as GameObject).GetComponent<Automaton>();
            _automaton.gameObject.name = $"Automaton{i}"; 
            Population.Add(_automaton); 
        }
    }
	#endregion

	#region Unity Methods
	// Awake is called when the script instance is being loaded
    private void Awake()
    {
        if (!Instance)
            Instance = this;
        else Destroy(this); 
    }

	// Use this for initialization
    private void Start()
    {
        InitPopulation(); 
    }
	
	// Update is called once per frame
	private void Update()
    {
        
	}
	#endregion

	#endregion
}
