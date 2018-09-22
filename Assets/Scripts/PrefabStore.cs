using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabStore : MonoBehaviour
{
    public static PrefabStore me;
    public GameObject hind, sam, missile, samMissile, explosionEffect,player;

    [SerializeField]
    Texture2D[] texturesForBuildigns;

    [SerializeField]
    Shader defaultShader;

    public Material wallMat;
    public List<Material> oneHighBuildings, twoHighBuildings, threeHighBuildings;//list of materials for the heights of buildings we could potentially have, 
    //hopefully by just referencing materials from here rather than changing each instance performance will be increased

    private void Awake()
    {
        me = this;
        initialiseMaterials();
    }

    void initialiseMaterials()
    {
        wallMat = new Material(defaultShader);
        wallMat.SetColor(0, Color.grey);
        

        foreach(Texture2D t in texturesForBuildigns)
        {
            Material m = new Material(defaultShader);
            m.SetTexture("_MainTex",t);
            oneHighBuildings.Add(m);

            Material m2 = new Material(defaultShader);
            m2.SetTexture("_MainTex", t);
            m2.SetTextureScale("_MainTex",new Vector2(1, 2));
            twoHighBuildings.Add(m2);
           
            Material m3 = new Material(defaultShader);
            m3.SetTexture("_MainTex", t);
            m3.SetTextureScale("_MainTex", new Vector2(1, 3));
            threeHighBuildings.Add(m3);
        }
    }

    public Material GetMaterialForBuilding(float yScale)
    {
        if(yScale<1.1f)
        {
            Debug.Log("returning 1 high material");
            return oneHighBuildings[Random.Range(0, oneHighBuildings.Count)];
        }else if(yScale<2.1f)
        {
            Debug.Log("returning 2 high material");

            return twoHighBuildings[Random.Range(0, twoHighBuildings.Count)];
        }else if (yScale < 3.1f)
        {
            Debug.Log("returning 3 high material");

            return threeHighBuildings[Random.Range(0, threeHighBuildings.Count)];
        }
        return oneHighBuildings[Random.Range(0, oneHighBuildings.Count)];
    }
}
