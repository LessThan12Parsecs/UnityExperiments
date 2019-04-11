using UnityEngine;

public class Graph : MonoBehaviour {
    public Transform pointPrefab;
    Transform[] points;
    
    [Range(10,10000)]
    public int resolution = 10;

    const float pi = Mathf.PI;

    public GraphFunctionName functionN = 0;

    static GraphFunction[] functions = {
        SineFunction, MultiSineFunction, Sine2DFunction, MultiSine2DFunction, Ripple
    };

    // Start is called before the first frame updaste
    void Awake(){
        float step = 2f / resolution;
        Vector3 scale = Vector3.one * step;
        Vector3 position;
		position.z = 0f;
        position.y = 0f;
        points = new Transform[resolution * resolution];
        for (int i = 0, z = 0; z < resolution; z++) {
              position.z = (z + 0.5f) * step - 1f;
            for (int x = 0; x < resolution; x++, i++) {
                Transform point = Instantiate(pointPrefab);
                position.x = (x + 0.5f) * step - 1f;
                // position.y = position.x * position.x * position.x;
                point.localPosition = position;
                point.localScale = scale;
                point.SetParent(transform,false);
                points[i] = point;
            }          
        }
    }

    void Update () {
        float t = Time.time;
        
        for (int i = 0; i < points.Length; i++){
            Transform point = points[i];
            Vector3 pos = point.localPosition;
            pos.y = functions[(int)functionN](pos.x, pos.z, t);
            point.localPosition = pos;
        }
    }

    static float SineFunction (float x, float z, float t) {
        return Mathf.Sin(pi * ( x + t ));
    }

    static float MultiSineFunction(float x, float z, float t) {
        float y = Mathf.Sin(pi * (x + t));
		y += Mathf.Sin(2f * pi * (x + 2f * t)) / 2f;
        y *= 2f / 3f;
		return y;
    }

    static float Sine2DFunction(float x, float z, float t) {
        float y = Mathf.Sin(pi * (x + t));
        y += Mathf.Sin(pi * (z + t));
        y *= 0.5f;
        return y;
    }

    static float MultiSine2DFunction (float x, float z, float t) {
		float y = 4f * Mathf.Sin(pi * (x + z + t * 0.5f));
		y += Mathf.Sin(pi * (x + t));
		y += Mathf.Sin(2f * pi * (z + 2f * t)) * 0.5f;
		y *= 1f / 5.5f;
		return y;
	}

    static float Ripple (float x, float z, float t) {
        float d = Mathf.Sqrt(x * x + z * z);
        float y = Mathf.Sin(pi * (4f * d + t));
        y /= 1f + 10f * d;
        return y;
    }
}
