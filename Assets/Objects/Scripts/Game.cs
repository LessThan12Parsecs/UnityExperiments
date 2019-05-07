using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Game : MonoBehaviour
{
    public Transform prefab;

    public KeyCode createKey = KeyCode.C;
    public KeyCode newGameKey = KeyCode.N;
    public KeyCode saveGameKey = KeyCode.S;
    public KeyCode loadGameKey = KeyCode.L;

    string savePath;

    private List<Transform> objects;

    void Awake () {
        savePath = Path.Combine(Application.persistentDataPath,"saveFile");
        objects = new List<Transform>();
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
            SaveGame();
        }
        else if (Input.GetKeyDown(loadGameKey)) {
            LoadGame();
        }
    }

    void CreateObject () {
        Transform t = Instantiate(prefab);
        t.localPosition = Random.onUnitSphere * 5f;
        t.localRotation = Random.rotation;
        t.localScale = Vector3.one * Random.Range(0.1f,1f);
        objects.Add(t);
    }

    void NewGame () {
        for (int i = 0; i < objects.Count; i++) {
			Destroy(objects[i].gameObject);
		}
        objects.Clear();
    }

    void SaveGame () {
        using (
            var writer = new BinaryWriter(File.Open(savePath, FileMode.Create))
        ) {
            writer.Write(objects.Count);
            for (int i = 0; i < objects.Count; i++) {
                Transform t = objects[i];
                writer.Write(t.localPosition.x);
                writer.Write(t.localPosition.y);
                writer.Write(t.localPosition.z);
            }
        }
    }
    
    void LoadGame (){
        NewGame();
        using (
            var reader = new BinaryReader(File.Open(savePath, FileMode.Open))
        ) {
            int count = reader.ReadInt32();
            for (int i = 0; i < count; i++) {
                Vector3 p;
                p.x = reader.ReadSingle();
                p.y = reader.ReadSingle();
                p.z = reader.ReadSingle();
                Transform t = Instantiate(prefab);
                t.localPosition = p;
                objects.Add(t);
            }
        }
    }

}
