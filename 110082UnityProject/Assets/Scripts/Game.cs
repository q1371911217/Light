using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum GameStep
{
    Wait,
    Start,
    Shoot,
    End,
}

public class Game : MonoBehaviour
{

    public AudioClip clip;
    public AudioClip winClip;
    public AudioClip loseClip;
    public AudioClip touchClip;
    public AudioClip lightClip;


    public GameObject touchEffect;

    public Transform menuLayer;
    public Transform menuCellRoot;


    public GameObject resultLayer;
    public Transform resultWinLayer;
    public Transform resultLoseLayer;

    public Transform GameRoot;

    public GameObject btnVolume, spDisable;

    public Text lblLevel;

    //public Button btnVolume;

    public GameObject LighterGo;
    public List<Transform> firePrefList;

    private float firePosX = -309;
    private float firePoxY = -264;
    private float fireOffsetY = -106;

    private Vector3 fireSourcePos = new Vector3(24.3f, 39.7f, 0);

    private List<int> levelLightCount = new List<int>() { 3, 1, 3, 2, 3, 1, 9, 1, 3, 4,3, 1, 3, 2, 3, 1, 9, 1, 3, 4 };
    private List<int> levelWallCount = new List<int>() { 1, 0, 1, 0, 3, 0, 4, 2, 0, 4, 1, 0, 1, 0, 3, 0, 4, 2, 0, 4 };
    //private List<int> levelLightCount = new List<int>() { 1, 2, 3, 3 ,3,9, 1, 3, 1, 4,  3,1,3,2,3,1,9,1,3,4};
    //private List<int> levelWallCount = new List<int>() { 0, 0, 1, 1,3,4, 0, 0, 2, 4,  1,0,1,0,3,0,4,2,0,4 };
    private List<int[]> levelFireList = new List<int[]>()
    {
        //new int[]{1,1,1,1},//1
        //new int[]{1,1,1,1},//2
        //new int[]{1,1,1,1},//3
        //new int[]{1,1,1,1},//4
        //new int[]{1,1,1,1},//5
        //new int[]{3,3,3,3},//6
        //new int[]{1,1,1,1},//7
        //new int[]{1,1,1,1},//8
        //new int[]{1,1,1,1},//9
        //new int[]{1,1,1,1},//10

        // new int[]{1,1,1,1},//3
        // new int[]{1,1,1,1},//5
        // new int[]{1,1,1,1},//7
        //  new int[]{1,1,1,1},//9
        //   new int[]{1,1,1,1},//1
        //   new int[]{1,1,1,1},//4
        //   new int[]{1,1,1,1},//2
        //   new int[]{1,1,1,1},//8            
        //    new int[]{3,3,3,3},//6
        //    new int[]{1,1,1,1},//10

    new int[]{1,1,1,1},//4
    new int[]{1,1,1,1},//4
    new int[]{1,1,1,1},//4
    new int[]{1,1,1,1},//4
    new int[]{1,1,1,1},//4
    new int[]{1,1,1,1},//4
    new int[]{3,3,3,3},//6
    new int[]{1,1,1,1},//4
    new int[]{1,1,1,1},//4
    new int[]{1,1,1,1},//4

    new int[]{1,1,1,1},//4
    new int[]{1,1,1,1},//4
    new int[]{1,1,1,1},//4
    new int[]{1,1,1,1},//4
    new int[]{1,1,1,1},//4
    new int[]{1,1,1,1},//4
    new int[]{3,3,3,3},//6
    new int[]{1,1,1,1},//4
    new int[]{1,1,1,1},//4
    new int[]{1,1,1,1},//4
    };

