using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    //n==unwalkable
    //r==room
    //c==corridor
    //

    [SerializeField]
    GameObject unwalkablePrefab,floorPrefab,starPrefab,exitPrefab;

    [SerializeField]
    int width = 50, height = 50,maxMoves=1500,starsToSpawn=10;

    int xPos = 0, yPos = 0;
    bool moveVertical = false,movePositive=true;

    char[,] levelGenerated;
    Texture2D mapBase;

    private void Awake()
    {
        if (PlayerPrefs.GetInt("Level") ==0 || Application.isEditor)
        {
            PlayerPrefs.SetInt("Level", 1);
        }
        else
        {
            PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level") + 1);
        }
        generateLevel();
    }

    private void Start()
    {
        MapController.me.setMapTexture(mapBase);
    }

    void generateLevel()
    {

        for(int x= 0; x < PlayerPrefs.GetInt("Level"); x++)
        {
            if(width<100)
            {
                width += 5;
            }

            if (height < 100)
            {
                height += 5;
            }

            if(maxMoves<3500)
            {
                maxMoves += 100;
            }

            starsToSpawn += 2;
        }


        levelGenerated = new char[width, height];
        xPos = Random.Range(0, width - 1);
        yPos = Random.Range(0, height - 1);
        List<Vector3> validPoints = new List<Vector3>();
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                levelGenerated[x, y] = 'n';
            }
        }

        for(int x =0;x<maxMoves;x++)
        {
            int r = Random.Range(0, 100);

            if(r<80)
            {
                //do nothing
                
                levelGenerated[xPos, yPos] = 'c';
                validPoints.Add(new Vector3(xPos,0, yPos));


                //while valid the points on the edge of the cursor are not added to the validPoints list to make sure objects spawned have some space around them if in a corridor
                if (isPosInWorld(xPos + 1, yPos))
                {
                    levelGenerated[xPos+1, yPos] = 'c';

                }

                if (isPosInWorld(xPos - 1, yPos))
                {
                    levelGenerated[xPos-1, yPos] = 'c';

                }

                if (isPosInWorld(xPos, yPos+1))
                {
                    levelGenerated[xPos, yPos+1] = 'c';

                }

                if (isPosInWorld(xPos, yPos-1))
                {
                    levelGenerated[xPos, yPos-1] = 'c';

                }

            }
            else if(r<95)
            {
                //change direction
                moveVertical = !moveVertical;
                levelGenerated[xPos, yPos] = 'c';
                validPoints.Add(new Vector3(xPos, 0, yPos));

                if (isPosInWorld(xPos + 1, yPos))
                {
                    levelGenerated[xPos + 1, yPos] = 'c';

                }

                if (isPosInWorld(xPos - 1, yPos))
                {
                    levelGenerated[xPos - 1, yPos] = 'c';

                }

                if (isPosInWorld(xPos, yPos + 1))
                {
                    levelGenerated[xPos, yPos + 1] = 'c';

                }

                if (isPosInWorld(xPos, yPos - 1))
                {
                    levelGenerated[xPos, yPos - 1] = 'c';

                }
                int r2 = Random.Range(0, 100);
                if(r2<50)
                {
                    movePositive = !movePositive;
                }
            }
            else
            {
                //create room
                int width = Random.Range(3, 7);
                int height = Random.Range(3, 7);

                for(int x2 = xPos - width; x2 < xPos; x2 ++)
                {
                    for(int y2 = yPos - height; y2 < yPos; y2++)
                    {
                        if (isPosInWorld(x2, y2))
                        {
                            levelGenerated[x2, y2] = 'r';
                            validPoints.Add(new Vector3(xPos, 0, yPos));
                        }
                    }
                }
            }
            levelGenerated[xPos, yPos] = 'c';

            if (moveVertical == true)
            {
                if(movePositive==true)
                {
                    if (yPos<height-1)
                    {
                        yPos++;
                    }
                    else
                    {
                        movePositive = false;
                    }
                }
                else
                {
                    if (yPos>0)
                    {
                        yPos--;
                    }
                    else
                    {
                        movePositive = true;
                    }
                }
            }
            else
            {
                if (movePositive == true)
                {
                    if (xPos<width-1)
                    {
                        xPos++;
                    }
                    else
                    {
                        movePositive = false;
                    }
                }
                else
                {
                    if (xPos>0)
                    {
                        xPos--;
                    }
                    else
                    {
                        movePositive = true;
                    }
                }
            }
        }

        //createWalls
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (levelGenerated[x, y] == 'n')
                {
                    GameObject g = (GameObject) Instantiate(unwalkablePrefab, new Vector3(x, 0, y), Quaternion.Euler(0, 0, 0));
                    g.transform.parent = this.transform;
                }
            }
        }

        //create floor
        GameObject floor = (GameObject) Instantiate(floorPrefab, new Vector3((width / 2)-0.5f, -1.0f, (height / 2)-0.25f), Quaternion.Euler(0, 0, 0));
        floor.transform.localScale = new Vector3(width+1, 1, height+1);

        //create world edge
        GameObject topWall = (GameObject)Instantiate(unwalkablePrefab, new Vector3((width / 2), 0.5f, height), Quaternion.Euler(0, 0, 0));
        topWall.transform.localScale = new Vector3(width, 2, 1);
        topWall.name = "Top Wall";
        GameObject bottomWall = (GameObject)Instantiate(unwalkablePrefab, new Vector3((width / 2), 0.5f, -1), Quaternion.Euler(0, 0, 0));
        bottomWall.transform.localScale = new Vector3(width, 2, 1);
        bottomWall.name = "Bottom Wall";
        GameObject rightWall = (GameObject)Instantiate(unwalkablePrefab, new Vector3(width , 0.5f, height/2), Quaternion.Euler(0, 0, 0));
        rightWall.transform.localScale = new Vector3(1, 2, height);
        rightWall.name = "Right Wall";
        GameObject leftWall = (GameObject)Instantiate(unwalkablePrefab, new Vector3(-1.0f, 0.5f, height / 2), Quaternion.Euler(0, 0, 0));
        leftWall.transform.localScale = new Vector3(1, 2, height);
        leftWall.name = "Left Wall";
    
        GameObject player = (GameObject)Instantiate(PrefabStore.me.player, validPoints[Random.Range(0, validPoints.Count)], Quaternion.Euler(0, 0, 0));
        FindObjectOfType<CameraFollowPlayer>().SetPlayerToFollow(player.transform);

        for (int x = 0; x < starsToSpawn; x++)
        {
            GameObject star = (GameObject) Instantiate(starPrefab, validPoints[Random.Range(0, validPoints.Count)], starPrefab.transform.rotation);
            levelGenerated[Mathf.RoundToInt(star.transform.position.x), Mathf.RoundToInt(star.transform.position.z)] = 's';
        }

        GameObject exit = (GameObject)Instantiate(exitPrefab, validPoints[Random.Range(0, validPoints.Count)], exitPrefab.transform.rotation);
        levelGenerated[Mathf.RoundToInt(exit.transform.position.x), Mathf.RoundToInt(exit.transform.position.z)] = 'e';

        int numOfHelisToSpawn = 5 + PlayerPrefs.GetInt("Level");
        for(int x=0;x<numOfHelisToSpawn;x++)
        {
            Vector3 pos = validPoints[Random.Range(0,validPoints.Count)];
            if (Vector3.Distance(pos, player.transform.position) >= 15)
            {
                Instantiate(PrefabStore.me.hind, pos, Quaternion.Euler(0, 0, 0));
            }
        }

        int numOfSamsToSpawn = 3 + PlayerPrefs.GetInt("Level");
        for (int x = 0; x < numOfSamsToSpawn; x++)
        {
            Vector3 pos = validPoints[Random.Range(0, validPoints.Count)];
            if (Vector3.Distance(pos, player.transform.position) >= 15)
            {
                Instantiate(PrefabStore.me.sam, pos, Quaternion.Euler(0, 0, 0));
            }
        }

        createMapBase();
    }

    void createMapBase()
    {
        mapBase = new Texture2D(width, height);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (levelGenerated[x, y] == 'n')
                {
                     mapBase.SetPixel(x,y,Color.gray);
                }else if (levelGenerated[x, y] == 'r' || levelGenerated[x, y] == 'c')
                {
                    mapBase.SetPixel(x, y, new Color(0.1f,0.1f,0.1f,1.0f));
                }else if (levelGenerated[x, y] == 'e')
                {
                    mapBase.SetPixel(x, y, Color.green);
                }
            }
        }
        mapBase.filterMode = FilterMode.Point;
        mapBase.Apply();
    }

    bool isPosInWorld(int x,int y)
    {
        if(x<0||y<0 || y>=height||x>=width)
        {
            return false;
        }
        return true;
    }
}
