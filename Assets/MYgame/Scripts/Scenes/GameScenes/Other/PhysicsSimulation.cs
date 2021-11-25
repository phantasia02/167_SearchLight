using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsSimulation : MonoBehaviour
{
    [ContextMenu("Run Simulation ")]
    public void RunSimulation()
    {
        Physics.autoSimulation = false;

        for (int i = 0; i < 1000; i++)
            Physics.Simulate(Time.deltaTime);

        Physics.autoSimulation = true;
    }
}
