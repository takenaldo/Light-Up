using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edge : MonoBehaviour
{
    // the rate of degrees this edge will rotate while clicked
    public float rotationDegrees = 180;
    public Sprite sprite_OFF;
    public Sprite sprite_ON;

    private bool flipX = false;
    private AudioSource clickBeep;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Debug.Log(transform.GetChild(i).name);
            transform.GetChild(i).GetComponent<Detector>().updateConnects();
        }
        clickBeep = GameObject.FindGameObjectWithTag("click_beep").GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
    //        rotateEdge();
        }
    }

    public void rotateEdge()
    {
        Vector3 new_rotation = new Vector3( transform.rotation.x, transform.rotation.y, transform.rotation.z );
        new_rotation.z += rotationDegrees;

        Debug.Log("Im " + new_rotation.z);
        
        int z = (int)(new_rotation.z);

        int max = 10;
        int half = max / 2;
        
        if(rotationDegrees % 5 == 0)
        {
            max = 5;
            half = max / 2;
        }

        int mod = (z % max);

        if (mod < half)
            z = z - mod;
        else
            z = z + (max - mod);

        new_rotation.z = z;

        Debug.Log("now Im " + new_rotation.z);

        Debug.Log("rot " + new_rotation.z);
        
        transform.Rotate(new_rotation);

    }


    public bool switching = false;
    public int swith_a, switch_b;

    private void OnMouseDown()
    {
        performClick();
    }

    public void performClick()
    {

        if (PlayerPrefs.GetString(Helper.SETTING_MUSIC, Helper.STATUS_ON) == Helper.STATUS_ON) {
            clickBeep.Play();
        }
            Debug.Log("ROTATE");
            if (switching)

            {

                rotationDegrees = rotationDegrees * 5;
                /*Vector3 new_rotation = new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z);
                float z = transform.rotation.z;
                Debug.Log("SWITch");
                //            if(MathF.Abs(z - swith_a) < 45)
                if (z == swith_a)
                {
                    z = switch_b;
                }
                else
                {
                    z = swith_a;
                }

                *//*            if(new_rotation.z < 280)
                            {
                                z = 360;
                            }
                            else
                            {
                                z = 270;
                            }*//*


                Debug.Log(new_rotation.z);
                transform.Rotate(new_rotation);
    */
            }
            //    performFlipX();
            else
            {

            }

            rotateEdge();

            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).GetComponent<Detector>().updateConnects();
            }

            List<GameObject> connects = getConnectsifPowerisOn();

            if (connects.Count > 0)
            {
                Debug.Log("connected to power");
                GetComponent<SpriteRenderer>().sprite = sprite_ON;

                foreach (GameObject connect in connects)
                {
                //                connect.GetComponent<SpriteRenderer>().sprite = sprite_ON;


                    Edge edgeConnect = connect.GetComponent<Edge>();
                if (edgeConnect != null)
                {
                    if (connect.name == "arc_left" || connect.name == "arc_right")
                    {
                        Debug.Log(connect.name);
                        Debug.Log("---here---");
                    }
                    edgeConnect.GetComponent<SpriteRenderer>().sprite = edgeConnect.sprite_ON;
                }
                else
                {
                    FlipEdge f_edgeConnect = connect.GetComponent<FlipEdge>();
                    if (f_edgeConnect != null)
                        f_edgeConnect.GetComponent<SpriteRenderer>().sprite = f_edgeConnect.sprite_ON;

                }
                }
            }
            else
            {
                Debug.Log("NOT connected to power");
                GetComponent<SpriteRenderer>().sprite = sprite_OFF;
            }

            GameManager.instance.gameFinished = GameManager.instance.isGameFinished();

    }

    private void performFlipX()
    {
        SpriteRenderer spr = GetComponent<SpriteRenderer>();
        if (spr.flipX)
        {
            spr.flipX = false;
            Debug.Log("FLIPPING X");
        }
        else
            spr.flipX = true;

//            GetComponent<SpriteRenderer>().flipX = !GetComponent<SpriteRenderer>().flipX;
    }

    private List<GameObject> getConnectsifPowerisOn()
    {
        bool connectedToPower = false;
        List<GameObject> connects = new List<GameObject>();
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            GameObject child = gameObject.transform.GetChild(i).gameObject;

            connects = getConnects(child.transform.parent.gameObject, new List<GameObject>());



            for (int j = 0; j < connects.Count; j++)
            {
                Debug.Log("===> " + connects[j].name);
                if (connects[j].tag == "power")
                {
                    connectedToPower = true;
                }
            }
        }
        if (connectedToPower)
            return connects;
        else
            return new List<GameObject>();
    }

    List<GameObject> getConnects(GameObject go, List<GameObject> allConnects)
    {
        if (!allConnects.Contains(go))
            allConnects.Add(go);

        //        List<GameObject> allConnects = new List<GameObject>();
        Debug.Log("CHILD count: " + go.transform.childCount);
        for (int i = 0; i < go.transform.childCount; i++)
        {
            Detector d = go.transform.GetChild(i).gameObject.GetComponent<Detector>();
            Debug.Log(go.name + " connects len" + d.connects.Count);
            foreach (GameObject item in d.connects)
            {
                Debug.Log("NAME : " + item.transform.parent.gameObject);

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
}
