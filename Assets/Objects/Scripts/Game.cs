using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Game : PersistingObject
{
    public ShapeFactory shapeFactory;
    public KeyCode createKey = KeyCode.C;
    public KeyCode newGameKey = KeyCode.N;
    public KeyCode saveGameKey = KeyCode.S;
    public KeyCode loadGameKey = KeyCode.L;

    public PersistentStorage storage;

    private List<Shape> shapes;

    const int saveVersion = 1;

    void Awake () {
        shapes = new List<Shape>();
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(createKey)) {
            CreateShape();
        }
        else if (Input.GetKeyDown(newGameKey)) {
            NewGame();
        }
        else if (Input.GetKeyDown(saveGameKey)) {
            storage.Save(this);
        }
        else if (Input.GetKeyDown(loadGameKey)) {
            NewGame();
            storage.Load(this);
        }
    }

    void CreateShape () {
        Shape instance = shapeFactory.GetRandom();
        Transform t = instance.transform;
        t.localPosition = Random.onUnitSphere * 5f;
        t.localRotation = Random.rotation;
        t.localScale = Vector3.one * Random.Range(0.1f,1f);
        shapes.Add(instance);
    }

    void NewGame () {
        for (int i = 0; i < shapes.Count; i++) {
			Destroy(shapes[i].gameObject);
		}
        shapes.Clear();
    }

    public override void Save (GameDataWriter writer) {
		writer.Write(-saveVersion);
		writer.Write(shapes.Count);
		for (int i = 0; i < shapes.Count; i++) {
            writer.Write(shapes[i].ShapeId);
			shapes[i].Save(writer);
		}
	}

    public override void Load (GameDataReader reader) {
		int version = -reader.ReadInt();
        if (version > saveVersion) {
            Debug.LogError("SaveFile version is higher than current game version " + version);
            return;
        }
		int count = version <= 0 ? -version : reader.ReadInt();
		for (int i = 0; i < count; i++) {
            int shapeId = version > 0 ? reader.ReadInt() : 0;
			Shape instance = shapeFactory.Get(shapeId);
			instance.Load(reader);
			shapes.Add(instance);
		}
	}
}
