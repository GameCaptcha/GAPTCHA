using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class VisualAgent : Agent
{
    [SerializeField] GlobalGameManager globalGameManager;

    bool episodeEnd = false;
    
    Rigidbody rBody;
    void Start () {
        rBody = GetComponent<Rigidbody>();
    }

    public override void OnEpisodeBegin()
    {
        GlobalDatas.DebugLog("Agent.OnEpisodeBegin()");
        globalGameManager.PerformOnEpisodeBegin();
    }
    
    public override void CollectObservations(VectorSensor sensor)
    {
        // // Target and Agent positions
        // sensor.AddObservation(Target.localPosition);
        // sensor.AddObservation(this.transform.localPosition);
        //
        // // Agent velocity
        // sensor.AddObservation(rBody.linearVelocity.x);
        // sensor.AddObservation(rBody.linearVelocity.z);
    }

    public float forceMultiplier = 10;

    public void OnEndEpisode()
    {
        episodeEnd = true;
    }


    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        bool endEpisodeFlag = false;
        
        var discreteActions = actionBuffers.DiscreteActions;
        if (true)
        {
            // Debug.Log("Agent.OnActionReceived(): discreteActions[0]=" + discreteActions[0]);
            globalGameManager.actionIndex = discreteActions[0];
        }

        if (episodeEnd)
        {
            episodeEnd = false;
            GlobalDatas.DebugLog("Agent.OnActionReceived(): EndEpisode");
            globalGameManager.GameChange(allowSame: true);
            SetReward(-1f);
            endEpisodeFlag = true;  // will call EndEpisode() at the end of this method
        }
        else
        {
            SetReward(0.01f);
        }

        if (endEpisodeFlag)
        {
            EndEpisode();
            // Never put any code after EndEpisode()
            // Because EndEpisode() call OnEpisodeBegin() immediately.
        }
        // Again, never put any code after EndEpisode()
        // Because EndEpisode() call OnEpisodeBegin() immediately.
    }
    
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        GlobalDatas.DebugLog("Agent.Heuristic()");
        // var continuousActionsOut = actionsOut.ContinuousActions;
        var discreteActionsOut = actionsOut.DiscreteActions;
        
        int inputValue = InputValue.NONE;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            inputValue *= InputValue.LEFT;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            inputValue *= InputValue.RIGHT;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            inputValue *= InputValue.UP;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            inputValue *= InputValue.DOWN;
        }
        if (Input.GetKey(KeyCode.Space)) 
        {
            inputValue = InputValue.SPACE;
        }

        int index = GlobalDatas.ConvertInputValueToIndex(inputValue);


        discreteActionsOut[0] = index;
    }

}
