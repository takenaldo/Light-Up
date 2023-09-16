using System.Collections;
using UnityEngine;
//using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;

public class SplashScreenManager : MonoBehaviour
{
    public string nextScene= "Main";

    private float startTime;
    public float waitingSeconds = 5;

    public GameObject progressBar;

    // for preventing language loading redundancy
    private bool active = false;

    // project specific for progress bar
    public GameObject mover, destnation;

    // 
    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
//        loadUserPreferedLocale();

//        moveUp();
    
    }


    // Update is called once per frame
    void Update()
    {
        float now = Time.time;
        
/*        if (now - startTime > waitingSeconds)
            SceneManager.LoadScene(nextScene);*/
/*

        if (System.String.Format("{0:.#}", mover.transform.position.x) == System.String.Format("{0:.#}", destnation.transform.position.x))
        {*/

        if(mover.transform.position.x > destnation.transform.position.x) { 
            SceneManager.LoadScene(nextScene);
        }

        //            rotateProgressBar();
        //    moveUp();
        moveRight();

    }

    void rotateProgressBar()
    {
        progressBar.gameObject.transform.Rotate(0, 0, 1 * 100);

    }

    private void moveUp()
    {
        progressBar.gameObject.transform.Translate(Vector2.up * 2);
    }

    private void moveRight()
    {
//        progressBar.gameObject.transform.Translate(Vector2.right * Time.deltaTime * 2);
        mover.gameObject.transform.Translate(Vector2.right * Time.deltaTime * 4);

    }

    private void loadUserPreferedLocale()
    {
        int local_id = PlayerPrefs.GetInt(Helper.LANGUAGE, 0);
  //      ChangeLocale(local_id);
        
        //ChangeLocale(GameManager.LANGUAGE_RU);

    }

/*    public void ChangeLocale(int localeID)
    {
        if (active == true)
            return;
        StartCoroutine(SetLocale(localeID));
    }


    IEnumerator SetLocale(int _localeID)
    {
        active = true;
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[_localeID];

        PlayerPrefs.SetInt(Helper.LANGUAGE, _localeID);

        active = false;
    }
*/


}
