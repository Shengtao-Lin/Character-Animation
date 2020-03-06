using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PrismManager : MonoBehaviour
{
    public int prismCount = 10;
    public float prismRegionRadiusXZ = 5;
    public float prismRegionRadiusY = 5;
    public float maxPrismScaleXZ = 5;
    public float maxPrismScaleY = 5;
    public GameObject regularPrismPrefab;
    public GameObject irregularPrismPrefab;

    private List<Prism> prisms = new List<Prism>();
    private List<GameObject> prismObjects = new List<GameObject>();
    private GameObject prismParent;
    private Dictionary<Prism,bool> prismColliding = new Dictionary<Prism, bool>();

    private const float UPDATE_RATE = 0.5f;

    #region Unity Functions

    void Start()
    {
        Random.InitState(0);    //10 for no collision

        prismParent = GameObject.Find("Prisms");
        for (int i = 0; i < prismCount; i++)
        {
            var randPointCount = Mathf.RoundToInt(3 + Random.value * 7);
            var randYRot = Random.value * 360;
            var randScale = new Vector3((Random.value - 0.5f) * 2 * maxPrismScaleXZ, (Random.value - 0.5f) * 2 * maxPrismScaleY, (Random.value - 0.5f) * 2 * maxPrismScaleXZ);
            var randPos = new Vector3((Random.value - 0.5f) * 2 * prismRegionRadiusXZ, (Random.value - 0.5f) * 2 * prismRegionRadiusY, (Random.value - 0.5f) * 2 * prismRegionRadiusXZ);

            GameObject prism = null;
            Prism prismScript = null;
            if (Random.value < 0.5f)
            {
                prism = Instantiate(regularPrismPrefab, randPos, Quaternion.Euler(0, randYRot, 0));
                prismScript = prism.GetComponent<RegularPrism>();
            }
            else
            {
                prism = Instantiate(irregularPrismPrefab, randPos, Quaternion.Euler(0, randYRot, 0));
                prismScript = prism.GetComponent<IrregularPrism>();
            }
            prism.name = "Prism " + i;
            prism.transform.localScale = randScale;
            prism.transform.parent = prismParent.transform;
            prismScript.pointCount = randPointCount;
            prismScript.prismObject = prism;

            prisms.Add(prismScript);
            prismObjects.Add(prism);
            prismColliding.Add(prismScript, false);
        }

        StartCoroutine(Run());
    }
    
    void Update()
    {
        #region Visualization

        DrawPrismRegion();
        DrawPrismWireFrames();

#if UNITY_EDITOR
        if (Application.isFocused)
        {
            UnityEditor.SceneView.FocusWindowIfItsOpen(typeof(UnityEditor.SceneView));
        }
#endif

        #endregion
    }

    IEnumerator Run()
    {
        yield return null;

        while (true)
        {
            foreach (var prism in prisms)
            {
                prismColliding[prism] = false;
            }

            foreach (var collision in PotentialCollisions())
            {
                if (CheckCollision(collision))
                {
                    prismColliding[collision.a] = true;
                    prismColliding[collision.b] = true;

                    ResolveCollision(collision);
                }
            }

            yield return new WaitForSeconds(UPDATE_RATE);
        }
    }

    #endregion

    #region Incomplete Functions

    private IEnumerable<PrismCollision> PotentialCollisions()
    {
        List<Tuple<float,float>> tuple_list = new List<Tuple<float,float>>();
        float min_x = float.MaxValue;
        float max_x = float.MinValue;

        // Compute the min_x and max_x for one prism
        /*
        Prism p = prisms[1];
        Tuple<float,float> t = new Tuple<float,float>(0f,0.0f);
        // float min_x = float.MaxValue;
        // float max_x = float.MinValue;
        for(int i=0;i<p.points.Length;i++){
            //Debug.Log("point " + i + p.points[i].ToString());
            Vector3 point = p.points[i];
            min_x = Mathf.Min(min_x,point.x);
            max_x = Mathf.Max(max_x,point.x);
            t.Item1 = min_x;
            t.Item2 = max_x;
        }
        tuple_list.Add(t);
        

        
        Debug.Log("t min_x = " +t.Item1 +" max_x = "+t.Item2);
        //check if this shit is added
        Debug.Log("in tuplelist min_x = " +tuple_list[0].Item1 +" max_x = "+tuple_list[0].Item2);
        */
        Hashtable table = new Hashtable();
        
        // for each prism, find the pair of x with max difference for x-axis
        for (int i = 0; i < prisms.Count; i++) {
            Prism pri = prisms[i];
            Tuple<float,float> t = new Tuple<float,float>(0f,0f);
            for(int j=0;j<pri.points.Length;j++){
                Vector3 point = pri.points[j];
                min_x = Mathf.Min(min_x,point.x);
                max_x = Mathf.Max(max_x,point.x);
                t.Item1 = min_x;
                t.Item2 = max_x;
            }
            table.Add(pri,t);
            tuple_list.Add(t);
            min_x = float.MaxValue;
            max_x = float.MinValue;
        }


        // this line can sort the list
        //tuple_list = tuple_list.OrderByDescending(t=>t.Item2).ToList();

        //check tupleList
        tuple_list = tuple_list.OrderByDescending(t=>t.Item2).ToList();
        for(int i=0;i<tuple_list.Count;i++){
            Tuple<float,float> tuple = tuple_list[i];
            Debug.Log("in tuple " + i + " min_x = " +tuple_list[i].Item1 +" max_x = "+tuple_list[i].Item2);
        }
        //above part shit can use!!!!!
        
        

        
        // Debug.Log("number of pts = " + p.pointCount);
        
        for (int i = 0; i < prisms.Count; i++) {
            for (int j = i + 1; j < prisms.Count; j++) {
                var checkPrisms = new PrismCollision();
                checkPrisms.a = prisms[i];
                checkPrisms.b = prisms[j];

                yield return checkPrisms;
            }
        }
        
        // using dictionary to strore each x values and mathc them to the proism it belonds
        // then traverse the sorted ararry and use hash set to keep updating the potential collision pair
        

        yield break;
        
    }

    private bool CheckCollision(PrismCollision collision)
    {
        var prismA = collision.a;
        var prismB = collision.b;

        
        collision.penetrationDepthVectorAB = Vector3.zero;

        return true;
    }
    
    #endregion

    #region Private Functions
    
    private void ResolveCollision(PrismCollision collision)
    {
        var prismObjA = collision.a.prismObject;
        var prismObjB = collision.b.prismObject;

        var pushA = -collision.penetrationDepthVectorAB / 2;
        var pushB = collision.penetrationDepthVectorAB / 2;

        prismObjA.transform.position += pushA;
        prismObjB.transform.position += pushB;

        Debug.DrawLine(prismObjA.transform.position, prismObjA.transform.position + collision.penetrationDepthVectorAB, Color.cyan, UPDATE_RATE);
    }
    
    #endregion

    #region Visualization Functions

    private void DrawPrismRegion()
    {
        var points = new Vector3[] { new Vector3(1, 0, 1), new Vector3(1, 0, -1), new Vector3(-1, 0, -1), new Vector3(-1, 0, 1) }.Select(p => p * prismRegionRadiusXZ).ToArray();
        
        var yMin = -prismRegionRadiusY;
        var yMax = prismRegionRadiusY;

        var wireFrameColor = Color.yellow;

        foreach (var point in points)
        {
            Debug.DrawLine(point + Vector3.up * yMin, point + Vector3.up * yMax, wireFrameColor);
        }

        for (int i = 0; i < points.Length; i++)
        {
            Debug.DrawLine(points[i] + Vector3.up * yMin, points[(i + 1) % points.Length] + Vector3.up * yMin, wireFrameColor);
            Debug.DrawLine(points[i] + Vector3.up * yMax, points[(i + 1) % points.Length] + Vector3.up * yMax, wireFrameColor);
        }
    }

    private void DrawPrismWireFrames()
    {
        for (int prismIndex = 0; prismIndex < prisms.Count; prismIndex++)
        {
            var prism = prisms[prismIndex];
            var prismTransform = prismObjects[prismIndex].transform;

            var yMin = prism.midY - prism.height / 2 * prismTransform.localScale.y;
            var yMax = prism.midY + prism.height / 2 * prismTransform.localScale.y;

            var wireFrameColor = prismColliding[prisms[prismIndex]] ? Color.red : Color.green;

            foreach (var point in prism.points)
            {
                Debug.DrawLine(point + Vector3.up * yMin, point + Vector3.up * yMax, wireFrameColor);
            }

            for (int i = 0; i < prism.pointCount; i++)
            {
                Debug.DrawLine(prism.points[i] + Vector3.up * yMin, prism.points[(i + 1) % prism.pointCount] + Vector3.up * yMin, wireFrameColor);
                Debug.DrawLine(prism.points[i] + Vector3.up * yMax, prism.points[(i + 1) % prism.pointCount] + Vector3.up * yMax, wireFrameColor);
            }
        }
    }

    #endregion

    #region Utility Classes

    private class PrismCollision
    {
        public Prism a;
        public Prism b;
        public Vector3 penetrationDepthVectorAB;
    }

    private class Tuple<K,V>
    {
        public K Item1;
        public V Item2;

        public Tuple(K k, V v) {
            Item1 = k;
            Item2 = v;
        }
    }

    #endregion
}
