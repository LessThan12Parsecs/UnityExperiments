using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Game : PersistingObject
{
    public PersistingObject prefab;

    public KeyCode createKey = KeyCode.C;
    public KeyCode newGameKey = KeyCode.N;
    public KeyCode saveGameKey = KeyCode.S;
    public KeyCode loadGameKey = KeyCode.L;

    public PersistentStorage storage;

    private List<PersistingObject> objects;

    void Awake () {
        objects = new List<PersistingObject>();
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(createKey)) {
            CreateObject();
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

    void CreateObject () {
        PersistingObject o = Instantiate(prefab);
        Transform t = o.transform;
        t.localPosition = Random.onUnitSphere * 5f;
        t.localRotation = Random.rotation;
        t.localScale = Vector3.one * Random.Range(0.1f,1f);
        objects.Add(o);
    }

    void NewGame () {
        for (int i = 0; i < objects.Count; i++) {
			Destroy(objects[i].gameObject);
		}
        objects.Clear();
    }

    public override void Save (GameDataWriter writer) {
		writer.Write(objects.Count);
		for (int i = 0; i < objects.Count; i++) {
			objects[i].Save(writer);
		}
	}

    public override void Load (GameDataReader reader) {
		int count = reader.ReadInt();
		for (int i = 0; i < count; i++) {
			PersistingObject o = Instantiate(prefab);
			o.Load(reader);
			objects.Add(o);
		}
	}
}
