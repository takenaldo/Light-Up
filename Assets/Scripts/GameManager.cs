using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    //    public GameObject[] detectors;
    public List<GameObject> detectors;
    public GameObject holder;
    public float margin = 0.1f;
    public bool gameFinished = false;
    public int current_level = 1;
    public static int LANGUAGE_EN = 0;
    public static int LANGUAGE_RU = 1;
    public int levels = 9;

    public GameObject dialogWin;
    public GameObject background;
    public Sprite backgroundWinningSprite;


    private void Awake()
    {
        if (instance == null)
            instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {

        detectors = new List<GameObject>(GameObject.FindGameObjectsWithTag("detector"));

        foreach (GameObject item in detectors)
        {
            item.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0);
        }

        // setDetectors();
        isGameFinished();
        Debug.Log("CURREnt lvl " + current_level);
    }

    bool timerStarted = false;
    bool timerEnded = true;
    private float start = 0f;

    // Update is called once per frame
    void Update()
    {

        if (gameFinished && !dialogWin.activeSelf)
        {

            background.GetComponent<SpriteRenderer>().sprite = backgroundWinningSprite;
//            dialogWin.SetActive(true);

            if (current_level + 1 > Helper.getUserLevel())
            {
                Debug.Log("here curr " + current_level);
                PlayerPrefs.SetInt(Helper.USER_LEVEL, Math.Min(current_level + 1, levels));
                Debug.Log(Helper.getUserLevel());
                //   Helper.updateLevel();
            }

            Debug.Log("Game OVER");

            if (!timerStarted)
            {
                timerStarted = true;
                timerEnded = false;

                start = Time.time;
            }

        }

        if (timerStarted && !timerEnded)
        {
            if(Time.time - start >= 1.5f)
            {
                timerStarted = false;
                timerEnded = true;

                dialogWin.SetActive(true);
            }
        }



    }

    private void setDetectors()
    {
        detectors = new List<GameObject>( GameObject.FindGameObjectsWithTag("detector"));
/*        for (int i = 0; i < holder.transform.childCount; i++)
        {
            GameObject piece = holder.transform.GetChild(i).transform.gameObject;

            for (int j = 0; j < piece.transform.childCount; j++)
            {
                GameObject detector = holder.transform.GetChild(i).transform.gameObject; ;
                detectors.Add(detector);
            }

        }*/

        Debug.Log(detectors.Count + " detectors Found");

    }


    public bool isGameFinished()
    {
        detectors = new List<GameObject>(GameObject.FindGameObjectsWithTag("detector"));

        for (int i = 0; i < detectors.Count; i++)
        {
            if (!detectors[i].activeInHierarchy)
                continue;
            bool no_near_obj = true;
            for (int j = 0; j < detectors.Count; j++)
            {
                GameObject detector1 = detectors[i];
                GameObject detector2 = detectors[j];

                if (i == j)
                    continue;

                if (detector1.transform.parent.gameObject == detector2.transform.parent.gameObject)
                    continue;

                if (!detector1.activeInHierarchy || !detector2.activeInHierarchy)
                    continue;

                float distance = Vector2.Distance(detector1.transform.position, detector2.transform.position);
//                Debug.Log("Comparing:  "+ detector1.transform.parent.name+"/" + detectors[i].name + " to  "+ detectors[j].transform.parent.name + "/"+ detectors[j].name + " : " + distance);
                //                Debug.Log("To: " + detectors[j].transform.position);
//                Debug.Log("Distance: "+distance);
                if(distance < margin)
                {
                    no_near_obj = false;
  //                  Debug.Log("near found");
                }

            }

            if (no_near_obj)
            {
                Debug.Log("Not found for  " +detectors[i].transform.parent.name+" / "+detectors[i].name );

                Debug.Log("NO Game Over");
                return false;
            }
        }

        checkBulbConnectivity();
        return true;

    }

    private void checkBulbConnectivity()
    {
        GameObject [] bulbs =  getAll("bulb");
        GameObject [] powers = getAll("power");

        foreach (GameObject bulb in bulbs)
        {
            List<GameObject> connects =  getConnects(bulb, new List<GameObject>());
            for (int i = 0; i < connects.Count; i++)
            {
                Debug.Log("===> "+connects[i].name);
            }
        }

    }

    List<GameObject> getConnects(GameObject go, List<GameObject> allConnects)
    {
        if(!allConnects.Contains(go))
            allConnects.Add(go);

//        List<GameObject> allConnects = new List<GameObject>();
        Debug.Log("CHILD count: "+ go.transform.childCount);
        for (int i = 0; i < go.transform.childCount; i++)
        {
            Detector d = go.transform.GetChild(i).gameObject.GetComponent<Detector>();
            Debug.Log(go.name+" connects len"+d.connects.Count);
            foreach (GameObject item in d.connects)
            {
                Debug.Log("NAME : "+item.transform.parent.gameObject);

                if (allConnects.Contains(item.transform.parent.gameObject))
                {
                    Debug.Log("ALREADY in LIST");
                    continue;
                }

                if (go == item.transform.parent.gameObject)
                {
                    continue;
                }
                else
                {
                    allConnects.Add(item.transform.parent.gameObject);
                    allConnects = getConnects(item.transform.parent.gameObject, allConnects);

                }


                if (go.tag == "power")
                {
                    Debug.Log("ppppppppppppppppppppp");
                    return allConnects;
                }

                   // allConnects.AddRange(getConnects(item.transform.parent.gameObject));
            }
            Debug.Log("All len " + allConnects.Count);
        }

        return allConnects;


    }


    private GameObject[] getAll(string tag)
    {
        return GameObject.FindGameObjectsWithTag(tag);
    }
}
