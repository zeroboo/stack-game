using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class BlockEvent : UnityEvent<Block> { }

[Serializable]
public class BlockOnBlockEvent : UnityEvent<Block, Block, Collision> { }

