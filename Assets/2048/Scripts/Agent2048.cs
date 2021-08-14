using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEditor;
using UnityEditor.Recorder;

public class Agent2048 : Agent
{
    public Manager gameManager; 
    
    Agent m_Agent;

    public int wins = 0;
    public int losses = 0;
    public int gamesPlayed = 0;

    private float prevScore = 0f;
    private float prevMax = 0f; 
    private int prevAction = -1;
    private int[,] prevGrid = new int[4,4];
    List<int> actionsToMask = new List<int>();
    private int decisionCount = 0; 

    public override void  Initialize()
    {
        m_Agent = GetComponent<Agent>();
    }

    void FixedUpdate()
    {
        if (gameManager.done && !gameManager.gameOver && gameManager.go == true) {
            m_Agent.RequestDecision();
		}

        if (gameManager.winner) {
            wins += 1;
            gamesPlayed += 1;
            m_Agent.AddReward(2048f);
            m_Agent.EndEpisode();
            gameManager.Restart();
            return;
        }

        if (gameManager.gameOver || decisionCount >= 100000) {
            losses += 1;
            gamesPlayed += 1;
            m_Agent.EndEpisode();
            gameManager.Restart();
            return;
        }
    }

    public override void OnEpisodeBegin() {
            prevScore = 0f;
            decisionCount = 0;
            prevMax = 0;
            recordGrid();
            actionsToMask = new List<int>();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        for (int i = 0; i < gameManager.grid.GetLength(0); i++)
            for (int j = 0; j < gameManager.grid.GetLength(1); j++)
                if(gameManager.grid[i,j] != null) {
                    //sensor.AddObservation(gameManager.grid[i, j].tileValue);
                    int v = (int)Math.Log(gameManager.grid[i, j].tileValue,2f);
                    sensor.AddObservation((1f/11f)*v);
                }
                else {
                    sensor.AddObservation(0f);
                }

        //string observation = string.Join(",", m_Agent.GetObservations());
        //Debug.Log(observation);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActionsOut = actionsOut.DiscreteActions;

        if(Input.GetKey(KeyCode.UpArrow))
            discreteActionsOut[0] = 0;
        else if(Input.GetKey(KeyCode.DownArrow))
            discreteActionsOut[0] = 1;
        else if(Input.GetKey(KeyCode.LeftArrow))
            discreteActionsOut[0] = 2;
        else if(Input.GetKey(KeyCode.RightArrow))
            discreteActionsOut[0] = 3;
        else 
            discreteActionsOut[0] = -1;
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        gameManager.Move(actionBuffers.DiscreteActions[0]);
        
        float max = FindMaxTileValue();
        if (prevMax < max) {
            // Reward for deterLocDeterValwNormMask, rndLocRndValwNormMask, and rndLocRndValwNormMaskColdStart
            // m_Agent.AddReward(max*(float)Math.Exp(-decisionCount/200f));
            // Reward for rndLocRndValwNormMaskSimpleReward
            m_Agent.AddReward(max);
            prevMax = max;
        }
        // Reward for deterLocDeterValwNormMask, rndLocRndValwNormMask, and rndLocRndValwNormMaskColdStart
        /*
        if (prevScore < gameManager.score) {
            m_Agent.AddReward((gameManager.score-prevScore)/100f);
            prevScore = gameManager.score; 
        }
        else {
            m_Agent.AddReward(-0.5f);
        }*/

        // if action lead to no change in the grid, mask it for next decision
        if (!gridChanged()) {
            if (!actionsToMask.Contains(actionBuffers.DiscreteActions[0]))
                actionsToMask.Add(actionBuffers.DiscreteActions[0]);
        } 
        else {
            actionsToMask = new List<int>();
        }

        recordGrid();
        decisionCount = decisionCount + 1;
    }

    public override void WriteDiscreteActionMask(IDiscreteActionMask actionMask)
    {
        foreach (int act in actionsToMask)
        {
            if (act != -1)
                actionMask.SetActionEnabled(0, act, false);
        }
    }

    private float FindMaxTileValue() {
        float max = 0f; 

        for (int i = 0; i < gameManager.grid.GetLength(0); i++)
            for (int j = 0; j < gameManager.grid.GetLength(1); j++)
                if(gameManager.grid[i,j] != null) 
                    if (gameManager.grid[i, j].tileValue > max)
                        max = gameManager.grid[i, j].tileValue;

        return max; 
    }

    private bool gridChanged() {
        for (int i = 0; i < gameManager.grid.GetLength(0); i++)
            for (int j = 0; j < gameManager.grid.GetLength(1); j++)
                if(gameManager.grid[i,j] == null && prevGrid[i,j] != 0)
                    return true;
                else if(gameManager.grid[i,j] != null && gameManager.grid[i,j].tileValue != prevGrid[i,j])
                    return true;
        return false;
    }

    private void recordGrid() {
        for (int i = 0; i < gameManager.grid.GetLength(0); i++)
            for (int j = 0; j < gameManager.grid.GetLength(1); j++)
                if(gameManager.grid[i,j] != null)
                    prevGrid[i,j] = gameManager.grid[i,j].tileValue;
                else
                    prevGrid[i,j] = 0;
    }

}
