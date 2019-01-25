using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager instance;

    [SerializeField]
    Block sampleBlock;

    [SerializeField]
    GameConfig config;

    [SerializeField]
    BlockEmiter leftEmiter;
    BlockEmiter rightEmiter;

    
    GameState gameState;
    GamePlayState playState;
    float timeStartPlay;
    float timeLastEmit;

    Block topBlock;
    List<Block> blockStack;
    List<Block> blockStopped;
    Block flyingBlock;

    [SerializeField]
    Vector3 fallingPoint;
    float currentHeight = 0;
    private void Awake()
    {
        if (instance == null)
        {
            instance = GetComponent<GameManager>();
            instance.Init();
        }

    }



    public GameManager()
    {
        Debug.Log("GameManager.Constructor");
        this.config = GameConfig.CreateDefaultGameConfig();
        
    }

    public void Init()
    {
        this.gameState = GameState.Playing;
        this.playState = GamePlayState.Prepare;
        this.blockStack = new List<Block>();
        this.blockStopped = new List<Block>();
        this.currentHeight = 0;
        this.topBlock = null;

        StartPlayGame();
    }

    public void StartPlayGame()
    {
        this.playState = GamePlayState.Prepare;
        
        ///Reset playing state
        this.timeStartPlay = Time.time;
        this.blockStack.Clear();
        this.blockStopped.Clear();
        this.topBlock = null;

        ///Setup first block
        Block genesisBlock = GetNewBlock();
        Vector3 pos = this.fallingPoint;
        pos.y = this.fallingPoint.y + genesisBlock.transform.localScale.y/2;
        genesisBlock.transform.position = pos;
        AddNewBlockToStack(genesisBlock);
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (this.gameState == GameState.Playing)
        {
            UpdatePlaying();
        }
	}
    void StartGamePlayingState()
    {
        this.playState = GamePlayState.Playing;
        this.timeLastEmit = 0;
        this.flyingBlock = null;
    }
    public Block GetNewBlock()
    {
        Block newBlock = Instantiate<Block>(sampleBlock, Vector3.zero, Quaternion.identity);
        newBlock.Init();
        return newBlock;
    }


    void EmitBlock()
    {
        UpdateEmiterPosition();

        ///Setup new block
        Block newBlock = GetNewBlock();
        Rigidbody blockBody = newBlock.GetComponent<Rigidbody>();
        blockBody.mass = 0.1f;
        blockBody.useGravity = false;
        blockBody.isKinematic = false;
        Vector3 newPos = this.leftEmiter.transform.position;

        newPos.y += newBlock.transform.localScale.y/2;
        newBlock.transform.position = newPos;
        Debug.Log(string.Format("EmitBlock: emitter={0}, newBlock={1}", newBlock.transform.position.ToString(), newBlock.transform.position));

        ///Track on ground
        newBlock.OnGroundListener.AddListener(HandleBlockOnGround);
        newBlock.OnBlockListener.AddListener(HandleBlockOnBlock);

        ///Fly bro
        blockBody.AddForce(this.leftEmiter.EmitDirection * config.EmitForce, ForceMode.Impulse);

        ///Record
        this.timeLastEmit = Time.time;
        flyingBlock = newBlock;
    }

    float newHeight = 0;
    void UpdateEmiterPosition()
    {
        this.newHeight = 0;
        foreach (Block block in this.blockStack)
        {
            this.newHeight += block.transform.localScale.y;
        }
        this.currentHeight = newHeight;

        Vector3 newPos = this.leftEmiter.transform.position;
        newPos.y = this.newHeight;
        this.leftEmiter.transform.position = newPos;
        Debug.Log(string.Format("UpdateEmiterPosition: height={0}, emiter={1}", currentHeight, leftEmiter.transform.position));
        
    }

    void UpdatePlaying()
    {
        if (this.playState == GamePlayState.Prepare)
        {
            if (Time.time - timeStartPlay > config.StatePlayingPrepareSecond)
            {
                Debug.Log("UpdatePlaying: Prepare done, go to Playing!");
                StartGamePlayingState();

            }
        }
        else if (this.playState == GamePlayState.Playing)
        {
            if (Time.time - this.timeLastEmit > config.BlockEmitIntervalSecond)
            {
                EmitBlock();
                UpdateEmiterPosition();
            }

            ///User stop flying block
            if (Input.GetAxis("Jump") == 1 && this.flyingBlock != null)
            {
                Debug.Log("InputAxis: " + Input.GetAxis("Jump"));
                Debug.Log("Stop!!!");
                StopBlock(this.flyingBlock);



                AddNewBlockToStack(this.flyingBlock);
                blockStopped.Add(this.flyingBlock);
                this.flyingBlock = null;


            }
        }
    }
    private void LateUpdate()
    {
        
        foreach (Block block in this.blockStopped)
        {
            Rigidbody body = block.GetComponent<Rigidbody>();
            block.OnGroundListener.AddListener(HandleBlockOnGround);
            ///body.isKinematic = true;
            Debug.Log(string.Format("LateUpdate: {0}", body.position));
        }
        blockStopped.Clear();
    }
    public void StopBlock(Block targetBlock)
    {
        Rigidbody body = targetBlock.GetComponent<Rigidbody>();

        body.velocity = Vector3.zero;
        body.angularVelocity = Vector3.zero;
        body.mass = 1000;
        body.useGravity = true;
        ///body.isKinematic = true;
    }
    public void AddNewBlockToStack(Block newBlock)
    {
        StopBlock(newBlock);

        this.blockStack.Add(newBlock);
        this.topBlock = newBlock;
        ///Calculate emitters position
        UpdateEmiterPosition();

    }
    public void HandleBlockOnGround(Block target)
    {
        Debug.Log("HandleBlockOnGround: ");
    }
    public void HandleBlockOnBlock(Block actor, Block target)
    {
        Debug.Log("HandleBlockOnBlock: ");
        if (target == topBlock)
        {
            target.SetStatic();
        }
    }

}
