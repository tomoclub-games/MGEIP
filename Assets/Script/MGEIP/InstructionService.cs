using System.Collections;
using System.Collections.Generic;
using MGEIP.GameData.ScenarioData;
using UnityEngine;

public class InstructionService : MonoBehaviour
{
    public static InstructionService Instance;

    [SerializeField] private InstructionDataContainer instructionDataContainer;

    public InstructionDataContainer InstructionDataContainer => instructionDataContainer;

    private void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
    }
}
