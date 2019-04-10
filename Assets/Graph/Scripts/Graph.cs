using UnityEngine;

public class Graph : MonoBehaviour {
    public Transform pointPrefab;
    Transform[] points;
    
    [Range(10,10000)]
    public int resolution = 10;

    public GraphFunctionName functionN = 0;

    static GraphFunction[] functions = {
        sineFunction,multiSineFunction 
    };

    // Start is called before the first frame updaste
    void Awake(){
        float step = 2f / resolution;
        Vector3 scale = Vector3.one * step;
        Vector3 position;
		position.z = 0f;
        position.y = 0f;
        points = new Transform[resolution * resolution];
        for (int i = 0, x = 0,z = 0; i < points.Length; i++, x++) {
            if (x == resolution) {
				x = 0;
                z++;
			}
            Transform point = Instantiate(pointPrefab);
            position.x = (x + 0.5f) * step - 1f;
            position.z = (z + 0.5f) * step - 1f;
            // position.y = position.x * position.x * position.x;
            point.localPosition = position;
            point.localScale = scale;
            point.SetParent(transform,false);
            points[i] = point;
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

    static float sineFunction (float x, float z, float t) {
        return Mathf.Sin(Mathf.PI * ( x + t ));
    }

    static float multiSineFunction(float x, float z, float t){
        float y = Mathf.Sin(Mathf.PI * (x + t));
		y += Mathf.Sin(2f * Mathf.PI * (x + 2f * t)) / 2f;
        y *= 2f / 3f;
		return y;
    }
}
