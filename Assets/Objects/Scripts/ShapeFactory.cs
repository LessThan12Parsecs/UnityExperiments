﻿using UnityEngine;

[CreateAssetMenu]
public class ShapeFactory : ScriptableObject
{
    [SerializeField]
    Material[] materials;
    
    [SerializeField]
    Shape[] prefabs;

    public Shape Get (int shapeId = 0, int materialId = 0) {        
        Shape instance = Instantiate(prefabs[shapeId]);
        instance.ShapeId = shapeId;
        instance.SetMaterial(materials[materialId], materialId);
        return instance;
    }

    public Shape GetRandom () {
        return Get(Random.Range(0,prefabs.Length), Random.Range(0,materials.Length));
    }

    
}
