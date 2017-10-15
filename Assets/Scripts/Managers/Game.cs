using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Game : MonoBehaviour {

	public static Game Instance
    {
        get
        {
            if(_instance == null)
            {
                throw new NullReferenceException(@"Couldn't locate game manager. Ensure ""Game"" component is present in scene");
            }

            return _instance;
        }
    }
    private static Game _instance;

    private List<Action> Initializers = new List<Action>();

    private void Awake()
    {
        _instance = this;

        DontDestroyOnLoad(gameObject);

        Initialize();
    }
    private void Initialize()
    {
        foreach (Action action in Initializers)
        {
            action.Invoke();
        }
    }
}