    private List<Vector3> lighterPosList = new List<Vector3>()
    {
        //new Vector3(-14, -358, 0),//1
        //new Vector3(-192, -243, 0),//2
        //new Vector3(-192, -243, 0),//3
        //new Vector3(224, -243, 0),//4
        //new Vector3(-10, -360, 0),//5
        //new Vector3(-192, -266.7f, 0),//6
        //new Vector3(-14, -358, 0),//7
        //new Vector3(-192, -318, 0),//8
        // new Vector3(-192, -243, 0),//9
        // new Vector3(-192, 243, 0),//10


         new Vector3(224, -243, 0),//4
         new Vector3(-14, -358, 0),//1
         new Vector3(-192, -243, 0),//3
         new Vector3(-192, -243, 0),//2
         new Vector3(-10, -360, 0),//5
          new Vector3(-14, -358, 0),//7
           new Vector3(-192, -266.7f, 0),//6
           new Vector3(-192, -243, 0),//9
           new Vector3(-192, -318, 0),//8
            new Vector3(-192, 243, 0),//10
            
           new Vector3(224, -243, 0),//4
         new Vector3(-14, -358, 0),//1
         new Vector3(-192, -243, 0),//3
         new Vector3(-192, -243, 0),//2
         new Vector3(-10, -360, 0),//5
          new Vector3(-14, -358, 0),//7
           new Vector3(-192, -266.7f, 0),//6
           new Vector3(-192, -243, 0),//9
           new Vector3(-192, -318, 0),//8
            new Vector3(-192, 243, 0),//10   
             
              
    };

    List<Transform> fireTransList = new List<Transform>();
    public static List<Transform> wallTransList = new List<Transform>();

    public static int curLevel;
    private int maxLevel;
    private const int totalLevel = 20;

    public static GameStep gameStep;

    public static bool volumOpen = true;

    private int shootTimes;

    private AudioSource audioSource;

    void Start()
    {

        curLevel = 1;
        maxLevel = PlayerPrefs.GetInt("maxLevel", 1);

        audioSource = GameObject.Find("GameManager").GetComponent<AudioSource>();
        audioSource.volume = Game.volumOpen ? 1 : 0;
        //btnVolume.transform.Find("spDisable").gameObject.SetActive(!Game.volumOpen);
        btnVolume.SetActive(Game.volumOpen);// transform.Find("spDisable").gameObject.SetActive(!Game.volumOpen);
        spDisable.gameObject.SetActive(!Game.volumOpen);

        gameStep = GameStep.Wait;

        menuLayer.gameObject.SetActive(true);
        menuLayer.transform.localScale = Vector3.zero;
        menuLayer.transform.DOScale(Vector3.one, 0.3f);

        updateMenu();
        //gameStart();
    }

    void clear()
    {
        if(levelTrans != null)
        {
            GameObject.Destroy(levelTrans.gameObject);
            levelTrans = null;
        }

        for(int i = fireTransList.Count - 1; i >=0; i--)
        {
            if(fireTransList != null)
            {
                GameObject.Destroy(fireTransList[i].gameObject);
            }
        }
        if(curFire != null)
        {
            GameObject.Destroy(curFire.gameObject);
            curFire = null;
        }

        if (menuLayer.gameObject.activeSelf)
        {
            menuLayer.DOScale(Vector3.zero, 0.3f).OnComplete(() =>
            {
                menuLayer.gameObject.SetActive(false);
            });
        }

        fireTransList.Clear();
        wallTransList.Clear();

        StopAllCoroutines();
        audioSource.Play();

    }
    Transform levelTrans;
    Transform curFire;
    Transform lastFire;
    void gameStart()
    {
        clear();

        lblLevel.text = string.Format("LEVEL:{0}", curLevel);

        shootTimes = 0;

        GameObject levelObj = Resources.Load<GameObject>(string.Format("level/level_{0}", curLevel));
        GameObject levelGo = GameObject.Instantiate(levelObj);
        levelGo.gameObject.SetActive(true);
        levelTrans = levelGo.transform;
        levelTrans.SetParent(GameRoot);
        levelTrans.localPosition = Vector3.zero;
        levelTrans.localScale = Vector3.one;
        levelTrans.localRotation = Quaternion.identity;
        levelTrans.SetAsFirstSibling();

        LighterGo.transform.localPosition = lighterPosList[curLevel - 1];

        int[] fireArr = levelFireList[curLevel - 1];
        for(int i = 0; i < fireArr.Length; i++)
        {
            Transform fireTrans = GameObject.Instantiate(firePrefList[fireArr[i] - 1]);
            fireTrans.gameObject.SetActive(true);
            fireTrans.SetParent(GameRoot);
            fireTrans.localScale = Vector3.one;
            fireTrans.localRotation = Quaternion.identity;
            fireTrans.localPosition = new Vector3(firePosX, firePoxY + fireOffsetY * (i - 2), 0);

            fireTransList.Add(fireTrans);
        }
        if(levelWallCount[curLevel - 1] > 0)
        {
            for (int i = 0; i < levelWallCount[curLevel - 1]; i++)
            {
                Transform wall = levelTrans.Find(string.Format("wall_{0}", i + 1));
                wallTransList.Add(wall);
            }
        }
        
        //fireTransList[0].SetParent(LighterGo.transform);
        fireTransList[0].position = firePrefList[0].position;
        curFire = fireTransList[0];
        fireTransList.RemoveAt(0);

        gameStep = GameStep.Start;
    }
    List<Vector3> lineRendererPosList;
    public void shoot(List<Vector3> lineRendererPosList)
    {
        gameStep = GameStep.Shoot;
        shootTimes += 1;

        //for (int i = 0; i < lineRendererPosList.Count; i++)
        //{
        //    Debug.LogError(lineRendererPosList[i].ToString());
        //}
        this.lineRendererPosList = lineRendererPosList;
        this.lineRendererPosList.RemoveAt(0);
        lastFire = curFire;
        curFire = null;
        fireMove();

        if (fireTransList.Count > 0)
        {
            fireTransList[0].DOMove(firePrefList[0].position, 0.3f);
            curFire = fireTransList[0];
            fireTransList.RemoveAt(0);
        }
        else
        {
            curFire = null;
        }
    }

