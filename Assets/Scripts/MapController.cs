using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class MapController : MonoBehaviour
{
    public static MapController me;

    [SerializeField]
    RawImage mapDisplay;

    Texture2D baseMapTex;
    bool[,] positionVisited;
    private void Awake()
    {
        me = this;
    }

    private void Update()
    {
        mapDisplay.texture = (Texture)generateMapTexture();

    }


    Texture2D generateMapTexture()//combines base map texture with another texture that contains information that changes (player pos, star locations etc..)
    {
        Texture2D combined = new Texture2D(baseMapTex.width, baseMapTex.height);

        for(int x = 0; x < combined.width; x++)
        {
            for(int y = 0; y < combined.height; y++)
            {
                if (positionVisited[x, y] == true)
                {
                    combined.SetPixel(x, y, baseMapTex.GetPixel(x, y));
                }
                else
                {
                    combined.SetPixel(x, y,Color.black);
                }
            }
        }
        Vector2Int ind = getWorldPosInArray(PlayerMovement.me.transform.position);

        combined.SetPixel(ind.x,ind.y, Color.red);
        revealMap(ind);
        foreach(Pickup p in GameController.me.pickups)
        {
            if (p.IsCollected == false)
            {
                Vector2Int ind2 = getWorldPosInArray(p.transform.position);
                combined.SetPixel(ind2.x,ind2.y, Color.yellow);
            }
            else
            {
                Vector2Int ind2 = getWorldPosInArray(p.transform.position);
                combined.SetPixel(ind2.x, ind2.y, new Color(0.1f, 0.1f, 0.1f, 1.0f));

            }
        }

        foreach (HelicopterEnemy s in HelicopterEnemy.instances)
        {
            if(Vector3.Distance(PlayerMovement.me.transform.position,s.gameObject.transform.position)>10)
            {
                continue;
            }

            Vector2Int pos = getWorldPosInArray(s.gameObject.transform.position);
            combined.SetPixel(pos.x, pos.y, Color.blue);
        }

        foreach (SamSite s in SamSite.instances)
        {
            if (Vector3.Distance(PlayerMovement.me.transform.position, s.gameObject.transform.position) > 10)
            {
                continue;
            }

            Vector2Int pos = getWorldPosInArray(s.gameObject.transform.position);
            combined.SetPixel(pos.x, pos.y, Color.blue);
        }

        combined.filterMode = FilterMode.Point;

        combined.Apply();

        return combined;
    }

    Vector2Int getWorldPosInArray(Vector3 position)//converts a world position into an index for drawing objects to the map
    {
        //since the levels start at 0,0 there is no need to modify the position values however if this were not the case we would need to work out the difference between (0,0) and the bottom left of the level and 
        //subtract that from the position when calculating the index.
        int x = Mathf.RoundToInt(position.x);
        int y = Mathf.RoundToInt(position.z);

       
        Vector2Int retVal = new Vector2Int(x,y);

        return retVal;
    }

    void revealMap(Vector2Int playersPosition)
    {
        for(int x = playersPosition.x-3;x<playersPosition.x+3;x++)
        {
            for(int y = playersPosition.y - 3; y < playersPosition.y + 3; y++)
            {
                if (isPosValid(x, y, positionVisited.GetLength(0), positionVisited.GetLength(1)))
                {
                    positionVisited[x, y] = true;
                }
            }
        }
    }

    bool isPosValid(int x, int y, int width, int height)
    {
        if (x < 0 || y < 0 || y >= height || x >= width)
        {
            return false;
        }
        return true;
    }

    public void setMapTexture(Texture2D texture)
    {
        baseMapTex = texture;
        positionVisited = new bool[baseMapTex.width, baseMapTex.height];
        for(int x =0;x<positionVisited.GetLength(0);x++)
        {
            for(int y =0;y<positionVisited.GetLength(1);y++)
            {
                positionVisited[x, y] = false;
            }
        }
    }
}
