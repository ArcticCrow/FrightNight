﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct RegionFurnitures {
	public string name;
	public GameObject prefab;
	public List<Vector3> spawnLocations;

	public RegionFurnitures(string name)
	{
		this.name = name;
		this.prefab = null;
		spawnLocations = new List<Vector3>();
	}

	public RegionFurnitures(GameObject prefab)
	{
		this.name = prefab.name;
		this.prefab = prefab;
		spawnLocations = new List<Vector3>();
	}

	public void AddPosition(Vector3 pos)
	{
		spawnLocations.Add(pos);
	}
}
