﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachableBlockSeed : BlockSeed
{
	public AttachableBlockSeed()
	{
	}

	public AttachableBlockSeed(BlockSeed parent) : base(parent)
	{
	}
}

public class AttachableBlock : Block, IAttachable
{
	protected override void Awake()
	{
		base.Awake();
	}

	protected override void Start()
    {
		base.Start();
    }

	protected override void Update()
    {
		base.Update();
    }

	protected new AttachableBlockSeed Seed
	{
		get
		{
			var data = new AttachableBlockSeed(base.Seed);
			data.type = VehicleComponentType.AttachableBlock;

			return data;
		}
	}

	public void Setup(Block block, Vector3 direction)
	{
		base.Setup(block.Vehicle);

		Vector3 translation = direction.Multiplied(block.Bounds.extents + this.Bounds.extents);

		this.transform.position = block.Bounds.center + translation;
		Physics.SyncTransforms();

		foreach (BoxCollider box in this.LinkageBoxes)
		{
			Collider[] colliders = Physics.OverlapBox(box.bounds.center, box.bounds.extents, Quaternion.identity, Helper.BlockLayerMask);

			foreach (Collider collider in colliders)
			{
				if (collider.gameObject != this.gameObject)
				{
					var hitBlock = collider.gameObject.GetComponent<Block>();
					if (hitBlock != null)
					{
						InterConnect(hitBlock, this);
					}
				}
			}
		}
	}

	public override string ToJson()
	{
		// Not calling base class method is intentional

		return this.Seed.ToJson();
	}

	public VehicleComponent VehicleComponent
	{
		get => this;
	}
	
	public static void InterConnect(Block a, Block b)
	{
		a.Connect(b);
		b.Connect(a);
	}

	public static AttachableBlockSeed FromJson(string json)
	{
		return JsonUtility.FromJson<AttachableBlockSeed>(json);
	}
}
