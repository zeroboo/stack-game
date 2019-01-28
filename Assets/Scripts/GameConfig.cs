using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConfig{
    [SerializeField]
    float blockHeight;
    [SerializeField]
    float blockEmitIntervalSecond;
    [SerializeField]
    float statePlayingPrepareSecond;

    [SerializeField]
    float emitForce;
    [SerializeField]
    float emitForceMax;
    [SerializeField]
    float increaseForcePerPoint;

    float maxBlockTravelDistance;
    public GameConfig(float blockHeight, float blockEmitInterval, float statePlayingPrepareSecond
        , float emitForce, float increaseForcePerPoint, float emitForceMax
        , float maxBlockTravelDistance)
    {
        this.blockHeight = blockHeight;
        this.blockEmitIntervalSecond = blockEmitInterval;
        this.statePlayingPrepareSecond = statePlayingPrepareSecond;
        this.emitForce = emitForce;
        this.emitForceMax = emitForceMax;
        this.increaseForcePerPoint = increaseForcePerPoint;
        this.maxBlockTravelDistance = maxBlockTravelDistance;
    }

    public static GameConfig CreateDefaultGameConfig()
    {
        GameConfig config = new GameConfig(1
            , 1///blockEmitInterval
            , 2///statePlayingPrepareSecond
            , 3
            , 0.5f ///increaseForcePerPoint
            , 6
            , 15//maxBlockTravelDistance 
            );

        return config;
    }

    public float BlockHeight {
        get { return this.blockHeight;}
        set { this.blockHeight = value;}
    }

    public float BlockEmitIntervalSecond
    {
        get { return this.blockEmitIntervalSecond; }
        set { this.blockEmitIntervalSecond = value; }
    }

    public float StatePlayingPrepareSecond
    {
        get { return statePlayingPrepareSecond; }
        set { this.statePlayingPrepareSecond = value; }
    }
    public float EmitForce
    {
        get { return emitForce; }
        set { this.emitForce = value; }
    }
    public float EmitForceMax
    {
        get { return emitForceMax; }
        set { this.emitForceMax = value; }
    }
    public float IncreaseForcePerPoint
    {
        get { return increaseForcePerPoint; }
        set { this.increaseForcePerPoint = value; }
    }
    public float MaxBlockTravelDistance
    {
        get { return maxBlockTravelDistance; }
        set { this.maxBlockTravelDistance = value; }
    }
    
}
