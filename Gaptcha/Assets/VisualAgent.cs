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
    
    [Tooltip("할당하면 카메라를 자동으로 켭니다. 하나의 환경을 크게 보기 위해서 사용합니다.")]
    [SerializeField] Camera agentCameraSensorOn;
    
    Rigidbody rBody;
    // A single 'visual' agent instance that owns the camera sensor
    public static VisualAgent MainVisualAgent { get; private set; }
    // (No coroutine tracking field required) coroutines stop automatically on disable/destroy

    void Awake()
    {
        if (MainVisualAgent == null)
        {
            MainVisualAgent = this;
        }
    }
    void Start()
    {
        rBody = GetComponent<Rigidbody>();
        
        if (this == MainVisualAgent)
        {
            // Only the main visual agent starts the coroutine to enable its camera sensor
            if (agentCameraSensorOn)
            {
                StartCoroutine(EnableCameraSensorAfterDelay(0.1f));
            }
        }
        
    }

    private System.Collections.IEnumerator EnableCameraSensorAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        // Ensure only the current main visual agent enables its camera (prevents race conditions)
        if (this == MainVisualAgent && agentCameraSensorOn != null)
        {
            agentCameraSensorOn.enabled = true;
            GlobalDatas.DebugLog(() => "Agent.Start(): enabled agentCameraSensorOn");
        }
    }

    // No OnDisable needed: coroutines started from this MonoBehaviour are stopped automatically

    void OnDestroy()
    {
        if (MainVisualAgent == this)
        {
            MainVisualAgent = null;
            // find another VisualAgent in scene and make it the main visual agent
            var others = FindObjectsOfType<VisualAgent>();
            foreach (var a in others)
            {
                if (a != this)
                {
                    MainVisualAgent = a;
                    if (MainVisualAgent.agentCameraSensorOn)
                    {
                        MainVisualAgent.StartCoroutine(MainVisualAgent.EnableCameraSensorAfterDelay(0.1f));
                    }
                    break;
                }
            }
        }
    }

    public override void OnEpisodeBegin()
    {
        GlobalDatas.DebugLog(() => "Agent.OnEpisodeBegin()");
        Debug.Log("Agent.OnEpisodeBegin()");
        globalGameManager.PerformOnEpisodeBegin();
    }
    
    public override void CollectObservations(VectorSensor sensor)
    {
        GlobalDatas.DebugLog(() => "Agent.CollectObservations()");
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
        GlobalDatas.DebugLog(() => "Agent.OnActionReceived(): called");
        bool endEpisodeFlag = false;
        
        var discreteActions = actionBuffers.DiscreteActions;
        if (true)
        {
            // Debug.Log("Agent.OnActionReceived(): discreteActions[0]=" + discreteActions[0]);
            globalGameManager.actionIndex = discreteActions[0];
            GlobalDatas.DebugLog(() => "Agent.OnActionReceived(): actionIndex=" + globalGameManager.actionIndex);
        }

        if (episodeEnd)
        {
            episodeEnd = false;
            GlobalDatas.DebugLog(() => "Agent.OnActionReceived(): EndEpisode");
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
        GlobalDatas.DebugLog(() => "Agent.OnActionReceived(): end");
    }
    
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        GlobalDatas.DebugLog(() => "Agent.Heuristic()");
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
