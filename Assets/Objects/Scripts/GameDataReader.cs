using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameDataReader
{
	public int Version { get; }
    BinaryReader reader;

    public GameDataReader (BinaryReader reader, int version) {
		this.reader = reader;
		this.Version = version;
		
	}

    public float ReadFloat () {
		return reader.ReadSingle();
	}

	public int ReadInt () {
		return reader.ReadInt32();
	}

    public Quaternion ReadQuaternion () {
        Quaternion q;
		q.x = reader.ReadSingle();
		q.y = reader.ReadSingle();
		q.z = reader.ReadSingle();
		q.w = reader.ReadSingle();
        return q;
	}
	
	public Vector3 ReadVector3 () {
		Vector3 v;
        v.x = reader.ReadSingle();
		v.y = reader.ReadSingle();
		v.z = reader.ReadSingle();
        return v;
	}

	public Color ReadColor () {
		Color c;
		c.r = reader.ReadSingle();
		c.g = reader.ReadSingle();
		c.b = reader.ReadSingle();
		c.a = reader.ReadSingle();
		return c;
	}
}
