using System.Collections;
using System.IO;
using UnityEngine;
using UnityEditor;

public class DrawOnSurface : MonoBehaviour {

    public GameObject Brush;
    public float BrushSize = 0.1f;
    public RenderTexture RTexture;

    public Texture2D drawableTexture;
    public NumberProcessor numberProcessor;


    void Start() 
    {
        


    }

	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButton(0))
        {
            //cast a ray to the plane
            var Ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(Ray, out hit))
            {
                //instanciate a brush
                var go = Instantiate(Brush, hit.point , Quaternion.identity, transform);
                go.transform.localScale = Vector3.one * BrushSize;
            }

        }
	}

    public void Save()
    {
        StartCoroutine(CoSave());
    }

    private IEnumerator CoSave()
    {
        /*//wait for rendering
        yield return new WaitForEndOfFrame();
        Debug.Log(Application.dataPath + "/savedImage.png");

        //set active texture
        RenderTexture.active = RTexture;

        //convert rendering texture to texture2D
        var texture2D = new Texture2D(RTexture.width, RTexture.height);
        texture2D.ReadPixels(new Rect(0, 0, RTexture.width, RTexture.height), 0, 0);
        texture2D.Apply();

        //write data to file
        var data = texture2D.EncodeToPNG();
        File.WriteAllBytes(Application.dataPath + "/savedImage.png", data);

        //AssetDatabase.Refresh();

        numberProcessor.ExecuteModel();
        */
        // Wait for rendering
        yield return new WaitForEndOfFrame();
        //Debug.Log(Application.dataPath + "/savedImage.png");

        // Set active texture
        RenderTexture.active = RTexture;

        // Convert rendering texture to Texture2D
        var texture2D = new Texture2D(RTexture.width, RTexture.height);
        texture2D.ReadPixels(new Rect(0, 0, RTexture.width, RTexture.height), 0, 0);
        texture2D.Apply();

        // Wait for the texture to be updated
        yield return null;

        // Write data to file
        //var data = texture2D.EncodeToPNG();
        //File.WriteAllBytes(Application.dataPath + "/savedImage.png", data);

        // AssetDatabase.Refresh(); // This line won't work in a build, it's for the editor

        numberProcessor.ExecuteModel(texture2D);
    }

    public void ClearBoard()
    {
        // Loop through all children of the parent GameObject
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            // Destroy each child GameObject
            Destroy(transform.GetChild(i).gameObject);
        }
    }
}