    void fireMove(Action callback = null)
    {
        Vector3 target = lineRendererPosList[0];
        lineRendererPosList.RemoveAt(0);
        float distance = Vector3.Distance(target, lastFire.localPosition);
        float duration = 3 / (2000 / distance);
        lastFire.GetComponent<Image>().enabled = false;
        lastFire.Find("fire").gameObject.SetActive(true);
        lastFire.Find("color").gameObject.SetActive(false);
        lastFire.transform.DOLocalMove(target, duration).SetEase(Ease.Linear).OnComplete(() =>
        {
            touchEffect.gameObject.SetActive(false);
            touchEffect.gameObject.SetActive(true);
            touchEffect.transform.localPosition = new Vector3(target.x, target.y + 80, 0);
            if (lineRendererPosList.Count > 0)
            {
                audioSource.PlayOneShot(touchClip);
                fireMove();
                
            }               
            else
            {
                GameObject.Destroy(lastFire.gameObject);
                lastFire = null;
                if(gameStep != GameStep.End)
                {
                    gameStep = GameStep.Start;
                    checkOver();
                }
                
            }
        });          
    }

    public void faguang()
    {
        audioSource.PlayOneShot(lightClip);
        checkOver();
    }


    public void checkOver()
    {
        if (gameStep == GameStep.End) return;
       
        int highlightCount = 0;
        for(int i = 1;i<= levelLightCount[curLevel - 1]; i++)
        {
            if(levelTrans.Find(string.Format("Light_{0}/spHighlight", i )).gameObject.activeSelf)
            {
                highlightCount += 1;
            }
        }
        if(highlightCount == levelLightCount[curLevel - 1])
        {
            StartCoroutine(gameOver(true));
        }
        else if(fireTransList.Count < 1 && shootTimes == levelFireList[curLevel - 1].Length)
        {
            StartCoroutine(gameOver(false));

        }
    }

