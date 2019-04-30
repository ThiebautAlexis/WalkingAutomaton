using System;
using System.Collections;
using System.Collections.Generic;

public static class NeuralMath  
{
    /* NeuralMath :
	 *
	 *	#####################
	 *	###### PURPOSE ######
	 *	#####################
	 *
	 *	[PURPOSE]
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

    #region Field and Properties
    private static System.Random random = new Random();
    private static int[] randomMultiplicators = new int[2] { -1, 1 };
    #endregion

    #region Methods
    public static float GetRandomValue()
    {
        //Get a random value between 0 and 1
        return (float)random.NextDouble();
    }

    public static float GetRandomValue(bool _allowNegative)
    {
        //Get a random value between -1 and 1
        int _rm = _allowNegative ? randomMultiplicators[UnityEngine.Random.Range(0, randomMultiplicators.Length)] : 1; 
        return (float)random.NextDouble() * _rm; 
    }

    public static float SigmoidSquish(float _input)
    {
        return 1 / (1 + (float)Math.Exp(-_input)); 
    }
	
    public static float SigmoidDerivative(float _input)
    {
        return _input * (1 - _input); 
    }
    #endregion

}
