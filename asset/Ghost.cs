using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour {
	private Renderer[] wangge;
	public Material chaizhi;
	public Vector3 buzheng = Vector3.zero;

	public float xiaosanshij = 0.5f;
    public float duration = 0.02f;
    public float appearGhostDistance = 0.01f; // = d^2

    private float lastCheckTime = 0;
    private Vector3 lastPosition;
    private Queue<GameObject> ghosts = new Queue<GameObject>();
    
    void OnEnable()
    {
        if (wangge == null)
        {
            wangge = GetComponentsInChildren<Renderer>();
        }

        lastCheckTime = 0;
        lastPosition = transform.position;
    }

    void OnDisable()
    {
        foreach (var g in ghosts)
        {
            GameObject.Destroy(g);
        }
        ghosts.Clear();
    }

    void Update()
    {
        lastCheckTime += Time.deltaTime;
        if (lastCheckTime >= duration)
        {
            lastCheckTime = 0;

            if ( (transform.position - lastPosition).sqrMagnitude >= appearGhostDistance )
            {
                jihuo();
            }
            
            lastPosition = transform.position;
        }
    }

	public void jihuo ()
	{
		for (int i = 0; i < wangge.Length; i++) {
			shengcheng (wangge[i]);
		}
	}
    void shengcheng(Renderer zujian)
    {
        GameObject c = null;
        if (zujian is SkinnedMeshRenderer)
        {
            c = chuxian(zujian as SkinnedMeshRenderer);
        }
        else
        {
            c = chuxian(zujian as MeshRenderer);
        }

        if (c != null)
        {
            ghosts.Enqueue(c);

            StartCoroutine(xiaosan(c.GetComponent<MeshFilter>()));
        }

	}
	IEnumerator xiaosan (MeshFilter shuru){
		Mesh s = shuru.sharedMesh;

		float t = xiaosanshij;
		while (t > 0) {
			t -= Time.deltaTime;
			float u = t / xiaosanshij;

            shuru.GetComponent<Renderer>().sharedMaterial.SetColor("_TintColor", new Color(1, 1, 1, u));

			yield return new WaitForEndOfFrame ();
		}
        ghosts.Dequeue();
		Destroy (shuru.gameObject);
	}
	public GameObject chuxian(SkinnedMeshRenderer zhujian)
	{
		Mesh s = new Mesh ();
		GameObject ying = new GameObject("yingzi");
		MeshFilter lujing = ying.AddComponent<MeshFilter>();
		zhujian.BakeMesh (s);
		lujing .sharedMesh = s;
		MeshRenderer xuanran = ying.AddComponent<MeshRenderer> ();
		xuanran.sharedMaterial = Instantiate<Material>(chaizhi);
        xuanran.sharedMaterial.mainTexture = zhujian.sharedMaterial.mainTexture;
		ying.transform.position = zhujian.transform.position;
        ying.transform.localEulerAngles = zhujian.transform.localEulerAngles + buzheng;
        ying.transform.localScale = zhujian.transform.localScale;
		return ying;
	}

    public GameObject chuxian(MeshRenderer zhujian)
    {
        var mesh = zhujian.GetComponent<MeshFilter>().sharedMesh;
	    var mesh2 = Instantiate(mesh);

        GameObject ying = new GameObject("yingzi");
        MeshFilter lujing = ying.AddComponent<MeshFilter>();
        lujing.sharedMesh = mesh2;

        MeshRenderer xuanran = ying.AddComponent<MeshRenderer>();
        xuanran.sharedMaterial = Instantiate<Material>(chaizhi);
        xuanran.sharedMaterial.mainTexture = zhujian.sharedMaterial.mainTexture;
        ying.transform.position = zhujian.transform.position;
        ying.transform.localEulerAngles = zhujian.transform.localEulerAngles + buzheng;
        ying.transform.localScale = zhujian.transform.localScale;
        return ying;
    }
}