    IEnumerator gameOver(bool isWin)
    {
        gameStep = GameStep.End;
       
        menuLayer.gameObject.SetActive(false);
        if (isWin)
        {
            if (curLevel + 1 > maxLevel)
            {
                maxLevel = curLevel + 1;
                if (maxLevel > totalLevel)
                    maxLevel = totalLevel;
                PlayerPrefs.SetInt("maxLevel", maxLevel);
            }
            int lastCount = PlayerPrefs.GetInt(string.Format("starLevel{0}", curLevel), 0);
            int curStar = 0;
            if (shootTimes <= 2)
                curStar = 3;
            else if (shootTimes == 3)
                curStar = 2;
            else
                curStar = 1;
            if (curStar > lastCount)
                PlayerPrefs.SetInt(string.Format("starLevel{0}", curLevel), curStar);
        }

        yield return new WaitForSeconds(1f);
        if (lastFire != null)
        {
            lastFire.DOKill();
            GameObject.Destroy(lastFire.gameObject);
            lastFire = null;
        }
        clear();
        resultLayer.gameObject.SetActive(true);
        resultLayer.transform.localScale = Vector3.zero;
        //displayResultLayer(true);
        resultLayer.gameObject.SetActive(true);
        audioSource.Stop();
        if (isWin)
        {
            audioSource.PlayOneShot(winClip);
            //audioSource.PlayOneShot(winClip);
            //resultWinLayer.gameObject.SetActive(true);
            resultAnim(resultWinLayer);
            
        }
        else
        {
            audioSource.PlayOneShot(loseClip);
            //audioSource.PlayOneShot(loseClip);
            //resultLoseLayer.gameObject.SetActive(true);
            resultAnim(resultLoseLayer);
        }
        yield return new WaitForSeconds(2.4f);
        audioSource.Play();
    }

    void updateMenu()
    {
        for (int i = 1; i <= maxLevel; i++)
        {
            Transform cell = menuCellRoot.GetChild(i - 1);
            cell.Find("spSelect").gameObject.SetActive(i == curLevel);
            cell.Find("lblLevel").GetComponent<Text>().text = i.ToString();
            cell.Find("spLock").gameObject.SetActive(false);
            int starCount = PlayerPrefs.GetInt(string.Format("starLevel{0}", i), 0);
            for (int j = 0; j < starCount; j++)
            {
                cell.Find(string.Format("star_{0}/spHighlight", j + 1)).gameObject.SetActive(true);
            }
            Button btn = cell.GetComponent<Button>();
            btn.onClick.RemoveAllListeners();
            int tmpLevel = i;
            btn.onClick.AddListener(() =>
            {
                curLevel = tmpLevel;
                gameStart();
            });
        }
    }

    public void onBtnClick(string name)
    {
        audioSource.PlayOneShot(clip);
        if (name == "btnMenu")
        {
            displayResultLayer(false);
            menuLayer.gameObject.SetActive(true);
            menuLayer.transform.localScale = Vector3.zero;
            menuLayer.transform.DOScale(Vector3.one, 0.3f);
            updateMenu();
        }else if (name == "btnVolume")
        {
            Game.volumOpen = !Game.volumOpen;
            audioSource.volume = Game.volumOpen ? 1 : 0;
            btnVolume.SetActive(Game.volumOpen);// transform.Find("spDisable").gameObject.SetActive(!Game.volumOpen);
            spDisable.gameObject.SetActive(!Game.volumOpen);
        }
        else if (name == "btnRestart")
        {
            displayResultLayer(false);
            gameStart();
        }
        else if (name == "btnNext")
        {
            displayResultLayer(false);
            curLevel += 1;
            if (curLevel > totalLevel)
                curLevel = 1;
            gameStart();
        }else if (name == "btnHome")
        {
            SceneManager.LoadSceneAsync("LoginScene");
        }
    }

    void displayResultLayer(bool isShow)
    {
        Vector3 targetScale = isShow ? Vector3.one : Vector3.zero;
        if (!resultLayer.activeSelf) return;
        resultLayer.transform.DOScale(targetScale, 0.3f).OnComplete(() =>
        {
            if (!isShow)
            {
                resultWinLayer.gameObject.SetActive(false);
                resultLoseLayer.gameObject.SetActive(false);
                resultLayer.gameObject.SetActive(false);
            }
        });
    }

    void resultAnim(Transform layer)
    {
        resultLayer.transform.localScale = Vector3.one;
        layer.gameObject.SetActive(true);
        layer.transform.localScale = Vector3.zero;
        layer.transform.DOScale(Vector3.one, 0.3f);
    }
}
