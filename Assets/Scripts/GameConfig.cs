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
    public GameConfig(float blockHeight, float blockEmitInterval, float statePlayingPrepareSecond, float emitForce)
    {
        this.blockHeight = blockHeight;
        this.blockEmitIntervalSecond = blockEmitInterval;
        this.statePlayingPrepareSecond = statePlayingPrepareSecond;
        this.emitForce = emitForce;
    }

    public static GameConfig CreateDefaultGameConfig()
    {
        GameConfig config = new GameConfig(1
            , 1
            , 2///statePlayingPrepareSecond
            , 2
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
}
