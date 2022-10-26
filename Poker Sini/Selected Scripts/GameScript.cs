using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Unity.Mathematics.math;
using static UnityEngine.Diagnostics.Utils;
using Photon.Pun;
using UnityEngine.Audio;
using Photon.Realtime;
using ExitGames.Client.Photon.StructWrapping;
using TMPro;

namespace PokerSini
{
    public class GameScript : MonoBehaviour
    {

        public class card : IComparable<card>
        {
            private Sprite face;
            private Sprite back = backall;
            private string suit;
            private int num;
            private int vis;
            private PokerSini.selectable prefab;


            //inisiate a card
            //card(string s, int n, Sprite[] df, int vis = 0)

            public void Create(string s, int n, Sprite[] df, int vis = 0)
            {
                int c = 0;
                this.suit = s;
                this.num = n;
                this.vis = vis; //initiate to deck
                if (n == 14)
                    c = 0;
                else
                    c = n - 1;
                if (s == "H")
                    c += 13;
                if (s == "D")
                    c += 26;
                if (s == "S")
                    c += 39;
                print("num: " + n.ToString() + "suit: " + s + "and c: " + c.ToString());
                this.face = df[c];
            }
            public Sprite Getface()
            {
                return this.face;
            }

            public selectable GetPrefab()
            {
                return this.prefab;
            }

            public Sprite GetBack()
            {
                return this.back;
            }

            public int GetNum()
            {
                return this.num;
            }
            public string GetSuit()
            {
                return this.suit;
            }
            public int GetVis()
            {
                return this.vis;
            }
            public void SetVis(int vis)
            {
                this.vis = vis;
            }
            public void SetPrefab(selectable p)
            {
                this.prefab = p;
            }
            public void SetFace(Sprite v)
            {
                this.face = v;
            }
            public void SetBack(Sprite v)
            {
                this.back = v;
            }
            public int CompareTo(card other)
            {
                return other.num - num;
            }
        }
        //sound
        public Sprite[] SoundLogos;
        public GameObject SoundButton;



        public class player
        {
            private int id;
            public GameObject TextName;
            public GameObject Port;
            private int playerPhoto = 0;
            private string playerName = "Bot";
            public GameScript manager;



            public int GetPhoto()
            {
                return playerPhoto;
            }
            public string GetName()
            {
                return playerName;
            }
            public int GetID()
            {
                return id;
            }
            public void SetName(string name)
            {
                playerName = name;

            }
            public void SetID(int i)
            {
                id = i;

            }
            public void SetPhoto(int i)
            {
                playerPhoto = i;
            }
        }
        public player playerA;
        public player playerB;
        //public Text TextNameOfA;
        //public Text TextNameOfB;
        public TMP_Text TextNameOfA;
        public TMP_Text TextNameOfB;

        public GameObject PortOfA;
        public GameObject PortOfB;
        public Sprite[] Portraits;

        private bool leave = false;
        public string typeofgame;
        bool same = true;
        bool sameB = true;

        GameObject GameRate;

        public int Menuflag = 0;
        public string[] CollectionNames = { "h", "p", "pp", "t", "s", "Flush", "Full", "Four", "sf" };
        public bool online = false;
        public GameObject cardprefab;
        public GameObject rateprefab;
        public GameObject ratePrefabBackUp;
        public int counter;
        public Sprite[] Rates;
        public static string[] suits = { "S", "D", "H", "C" };
        public static int[] num = { 14, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 };
        public static Sprite backall;
        public GameObject[] Acards;
        public GameObject[] Bcards;
        public int[] Ascores = new int[5];
        public int[] Bscores = new int[5];
        public int[] winScore = new int[5];
        public int k = -1;
        public GameObject DealCard;
        public GameObject left;

        public Sprite[] CardsBack;

        int whowon;

        public Color[] col;
        public Color[] colLevel;

        //counter
        public int[] amount = new int[17];

        public string deckstring;

        //
        public int Lose;
        public int room;
        public int Win;
        private bool inplay;

        public bool shuf = false;
        public bool paid;


        public bool printed = false;
        public bool start = false;
        public int whostarts;
        public bool host;
        public GameObject BackButton;

        public GameObject dragMethod;
        public GameObject manager;
        public GameObject Obj;
        public GameObject deckcard;
        public GameObject deckbutton;
        public List<card> Deck;
        public Sprite[] DeckFace;
        public card[][] PA;
        public card[][] PB;

        public GameObject BackPopover;

        //counter
        public GameObject Popover;
        public GameObject PurchPopover;
        public GameObject PurchButton;
        public Text PurchCounterText;
        public GameObject CountButton;
        public double[] CounterPrices = { 0.2, 0.25, 0.3334, 0.5, 0.5 };
        public Text PlayerCoins;
        public Text CurrPlayerCanvasPrice;
        public GameObject CloseButton;

        public selectable[] flipCards;
        public GameObject RevealButton;
        bool leftb = false;

        int[] a = new int[5];
        int[] b = new int[5];

        GameObject AR;

        public SpriteRenderer[,,] matrix = new SpriteRenderer[2, 5, 5];

        protected card[] a1 = new card[5];
        protected card[] a2 = new card[5];
        protected card[] a3 = new card[5];
        protected card[] a4 = new card[5];
        protected card[] a0 = new card[5];
        protected card[] b1 = new card[5];
        protected card[] b2 = new card[5];
        protected card[] b3 = new card[5];
        protected card[] b4 = new card[5];
        protected card[] b0 = new card[5];

        public GameObject Timer;
        private int Tim1 = 15;
        private int Tim2 = 15;
        public GameObject xButton;

        //AutoCard
        //private bool gogo = true;
        float flip = 0f;
        public Sprite whitePop;
        public GameObject Grid;
        public GameObject toggle;
        public GameObject Logo;
        public GameObject Canvas;


        public Sprite GetBackAll()
        {
            return backall;
        }


        [PunRPC]
        public void UpdateGameStats(int ws, int cnt)
        {
            GridMe();
            whostarts = ws;
            counter = cnt;
        }

        public IEnumerator CloseCard(GameObject Popover)
        {
            for (float i = 0f; i <= 90f; i += 10f)
            {
                Popover.transform.rotation = Quaternion.Euler(0f, i, flip);
                yield return new WaitForSeconds(0.01f);

            }
           
            Popover.SetActive(false);

        }
        public IEnumerator OpenCard(GameObject Popover)
        {
            Popover.transform.rotation = Quaternion.Euler(0f, 90f, flip);
            Popover.SetActive(true);

            for (float i = 90f; i >= 0f; i -= 10f)
            {
                Popover.transform.rotation = Quaternion.Euler(0f, i, flip);
                yield return new WaitForSeconds(0.01f);

            }

        }






        private void Update()
        {


            if (counter > 52)
                print("rateprefab" + rateprefab);
            if (leave)
                SceneManager.LoadScene(0);
            if (typeofgame != "Tour" && typeofgame != "Practice" && counter < 42 & counter > 40 && PlayerPrefs.GetInt("PlayerCoins") != PlayerPrefs.GetInt("LoseScore"))
                PlayerPrefs.SetInt("PlayerCoins", PlayerPrefs.GetInt("LoseScore"));
            if (online && PhotonNetwork.PlayerList.Length < 2 & !leftb & inplay & PlayerPrefs.GetInt("DidILeft") == 0)
            {
                left.SetActive(true);
                inplay = false;
                if (!host)
                    left.transform.Rotate(new Vector3(left.transform.rotation.x, left.transform.rotation.y, 180f));
                leftb = true;
                PlayerPrefs.SetInt("PlayerCoins", PlayerPrefs.GetInt("WinScore"));
            }
            if (online && DealCard == null && deckbutton.GetComponent<clicked>().click && PB[4][0] != null && shuf)
            {
                int t;
                deckbutton.GetComponent<clicked>().click = false;
                getcard();


                //Color c = DealCard.GetComponent<Image>().color;
                //c.a = 150f;
                //DealCard.GetComponent<Image>().color = c;
                if (counter > 40 && whostarts == 0)
                    DealCard.GetComponent<SpriteRenderer>().sprite = DealCard.GetComponent<PlayerScript>().cardback;

                if (whostarts == 1 && (t = DragCounter(dragMethod.GetComponent<Snapcontroller>().dragindA)) != -1)
                {
                    DealCard.GetComponent<selectable>().move = false;
                    StartCoroutine(LastCTrans(t, whostarts));
                }
                else if(whostarts == 0 && (t = DragCounter(dragMethod.GetComponent<Snapcontroller>().dragindB)) != -1 && online)
                {
                    DealCard.GetComponent<selectable>().move = false;
                    Timer.GetComponent<TimerScript>().remainingDuration = 0;
                    DealCard.GetComponent<PhotonView>().RPC("MoveFalse", RpcTarget.Others);
                    
                    //DealCard.GetComponent<PhotonView>().RPC("AllocateCard", RpcTarget.Others, t+5);
                }

            }


        }

        //punishment rule
        public IEnumerator LastCTrans(int t, int ws = 1)
        {
            for (int i = 0; i < 2; i++)
            {
                yield return new WaitForSeconds(0.5f);
                if (i == 1)
                {
                    DealCard.GetComponent<selectable>().TransformCardPos(dragMethod.GetComponent<Snapcontroller>().snapPoints[t+((1-ws)*5)], t);
                }

            }
        }


        public int DragCounter(object[] drag)
        {
            int count = 0;
            int d = -1;
            for (int i = 5; i < 10; i++)
            {
                if ((bool)drag[i] == true)
                {
                    count++;
                    d = i - 5;
                }
            }
            if (count == 1)
                return d;
            else
                return -1;
        }


        public void UpdateCount(card c)
        {
            amount[c.GetNum()] -= 1;
            if (c.GetSuit() == "D")
                amount[0] -= 1;
            if (c.GetSuit() == "H")
                amount[1] -= 1;
            if (c.GetSuit() == "S")
                amount[15] -= 1;
            if (c.GetSuit() == "C")
                amount[16] -= 1;
        }
        public void InitAmount()
        {
            for (int i = 2; i < 15; i++)
                amount[i] = 4;
            amount[0] = 13;
            amount[1] = 13;
            amount[15] = 13;
            amount[16] = 13;
            for (int i = 0; i < 5; i++)
            {
                if (PA[0][i] == null)
                    Debug.Log("Here");
                amount[PA[i][0].GetNum()] -= 1;
                amount[PB[i][0].GetNum()] -= 1;

                if (PB[i][0].GetSuit() == "D")
                    amount[0] -= 1;
                if (PB[i][0].GetSuit() == "H")
                    amount[1] -= 1;
                if (PB[i][0].GetSuit() == "S")
                    amount[15] -= 1;
                if (PB[i][0].GetSuit() == "C")
                    amount[16] -= 1;

                if (PA[i][0].GetSuit() == "D")
                    amount[0] -= 1;
                if (PA[i][0].GetSuit() == "H")
                    amount[1] -= 1;
                if (PA[i][0].GetSuit() == "S")
                    amount[15] -= 1;
                if (PA[i][0].GetSuit() == "C")
                    amount[16] -= 1;

            }
        }


        public Sprite[] GetDeckFace()
        {
            return DeckFace;
        }

        public string[] DeckToString(List<card> deckf)
        {
            string[] ret = new string[52];
            for (int i = 0; i < 52; i++)
            {
                ret[i] = deckf[i].GetNum().ToString() + deckf[i].GetSuit();
            }
            return ret;
        }
        protected void Awake()
        {
            if (PlayerPrefs.GetInt("DidILeft") == 1)
                SceneManager.LoadScene(0);
            playerA = new player();
            playerB = new player();
            playerA.SetID(0);
            playerB.SetID(1);

            if (typeofgame != "Tour")
                PlayerPrefs.SetInt("TourCollect", 0);

            //    foreach (Sound s in Sounds)
            //    {
            //        s.source = gameObject.AddComponent<AudioSource>();
            //        s.source.clip = s.clip;
            //        s.source.loop = s.loop;
            //        s.source.volume = s.volume;
            //  s.source.outputAudioMixerGroup = mixerGroup;
            //    }
        }

        public void OnCancel()
        {
            StartCoroutine(CloseCard(PurchPopover));
            //PurchPopover.SetActive(false);
            CloseButton.SetActive(false);
            PurchButton.SetActive(true);

        }
        public void OnNotCancel()
        {
            Debug.Log("Fuck");
            if (!paid)
            {
                StartCoroutine(OpenCard(PurchPopover));
                //PurchPopover.SetActive(true);
                CloseButton.SetActive(true);
                PurchButton.SetActive(false);
                if (typeofgame != "Tour")
                {
                    CurrPlayerCanvasPrice.text = "Current Price: " + (CounterPrices[(counter / 10) - 1] * PlayerPrefs.GetInt("RoomEnteryFee")).ToString();
                }
                else
                {
                    CurrPlayerCanvasPrice.text = "Current Price: " + (CounterPrices[(counter / 10) - 1] * PlayerPrefs.GetInt("RoomTourEnteryFee")).ToString();
                }

            }
        }

        public void OnPurch()
        {
            int price = 0;
            if (typeofgame != "Tour")
            {
                price = (int)(CounterPrices[(counter / 10) - 1] * PlayerPrefs.GetInt("RoomEnteryFee"));
            }
            else
            {
                price = (int)(CounterPrices[(counter / 10) - 1] * (PlayerPrefs.GetInt("RoomTourEnteryFee") / 10));
            }
            int playercoins = PlayerPrefs.GetInt("PlayerCoins");
            if (PlayerPrefs.GetInt("PlayerCoins") >= price)
            {
                StartCoroutine(Money(playercoins, playercoins - price, -1));
                CountButton.SetActive(true);
                PurchButton.SetActive(false);
                paid = true;
            }
        }
        public string ToStringConvertor(double n)
        {
            string ret;
            double k;
            if (n > 1000000)
            {
                k = n / 1000000.0;

                ret = k.ToString("#.##") + "M";
            }

            else if (n > 1000)
            {
                k = n / 1000.0;
                ret = k.ToString("#.##") + "K";
            }

            else
                ret = n.ToString();
            return ret;
        }

        public IEnumerator Money(int premoney, int postmoney, int pm)
        {
            Debug.Log("PreMoney: " + premoney + " PostMoney: " + postmoney);
            int c = Mathf.Abs(postmoney - premoney) / 100;
            for (int i = 0; i < 100; i++)
            {
                yield return new WaitForSeconds(0.0001f);
                PlayerCoins.text = ToStringConvertor(premoney);
                premoney += c * pm;
            }

            PlayerCoins.text = ToStringConvertor(postmoney);
            PlayerPrefs.SetInt("PlayerCoins", postmoney);
            StartCoroutine(CloseCard(PurchPopover));
            //PurchPopover.SetActive(false);
            CloseButton.SetActive(false);

        }



        [PunRPC]
        public void UpdatePrefs(string s, int i)
        {
            Debug.Log("InUpdatePrefs");
            int j = PlayerPrefs.GetInt("PlayerPhoto");
            UpdateCoins();
            if (PhotonNetwork.IsMasterClient)
            {

                TextNameOfA.text = PlayerPrefs.GetString("PlayerName");
                PortOfA.GetComponent<SpriteRenderer>().sprite = Portraits[j];
                TextNameOfB.text = s;
                PortOfB.GetComponent<SpriteRenderer>().sprite = Portraits[i];
            }
            else
            {
                TextNameOfB.text = PlayerPrefs.GetString("PlayerName");
                PortOfB.GetComponent<SpriteRenderer>().sprite = Portraits[j];
                TextNameOfA.text = s;
                PortOfA.GetComponent<SpriteRenderer>().sprite = Portraits[i];
                TextNameOfA.transform.Rotate(0f, 0f, 180f);
                TextNameOfB.transform.Rotate(0f, 0f, 180f);
                BackButton.transform.Rotate(0f, 0f, 180f);
                Logo.transform.Rotate(0f, 0f, 180f);
                Timer.transform.Rotate(0f, 0f, 180f);
                xButton.transform.position = new Vector3(deckbutton.transform.position.x-25f, PortOfA.transform.position.y, 0);
                PortOfB.transform.Rotate(0f, 0f, 180f);
                PortOfA.transform.Rotate(0f, 0f, 180f);
                BackPopover.transform.Rotate(0f, 0f, 180f);
                manager.GetComponent<PhotonView>().RPC("UpdatePrefs", RpcTarget.Others, TextNameOfB.text, j);
            }
        }


        private void Start()
        {

            BackButton.SetActive(false);
            BackPopover.SetActive(false);
            paid = false;
            //BackPopover.SetActive(false);
            OnSound();
            if (online)
            {
                RevealButton.SetActive(false);
                left.SetActive(false);

            }

            Debug.Log("Start On My device");
            if (PhotonNetwork.IsConnected)
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    online = true;
                    host = true;
                    //SyncSnapEnv();
                    //manager.GetComponent<PhotonView>().RPC("UpdateProfile", RpcTarget.Others, playerA.GetName(), playerA.GetPhoto());
                }
                else
                {
                    PhotonNetwork.OfflineMode = true;
                    online = true;
                    host = false;
                    flip = 180f;
                    //BackPopover.transform.rotation = new Quaternion(0, 0, 180f, 0);
                   //BackPopover.transform.Rotate(new Vector3(0, 0, 180f));
                    Camera.main.transform.Rotate(new Vector3(Camera.main.transform.rotation.x, Camera.main.transform.rotation.y, 180f));
                    //Camera.main.transform.rotation.Set(Camera.main.transform.rotation.x, Camera.main.transform.rotation.y, 180f, Camera.main.transform.rotation.w);
                }
            }
            else
            {
                online = false;
                host = false;




            }
            //         if (online)
            //         {
            //             Snapcontroller sp = dragMethod.GetComponent<Snapcontroller>();
            //             for (int i = 0; i < sp.snapPoints.Length; i++)
            //             {
            //                 SyncSnap(i, (float)sp.snapPoints[i].transform.position.x, (float)sp.snapPoints[i].transform.position.y, (float)sp.snapPoints[i].transform.position.z);
            //                 manager.GetComponent<PhotonView>().RPC("SyncSnap", RpcTarget.Others, new object[] { i, (float)sp.snapPoints[i].transform.position.x, (float)sp.snapPoints[i].transform.position.y, (float)sp.snapPoints[i].transform.position.z });
            //            }
            //        }

            StartGame();
            GridMe();
           

        }


      


        
        public void OnSound()
        {
            if (PlayerPrefs.GetInt("so") == 1)
            {
                SoundButton.GetComponent<Image>().sprite = SoundLogos[1];
                AudioListener.pause = true;
            }
            else
            {
                AudioListener.pause = false;
                SoundButton.GetComponent<Image>().sprite = SoundLogos[0];
            }
        }
        public void OnToggleTaggled()
        {
            PlayerPrefs.SetInt("HexaGame", 1 - PlayerPrefs.GetInt("HexaGame"));
            GridMe();

        }


        public void GridMe()
        {
            int num = PlayerPrefs.GetInt("RoomNum") * 2;
            Canvas.GetComponent<Image>().sprite = whitePop;
            if (PlayerPrefs.GetInt("HexaGame") == 1)
            {
                for (int i = 0; i < 6; i++)
                {
                    col[i * 2] = colLevel[num];
                    col[(i * 2) + 1] = colLevel[num + 1];
                }
                Grid.SetActive(true);
                Logo.SetActive(false);
                Canvas.GetComponent<Image>().sprite = whitePop;
                toggle.GetComponent<Image>().color = Color.white;

            }
            else
            {
                Grid.SetActive(false);
                Logo.SetActive(true);
                toggle.GetComponent<Image>().color = Color.grey;
                Canvas.GetComponent<Image>().color = colLevel[num];
                Logo.GetComponent<Image>().color = colLevel[num + 1];
        }
        }

        // Start is called before the first frame update

        public void UpdateCoins()
        {
            int c = PlayerPrefs.GetInt("PlayerCoins");
            int w = PlayerPrefs.GetInt("RoomEnteryFee");
            PlayerPrefs.SetInt("WinScore", c + w);
            PlayerPrefs.SetInt("LoseScore", c - w);
            Debug.Log("WinScore is " + (c + w) + "LoseScore is " + (c - w));
        }


        [PunRPC]
        private void TimerOnlineUpdate(int Duration, int remainingDuration, int uif)
        {
            TimerScript ts = Timer.GetComponent<TimerScript>();
            ts.uiFill.fillAmount = Mathf.InverseLerp(0, Duration, remainingDuration);
            ts.uiFill.GetComponent<Image>().sprite = ts.sprites[uif];
            ts.uiLogo.GetComponent<Image>().sprite = ts.logos[uif];
            if (whostarts == 1)
            {
                ts.uiFill.GetComponent<Image>().color = Color.grey;
                ts.uiLogo.GetComponent<Image>().color = Color.grey;
            }
            else
            {
                ts.uiFill.GetComponent<Image>().color = Color.white;
                ts.uiLogo.GetComponent<Image>().color = Color.white;
            }

        }


        public void StartGame()
        {
            inplay = true;
            if (!online)
            {
                //CounterShit
                if (typeofgame != "Practice" && typeofgame != "Online")
                {
                    PlayerCoins.text = ToStringConvertor(PlayerPrefs.GetInt("PlayerCoins"));
                }

                UpdateCoins();
                playerA.SetName(PlayerPrefs.GetString("PlayerName"));
                playerA.SetPhoto(PlayerPrefs.GetInt("PlayerPhoto"));
                PortOfA.GetComponent<SpriteRenderer>().sprite = Portraits[playerA.GetPhoto()];
                TextNameOfA.text = playerA.GetName();
                if (typeofgame == "Tour")
                {
                    playerB.SetPhoto(PlayerPrefs.GetInt("GameOp"));

                }
                else
                {
                    System.Random random = new System.Random();
                    int k = random.Next(0, 7);
                    playerB.SetPhoto(k);
                }
                playerB.SetName(Portraits[playerB.GetPhoto()].name);
                PortOfB.GetComponent<SpriteRenderer>().sprite = Portraits[playerB.GetPhoto()];
                TextNameOfB.text = playerB.GetName();
            }
            else if (host)
            {
                playerA.SetName(PlayerPrefs.GetString("PlayerName"));
                playerA.SetPhoto(PlayerPrefs.GetInt("PlayerPhoto"));
                TextNameOfA.text = playerA.GetName();
                PortOfA.GetComponent<SpriteRenderer>().sprite = Portraits[playerA.GetPhoto()];
                manager.GetComponent<PhotonView>().RPC("UpdatePrefs", RpcTarget.Others, playerA.GetName(), playerA.GetPhoto());

            }

            if(typeofgame != "Practice")
            {
                backall = CardsBack[PlayerPrefs.GetInt("RoomNum")];
                cardprefab.GetComponent<PlayerScript>().cardback = CardsBack[PlayerPrefs.GetInt("RoomNum")];
            }


            flipCards = new selectable[10];
            counter = 9;
            PA = new card[][] { a0, a1, a2, a3, a4 };
            PB = new card[][] { b0, b1, b2, b3, b4 };

            if ((online & host) || (!online))
            {
                Deck = GenDeck(DeckFace);
                Shuffle(Deck);
                sortdeal();
                if (online)
                {
                    manager.GetComponent<PhotonView>().RPC("UpdateGameStats", RpcTarget.Others, whostarts, counter);
                    //manager.GetComponent<PhotonView>().RPC("UpdateProfile", RpcTarget.Others, playerA.GetName(), playerA.GetPhoto());

                }
            }
            InitAmount();




            //changed ---
            //whostarts = checkstart();
            // if (whostarts == 0)
            //  {
            //      print("player A will start");
            //  }
            //   if (whostarts == 0)
            //     print("player B will start");
        }
        public void SyncSnapEnv()
        {
            Snapcontroller sp = dragMethod.GetComponent<Snapcontroller>();
            for (int i = 0; i < 10; i++)
            {
                manager.GetComponent<PhotonView>().RPC("SyncSnapoints", RpcTarget.Others, new object[] { i, sp.transform.position.x, sp.snapPoints[i].transform.position.y, sp.snapPoints[i].transform.position.z });
            }
        }

        [PunRPC]
        public void SyncSnapoints(int i,float x,float y, float z)
        {
            dragMethod.GetComponent<Snapcontroller>().snapPoints[i].transform.position = new Vector3(x, y, z);
        }

        [PunRPC]
        public void SyncSnap(int k, float x, float y, float z)
        {
            print("before " + dragMethod.GetComponent<Snapcontroller>().snapPoints[k].transform.position.y);
            dragMethod.GetComponent<Snapcontroller>().snapPoints[k].transform.position = new Vector3(x, y, z);
            print("after " + dragMethod.GetComponent<Snapcontroller>().snapPoints[k].transform.position.y);
            if (k < 5)
                Acards[k].transform.position = new Vector3(x, y, z);
            else
                Bcards[k - 5].transform.position = new Vector3(x, y, z);

        }


        public int checkstart()
        {
            for (int j = 0; j < PA.Length; j++)
            {
                if (PA[j][0].GetNum() == 1)
                    a[j] = 14;
                else
                {
                    a[j] = PA[j][0].GetNum();
                }
                if (PB[j][0].GetNum() == 1)
                    b[j] = 14;
                else
                {
                    b[j] = PB[j][0].GetNum();
                }

            }
            Array.Sort(a);
            Array.Sort(b);
            for (int i = 4; i > -1; i--)
            {
                print("a " + i + " " + a[i] + " b " + i + " " + b[i]);
                if (a[i] > b[i])
                    return 0;

                if (b[i] > a[i])
                    return 1;
            }
            return 0;
        }


        [PunRPC]
        public void UpdateSnap(int k, int ws)
        {
            Snapcontroller sp = dragMethod.GetComponent<Snapcontroller>();
            Debug.Log("UpdateSnap " + k + "value: " + (sp.snapPoints[k + 5].transform.position.y + 40));
            if (ws == 0)
                sp.snapPoints[k].transform.position = new Vector3(sp.snapPoints[k].transform.position.x, sp.snapPoints[k].transform.position.y - 40, sp.snapPoints[k].transform.position.z);
            else
            {
                sp.snapPoints[k + 5].transform.position = new Vector3(sp.snapPoints[k + 5].transform.position.x, sp.snapPoints[k + 5].transform.position.y + 40, sp.snapPoints[k + 5].transform.position.z);
                Debug.Log("UpdateSnap " + k + "value: " + (sp.snapPoints[k + 5].transform.position.y));
            }


        }

        // Update is called once per frame

        public static List<card> GenDeck(Sprite[] df)
        {
            List<card> nd = new List<card>();

            foreach (string s in suits)
            {
                foreach (int d in num)
                {
                    card tmp = new card();
                    tmp.Create(s, d, df);
                    nd.Add(tmp);
                }
            }
            return nd;

            // List<string> nd = new List<string>();
            //foreach(string s in suits)
            // {
            //      foreach(string d in num)
            //      {
            //          nd.Add(d + s);
            //      }
            //  }
            //  return nd;
        }



        public IEnumerator FlipCard(int i)
        {
            for (float j = 180f; j >= 0f; j -= 10f)
            {
                flipCards[i].transform.rotation = Quaternion.Euler(0f, j, 0f);
                if (j == 90)
                {
                    flipCards[i].GetComponent<SpriteRenderer>().sprite = flipCards[i].GetComponent<PlayerScript>().cardface;
                    flipCards[i].tmpfliped = false;
                }
                yield return new WaitForSeconds(0.01f);

            }
        }


        [PunRPC]
        public void clientRev()
        {
            OnRevealClicked();
        }

        public void OnRevealClicked()
        {
            print("Im here");
            RevealButton.SetActive(false);
            inplay = false;
            //left.GetComponent<SpriteRenderer>().sprite = null;

            leftb = true;
            StartCoroutine(Reveal(flipCards, 1f));
            StartCoroutine(SlideLoser(2f));
            for (int i = 0; i < 5; i++)
            {
                print("PA: " + PA[i][0].GetNum() + PA[i][0].GetSuit() + " " + PA[i][1].GetNum() + PA[i][1].GetSuit() + " " + PA[i][2].GetNum() + PA[i][2].GetSuit() + " " + PA[i][3].GetNum() + PA[i][3].GetSuit() + " " + PA[i][4].GetNum() + PA[i][4].GetSuit() + " ");
                print("PB: " + PB[i][0].GetNum() + PB[i][0].GetSuit() + " " + PB[i][1].GetNum() + PB[i][1].GetSuit() + " " + PB[i][2].GetNum() + PB[i][2].GetSuit() + " " + PB[i][3].GetNum() + PB[i][3].GetSuit() + " " + PB[i][4].GetNum() + PB[i][4].GetSuit() + " ");
            }
            int win = CheckWinGame(PA, PB);
        }



        public void getcard()
        {
            counter += 1;
            int price = 0;
            print("---------------------" + rateprefab.name + "------------------");
            if (typeofgame != "Tour")
            {
                price = (int)(CounterPrices[(counter / 10) - 1] * PlayerPrefs.GetInt("RoomEnteryFee"));
            }
            else
            {
                price = (int)(CounterPrices[(counter / 10) - 1] * (PlayerPrefs.GetInt("RoomTourEnteryFee") / 10));
            }
            if (PlayerPrefs.GetInt("PlayerCoins") < price && typeofgame != "Online")
                PurchButton.SetActive(false);
            if (counter > 52)
            {
                if (host && online)
                {
                    ///STOPPED HERE. 
                    //RevealButton.SetActive(true);
                    OnRevealClicked();
                    return;
                }
                else if (!online)
                {
                    print("Im here");
                    StartCoroutine(Reveal(flipCards, 1f));
                    StartCoroutine(SlideLoser(2f));
                    for (int i = 0; i < 5; i++)
                    {
                        print("PA: " + PA[i][0].GetNum() + PA[i][0].GetSuit() + " " + PA[i][1].GetNum() + PA[i][1].GetSuit() + " " + PA[i][2].GetNum() + PA[i][2].GetSuit() + " " + PA[i][3].GetNum() + PA[i][3].GetSuit() + " " + PA[i][4].GetNum() + PA[i][4].GetSuit() + " ");
                        print("PB: " + PB[i][0].GetNum() + PB[i][0].GetSuit() + " " + PB[i][1].GetNum() + PB[i][1].GetSuit() + " " + PB[i][2].GetNum() + PB[i][2].GetSuit() + " " + PB[i][3].GetNum() + PB[i][3].GetSuit() + " " + PB[i][4].GetNum() + PB[i][4].GetSuit() + " ");
                    }
                    int win = CheckWinGame(PA, PB);
                    return;
                }

            }
            card c = GetCardFromDeck();




            cardprefab.GetComponent<PlayerScript>().cardface = c.Getface();
            GameObject dealcard = new GameObject();
            if (online)
            {
                dealcard = PhotonNetwork.InstantiateRoomObject(cardprefab.name, new Vector3(deckcard.transform.position.x, deckcard.transform.position.y, deckcard.transform.position.z), Quaternion.identity);
                if (whostarts == 0 && counter > 40)
                    dealcard.GetComponent<SpriteRenderer>().sprite = c.GetBack();
                dealcard.GetComponent<PhotonView>().RPC("UpdateFace", RpcTarget.All, new object[] { c.GetNum(), c.GetSuit(), whostarts, 1 });
                Debug.Log("Photon instantiate a card");
                if (whostarts == 1)
                {
                    dealcard.GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.PlayerList[1]);
                    dealcard.GetComponent<PhotonView>().RPC("MoveIt", RpcTarget.All);


                }




            }
            else
            {
                dealcard = Instantiate(cardprefab, new Vector3(deckcard.transform.position.x, deckcard.transform.position.y, deckcard.transform.position.z), Quaternion.identity, deckcard.transform);
            }
            dealcard.name = (c.GetNum().ToString()) + c.GetSuit();
            print((c.GetNum().ToString()) + c.GetSuit());
            if (whostarts == 0 || online || typeofgame == "Practice")
            {
                dealcard.GetComponent<selectable>().move = true;

            }

            dealcard.GetComponent<SpriteRenderer>().sortingOrder += counter;
            dealcard.GetComponent<PlayerScript>().cardface = c.Getface();
            dragMethod.GetComponent<Snapcontroller>().draggableObj[0] = dealcard.GetComponent<selectable>();
            dealcard.GetComponent<selectable>().dragMethod = dragMethod;
            dealcard.GetComponent<selectable>().deckbutton = deckbutton;
            deckbutton.GetComponent<clicked>().click = false;
            dealcard.GetComponent<selectable>().manager = manager;
            dealcard.GetComponent<selectable>().c = c;


            if (whostarts == 0)
            {
                whostarts = 1;
                dealcard.GetComponent<selectable>().ws = 0;
                print("A turn");
            }
            else
            {
                whostarts = 0;
                dealcard.GetComponent<selectable>().ws = 1;
                print("B turn");
                
            }
            DealCard = dealcard;
            if (counter > 50)
            {
                for (int i = 0; i < 5; i++)
                {
                    dragMethod.GetComponent<Snapcontroller>().dragindA[i + 5] = true;
                    dragMethod.GetComponent<Snapcontroller>().dragindB[i + 5] = true;
                }             
                Debug.Log("HereBUG");

            }
            if (counter % 10 == 1)
            {
                if(online)
                    manager.GetComponent<PhotonView>().RPC("dragIndRes", RpcTarget.Others);
                for (int i = 0; i < 5; i++)
                {
                    dragMethod.GetComponent<Snapcontroller>().dragindA[i + 5] = true;
                    dragMethod.GetComponent<Snapcontroller>().dragindB[i + 5] = true;
                    print("PA: ");
                    for (int j = 0; j < (counter % 10); j++)
                    {
                        print(" PA: " + j + " " + PA[i][j]);
                        print(" PB: " + j + " " + PB[i][j]);
                    }
                }
            }
            if(online)
                manager.GetComponent<PhotonView>().RPC("UpdateGameStats", RpcTarget.Others, whostarts, counter);

        }


        [PunRPC]
        public void dragIndRes()
        {
            for (int i = 0; i < 5; i++)
            {
                dragMethod.GetComponent<Snapcontroller>().dragindA[i + 5] = true;
                dragMethod.GetComponent<Snapcontroller>().dragindB[i + 5] = true;
                dragMethod.GetComponent<Snapcontroller>().snapPoints[i + 5].transform.position = new Vector3(dragMethod.GetComponent<Snapcontroller>().snapPoints[i+5].transform.position.x, dragMethod.GetComponent<Snapcontroller>().snapPoints[i+5].transform.position.y + 40, dragMethod.GetComponent<Snapcontroller>().snapPoints[i+5].transform.position.z);
                Debug.Log("Update In dragIndRes " + k + " value " + dragMethod.GetComponent<Snapcontroller>().snapPoints[i + 5].transform.position.y);
            }
        }

        public card GetCardFromDeck()
        {
            card c =Deck.Last<card>();
            Deck.RemoveAt(Deck.Count - 1);

            if(typeofgame != "Practice")
                Timer.GetComponent<TimerScript>().Being(Tim1, Deck.Count);

            //Play("SortCard");
            return c;
        }
        




        public void Shuffle<T>(List<T> list)
        {
            System.Random random = new System.Random();
            int n = list.Count;
            while (n > 1)
            {
                int k = random.Next(n);
                n--;
                T temp = list[k];
                list[k] = list[n];
                list[n] = temp;
            }

        }
        //   void Deal()
        // {
        //   foreach(card c in Deck)
        // {
        //GameObject newcard = PhotonNetwork.Instantiate(cardprefab.name, transform.position, Quaternion.identity);
        //   GameObject newcard = Instantiate(cardprefab, transform.position, Quaternion.identity);
        // newcard.name = ((c.GetNum()).ToString())+c.GetSuit();
        //    }
        //  }
        //IEnumerator sortdeal(card[][] PA, card[][] PB)

        //[PunRPC]
        void sortdeal()
        {

            for (int i = 0; i < 5; i++)
            {
                //yield return new WaitForSeconds(0.1f);
                Addfirst(PA, Acards, i,0);
                Addfirst(PB, Bcards, i,1);


                a[i] = PA[i][0].GetNum();
                b[i] = PB[i][0].GetNum();
            }
            StartCoroutine(showcard(PA, Acards, 0.2f));
            StartCoroutine(showcard(PB, Bcards, 0.25f));
            Array.Sort(a);
            Array.Sort(b);
            print(a[0
                ] + " " + a[1] + " " + a[2] + " " + a[3] + " " + a[4]);
            counter = 10;
            whostarts = checkstart();
        }
        void Addfirst(card[][] P, GameObject[] cards, int i,int ab)
        {
            P[i][0] = Deck.Last<card>();
            ///here some matrix shit

            Debug.Log("it has changed befor");
            Deck.RemoveAt(Deck.Count - 1);

        }


        public IEnumerator showcard(card[][] P, GameObject[] cards, float fl)
        {

            for (int i = 0; i < 5; i++)
            {
                if(online)
                    manager.GetComponent<PhotonView>().RPC("SyncSnapoints", RpcTarget.Others, new object[] { i, cards[i].transform.position.x, cards[i].transform.position.x, cards[i].transform.position.x });
                yield return new WaitForSeconds(fl);
                //changed here ****
                PutCardInPlace(P, i, cards, fl);
                //Play("SortCard");
                if (fl == 0.25f && i == 4)
                    shuf = true;
            }
            
        }


        public void UpdatePrefab(int n,string s)
        {
            card c = new card();
            c.Create(s, n, DeckFace);
            Obj.GetComponent<PlayerScript>().cardface = c.Getface();
        }

        public void PutCardInPlace(card[][] P,int i, GameObject[] cards, float fl)
        {
            cardprefab.GetComponent<PlayerScript>().cardface = P[i][0].Getface();
            GameObject newcardA;
            if (online)
            {
                newcardA = PhotonNetwork.InstantiateRoomObject(cardprefab.name, new Vector3(cards[i].transform.position.x, cards[i].transform.position.y, cards[i].transform.position.z), Quaternion.identity);
                newcardA.GetComponent<PhotonView>().RPC("UpdateFace", RpcTarget.All, new object[] { P[i][0].GetNum(),P[i][0].GetSuit(),whostarts,0 });
                newcardA.GetComponent<PlayerScript>().cardface = P[i][0].Getface();
                newcardA.GetComponent<SpriteRenderer>().sortingOrder += i;

            }
            else
                newcardA = Instantiate(cardprefab, new Vector3(cards[i].transform.position.x, cards[i].transform.position.y, cards[i].transform.position.z), Quaternion.identity, deckcard.transform);
            if (fl == 0.2f)
            {
                cards[i].transform.position = new Vector3(cards[i].transform.position.x, cards[i].transform.position.y - 40, cards[i].transform.position.z);
            }

            else
                cards[i].transform.position = new Vector3(cards[i].transform.position.x, cards[i].transform.position.y + 40, cards[i].transform.position.z);
            newcardA.name = ((P[i][0].GetNum()).ToString()) + P[i][0].GetSuit();
            newcardA.GetComponent<selectable>().faceup = true;
            newcardA.GetComponent<PlayerScript>().cardface = P[i][0].Getface();
            P[i][0].SetPrefab(newcardA.GetComponent<selectable>());

            
        }

        public IEnumerator SlideLoser(float f)
        {
            inplay = false;
            for (int i = 0; i < 5; i++)
            {
                yield return new WaitForSeconds(f);
                if (winScore[i] == 1)
                {
                    
                    if (online)
                    {
                        manager.GetComponent<PhotonView>().RPC("GreyHand", RpcTarget.All, new object[] { 1, i });
                        //PB[i][0].GetPrefab().GetComponent<SpriteRenderer>().color = Color.grey;
                        Debug.Log("in LoserB");
                        LoserB(i);

                    }
                    else
                    {
                        Loser(PB[i], 1,i);
                    }
                        
                }
                else if (winScore[i] == -1)
                {
                    Loser(PA[i], -1, i);
                    if (online)
                    {
                        manager.GetComponent<GameScript>().GreyHand(-1, i);
                        //manager.GetComponent<PhotonView>().RPC("GreyHand", RpcTarget.Others, new object[] { -1, i });

                    }
                }
            }
        }

        [PunRPC]
        public void GreyHand(int ab, int i)
        {
            if(ab == -1)
                for(int j = 0; j < 5; j++)
                {
                    PA[i][j].GetPrefab().GetComponent<SpriteRenderer>().color = Color.grey;

                }
            else
                for (int j = 0; j < 5; j++)
                    PB[i][j].GetPrefab().GetComponent<SpriteRenderer>().color = Color.grey;

        }

        [PunRPC]
        public void TimerStop()
        {
            Timer.SetActive(false);
            inplay = false;
        }
        


        public IEnumerator Reveal(selectable[] flip, float fl)
        {
            print("--------" + ratePrefabBackUp + "-----------");
            int[] flush = new int[4];
            int[] flushB = new int[4];
            if(typeofgame != "Practice")
                Timer.SetActive(false);
            if (typeofgame == "Bot" || typeofgame == "Tour")
            {
                PurchButton.SetActive(false);
                CountButton.SetActive(false);

            }

            if (online)
            {
                manager.GetComponent<PhotonView>().RPC("TimerStop", RpcTarget.Others);
            }
            for (int i = 0; i < 11; i++)
            {
               
                yield return new WaitForSeconds(fl);
                if(i== 10)
                {
                    BackButton.SetActive(true);
                    break;
                }
                if (online)
                {
                    
                    if (Bscores[i / 2] != Bscores[0])
                        sameB = false;
                    if (Bscores[i / 2] == 5)
                    {
                        if (PB[i / 2][0].GetSuit() == "D")
                            flushB[0] += 1;
                        if (PB[i / 2][0].GetSuit() == "H")
                            flushB[1] += 1;
                        if (PB[i / 2][0].GetSuit() == "S")
                            flushB[2] += 1;
                        if (PB[i / 2][0].GetSuit() == "C")
                            flushB[3] += 1;
                    }
                }
               
                if (Ascores[i/2] == 5)
                {
                    if (PA[i/2][0].GetSuit() == "D")
                        flush[0] += 1;
                    if (PA[i/2][0].GetSuit() == "H")
                        flush[1] += 1;
                    if (PA[i/2][0].GetSuit() == "S")
                        flush[2] += 1;
                    if (PA[i/2][0].GetSuit() == "C")
                        flush[3] += 1;
                }
                StartCoroutine(FlipCard(i));
                //flip[i].GetComponent<SpriteRenderer>().sprite = flip[i].GetComponent<PlayerScript>().cardface;
                if(online)
                    flip[i].GetComponent<PhotonView>().RPC("RevealCard", RpcTarget.All);
                if(i%2 == 0)
                {
                    rateprefab.GetComponent<showRate>().rate = Rates[Ascores[i/2]];
                    GameObject AR = new GameObject();
                    ///////edited//////
                    if (online)
                    {
                        //AR = PhotonNetwork.InstantiateRoomObject(rateprefab.name, new Vector3(dragMethod.GetComponent<Snapcontroller>().snapPoints[i / 2].transform.position.x, dragMethod.GetComponent<Snapcontroller>().snapPoints[i / 2].transform.position.y - 120, dragMethod.GetComponent<Snapcontroller>().snapPoints[i / 2].transform.position.z), Quaternion.identity);
                        AR = Instantiate(rateprefab, new Vector3(dragMethod.GetComponent<Snapcontroller>().snapPoints[i / 2].transform.position.x, dragMethod.GetComponent<Snapcontroller>().snapPoints[i / 2].transform.position.y - 120, dragMethod.GetComponent<Snapcontroller>().snapPoints[i / 2].transform.position.z), Quaternion.identity, rateprefab.transform);
                        manager.GetComponent<PhotonView>().RPC("InstanRate", RpcTarget.Others, new object[] { i, Ascores[i / 2],0 });
                        //AR.GetComponent<PhotonView>().RPC("RevealRate", RpcTarget.All,Ascores[i/2]);
                        AR.GetComponent<showRate>().RevealRate(Ascores[i / 2]);

                    }
                    else
                        AR = Instantiate(rateprefab, new Vector3(dragMethod.GetComponent<Snapcontroller>().snapPoints[i/2].transform.position.x, dragMethod.GetComponent<Snapcontroller>().snapPoints[i/2].transform.position.y - 120, dragMethod.GetComponent<Snapcontroller>().snapPoints[i/2].transform.position.z), Quaternion.identity, rateprefab.transform);
                    AR.name = "A " + (i/2);
                    AR.GetComponent<showRate>().rate = Rates[Ascores[i/2]];
                    /////////edited/////////
                }
                else
                {
                    rateprefab.GetComponent<showRate>().rate = Rates[Bscores[i/2]];
                    GameObject BR = new GameObject();
                    if (online)
                    {
                        //BR = PhotonNetwork.InstantiateRoomObject(rateprefab.name, new Vector3(dragMethod.GetComponent<Snapcontroller>().snapPoints[(i/2)+5].transform.position.x, dragMethod.GetComponent<Snapcontroller>().snapPoints[(i / 2) + 5].transform.position.y + 120, dragMethod.GetComponent<Snapcontroller>().snapPoints[(i / 2) + 5].transform.position.z), Quaternion.identity);
                        BR = Instantiate(rateprefab, new Vector3(dragMethod.GetComponent<Snapcontroller>().snapPoints[(i / 2) + 5].transform.position.x, dragMethod.GetComponent<Snapcontroller>().snapPoints[(i / 2) + 5].transform.position.y + 120, dragMethod.GetComponent<Snapcontroller>().snapPoints[(i / 2) + 5].transform.position.z), Quaternion.identity, rateprefab.transform);
                        manager.GetComponent<PhotonView>().RPC("InstanRate", RpcTarget.Others, new object[] { i, Bscores[i / 2],1 });

                        //BR.GetComponent<PhotonView>().RPC("RevealRate", RpcTarget.All, Bscores[i / 2]);
                        BR.GetComponent<showRate>().RevealRate(Bscores[i / 2]);
                        //BR.GetComponent<showRate>().Spin();

                    }
                    else
                        BR = Instantiate(rateprefab, new Vector3(dragMethod.GetComponent<Snapcontroller>().snapPoints[(i/2) + 5].transform.position.x, dragMethod.GetComponent<Snapcontroller>().snapPoints[(i/2) + 5].transform.position.y + 120, dragMethod.GetComponent<Snapcontroller>().snapPoints[(i/2) + 5].transform.position.z), Quaternion.identity, rateprefab.transform);
                    BR.name = "B " + (i/2);
                    BR.GetComponent<showRate>().rate = Rates[Bscores[(i/2)]];
                }
                
                
                //Sprite tmp = flip[i].GetComponent<PlayerScript>().cardface;
                //flip[i].GetComponent<PlayerScript>().cardface = flip[i].GetComponent<PlayerScript>().cardback;
                //flip[i].GetComponent<PlayerScript>().cardback = tmp;


                if (i == 9)
                {
                    if (online)
                        manager.GetComponent<PhotonView>().RPC("ShoWon", RpcTarget.All);
                    else
                        AR.GetComponent<SpriteRenderer>().enabled = true;
                }


            }
           
            bool flushcheck = true;
            bool flushBcheck = true;
            for (int j = 0; j < 4; j++)
            {
                if (flush[j] > 2)
                {
                    UpdateDATA(5, 4, 5);
                }
                if (flushB[j] > 2)
                {
                    manager.GetComponent<PhotonView>().RPC(" UpdateDATA", RpcTarget.Others, 5, 4, 5);
                }
                if (flush[j] == 0)
                    flushcheck = false;
                if (flushB[j] == 0)
                    flushBcheck = false;

            }
            if (flushcheck)
            {
                UpdateDATA(5, 5, 5);
            }
            if (flushBcheck)
            {
                manager.GetComponent<PhotonView>().RPC(" UpdateDATA", RpcTarget.Others, 5, 5, 5);
            }
        }


        [PunRPC]
        public void InstanRate(int i,int ascores,int ab)
        {
            if (ab == 0)
                AR = Instantiate(rateprefab, new Vector3(dragMethod.GetComponent<Snapcontroller>().snapPoints[i / 2].transform.position.x, dragMethod.GetComponent<Snapcontroller>().snapPoints[i / 2].transform.position.y - 120, dragMethod.GetComponent<Snapcontroller>().snapPoints[i / 2].transform.position.z), Quaternion.identity, rateprefab.transform);
            else if(ab==1)
                AR = Instantiate(rateprefab, new Vector3(dragMethod.GetComponent<Snapcontroller>().snapPoints[(i / 2) + 5].transform.position.x, dragMethod.GetComponent<Snapcontroller>().snapPoints[(i / 2) + 5].transform.position.y + 120, dragMethod.GetComponent<Snapcontroller>().snapPoints[(i / 2) + 5].transform.position.z), Quaternion.identity, rateprefab.transform);
            else
            {
                GameRate = Instantiate(rateprefab, new Vector3(deckcard.transform.position.x + 305, deckcard.transform.position.y, deckcard.transform.position.z), Quaternion.identity, rateprefab.transform);
                AR = GameRate;
            }

            AR.GetComponent<showRate>().RevealRate(ascores);
            AR.GetComponent<showRate>().Spin();
            if (ascores > 9)
                BackButton.SetActive(true);
        }

        [PunRPC]
        public void ShoWon()
        {
            GameRate.GetComponent<SpriteRenderer>().enabled = !GameRate.GetComponent<SpriteRenderer>().enabled;
        }



            [PunRPC]
            public void UpdateDATA(int n,int j,int index)
            { 
            if(typeofgame != "Practice")
            {
                if (online && !host && j == 13 && PlayerPrefs.GetInt("Unlocked" + CollectionNames[Bscores[0]]) != 1)
                    return;
                int a = PlayerPrefs.GetInt((CollectionNames[n] + j).ToString());
                if (a != 1 && n != 0 && n != 2)
                {
                    PlayerPrefs.SetInt((CollectionNames[n] + j).ToString(), 1);
                    PlayerPrefs.SetInt("NewCollect", 1);
                    if (PlayerPrefs.GetInt("ColIndex") == 0)
                        PlayerPrefs.SetInt("ColIndex", index);
                    PlayerPrefs.SetString(index + "Collect", (CollectionNames[n] + j).ToString());
                    //do something
                }
            }
            
            }

        public void OnManu()
        {
            if (inplay)
            {
                Menuflag = 1;
                if (BackPopover.activeSelf)
                    CloseMenu();
                else
                {
                    StartCoroutine(OpenCard(BackPopover));


                }
                //BackPopover.SetActive(true);
            }
            else
            {
                GoBack();
            }

            
        }

        public void AudioManage()
        {
            int so = 1 - PlayerPrefs.GetInt("so");
            PlayerPrefs.SetInt("so", so);
            OnSound();
        }

        public void CloseMenu()
        {
            Menuflag = 0;
            StartCoroutine(CloseCard(BackPopover));
            //BackPopover.SetActive(false);
        }

        public void GoBack()
        {
            if (online)
            {
                leave = true;
                left.GetComponent<SpriteRenderer>().sprite = null;
            }
            if (PhotonNetwork.IsConnected)
            {
                PlayerPrefs.SetInt("DidILeft", 1);
                PhotonNetwork.LeaveRoom();
                PhotonNetwork.LeaveLobby();
                PhotonNetwork.Disconnect();
                while (PhotonNetwork.InRoom)
                    print("Leaving Room");
                DoSwitchScene();
            }        
            if(typeofgame == "Tour")
            {
                if (inplay)
                {
                    PlayerPrefs.SetInt("CurrentGame", 1);
                    PlayerPrefs.SetInt("Ascore", 0);
                    PlayerPrefs.SetInt("Bscore", 5);
                    PlayerPrefs.SetInt("TourInProgress", PlayerPrefs.GetInt("TourInProgress") + 1);
                }


                if(PlayerPrefs.GetInt("NewCollect") == 1)
                {
                    PlayerPrefs.SetInt("TourCollect", 1);
                    SceneManager.LoadScene(0);
                }
                else
                    SceneManager.LoadScene(4);
            }

            else
            {
                if (inplay  && typeofgame != "Practice")
                {
                    PlayerPrefs.SetInt("PlayerCoins", PlayerPrefs.GetInt("LoseScore"));
                }      
                if(!online)
                    SceneManager.LoadScene(0);
                

            }
                
        }


        IEnumerator DoSwitchScene()
        {
            while (PhotonNetwork.IsConnected)
                yield return null;
            PlayerPrefs.SetInt("PlayerCoins", PlayerPrefs.GetInt("LoseScore"));
            SceneManager.LoadScene(0);

        }




        public int[][] GroupBy(List<card> hand,int n)
        {
            int max1 = 1, max2 = 1;
            int maxNum1 = hand[0].GetNum(); 
            int maxNum2 = hand[1].GetNum();
            int cnt = 1;
            int[][] ret = new int[2][];
            ret[0] = new int[2] { 1, 1 };
            ret[1] = new int[2];
          //  for (int i = 0; i < 2; i++)
         //       ret[0][i] = 1;
            for (int i = 0; i < n; i++)
            {
                if (hand[i+1] != null && hand[i].GetNum() == hand[i + 1].GetNum())
                    cnt++;
                else 
                {

                    if (cnt > max1)
                    {
                        max2 = max1;
                        max1 = cnt;
                        maxNum2 = maxNum1;
                        maxNum1 = hand[i].GetNum();
                    }
                    else if (cnt > max2)
                    {
                        max2 = cnt;
                        maxNum2 = hand[i].GetNum();
                    }

                    cnt = 1;
                }
            }
            if (cnt >= max1)
            {
                maxNum2 = maxNum1;
                maxNum1 = hand[n].GetNum();
                max2 = max1;
                max1 = cnt;
            }
            else if (cnt > max2)
            {
                maxNum2 = hand[n].GetNum();
                max2 = cnt;
            }
            ret[0][0] = max1;
            ret[0][1] = max2;

            ret[1][0] = maxNum1;
            ret[1][1] = maxNum2;

            if(max1 == max2 && maxNum1< maxNum2)
            {
                ret[1][0] = maxNum2;
                ret[1][1] = maxNum1;
            }

            return ret;
        }
        //get a sorted list of cards
        public bool checkFlush(List<card> hand)
        {
            string s = hand[0].GetSuit();
            for (int i = 1; i < 5; i++)
                if (hand[i].GetSuit() != s)
                    return false;
            return true;

        }
        //get a sorted list of cards
        public bool checkStraight(List<card> hand)
        {
            if (hand[0].GetNum() == 14 && hand[1].GetNum() == 5)
            {
                for (int i = 1; i < 4; i++)
                    if (hand[i].GetNum() != hand[i + 1].GetNum() + 1)
                        return false;
                return true;
            }
            else
            {


                for (int i = 0; i < 4; i++)
                    if (hand[i].GetNum() != hand[i + 1].GetNum() + 1)
                    {
                        return false;
                    }

                return true;
            }
        }

        public void OnTut()
        {
            SceneManager.LoadScene(6);
        }

        //get a sorted list of cards
        public int HighStraight(List<card> hand)
        {
            Debug.Log("ARE YOU HAPPY NOW?!!");
            if (hand[0].GetNum() == 14 && hand[1].GetNum() == 5)
                return 5;
            return hand[0].GetNum();
        }
        //get a sorted list of cards
        public double HighCardGrpby(List<card> hand, int[] grpby)
        {
            double ret = (grpby[0] * Math.Pow(10, -2)) + (grpby[1] * Math.Pow(10, -4));
            int p = -6;
            for (int i = 0; i < 5; i++)
            {
                if (hand[i].GetNum() != grpby[0] && hand[i].GetNum() != grpby[1])
                {
                    ret += hand[i].GetNum() * Math.Pow(10, p);
                    p -= 2;
                }

            }
            print(ret);
            return ret;
        }




        public int CheckWinHand(card[] HA, card[] HB, int i)
        {
            List<card> HandA = HA.ToList();
            List<card> HandB = HB.ToList();
            HandA.Sort();
            //HandA.Reverse(); - changed CompareTo
            HandB.Sort();
            //HandB.Reverse();
            int[][] grpbA = GroupBy(HandA,4);
            int[][] grpbB = GroupBy(HandB,4);
            int RateA = getRate(HandA, grpbA[0]);
            int RateB = getRate(HandB, grpbB[0]);
            Ascores[i] = RateA;
            Bscores[i] = RateB;
            if (Ascores[i] != Ascores[0])
                same = false;
            if (Bscores[i] != Ascores[0])
                sameB = false;
            ///
            // rateprefab.GetComponent<showRate>().rate = Rates[RateA];
            //  GameObject AR = Instantiate(rateprefab, new Vector3(dragMethod.GetComponent<Snapcontroller>().snapPoints[i].transform.position.x, dragMethod.GetComponent<Snapcontroller>().snapPoints[i].transform.position.y - 200, dragMethod.GetComponent<Snapcontroller>().snapPoints[i].transform.position.z), Quaternion.identity, rateprefab.transform);
            //  AR.name = "A " + i;
            //  AR.GetComponent<showRate>().rate = Rates[RateA];
            //  rateprefab.GetComponent<showRate>().rate = Rates[RateA];
            // GameObject BR = Instantiate(rateprefab, new Vector3(dragMethod.GetComponent<Snapcontroller>().snapPoints[i + 5].transform.position.x, dragMethod.GetComponent<Snapcontroller>().snapPoints[i + 5].transform.position.y + 200, dragMethod.GetComponent<Snapcontroller>().snapPoints[i + 5].transform.position.z), Quaternion.identity, rateprefab.transform);
            //  BR.name = "B " + i;
            // BR.GetComponent<showRate>().rate = Rates[RateB];
            ///

            print( "RateA is: " + RateA + " Rate B is: " + RateB);
            if (RateA > RateB)
            {
                if(RateA == 5)
                {
                    for(int j=0;j<4;j++)
                    {
                        if (suits[j] == PA[i][0].GetSuit())
                            UpdateCollectionPrefs(i, j, RateA);
                    }
                }
                else if(RateA == 4 || RateA>7)
                    UpdateCollectionPrefs(i, HighStraight(HandA),RateA);
                else
                    UpdateCollectionPrefs(i, grpbA[1][0], RateA);
                return 1;
            }
                
            if (RateB > RateA)
            {
                if (online)
                {
                    if (RateB == 5)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            if (suits[j] == PB[i][0].GetSuit())
                                manager.GetComponent<PhotonView>().RPC("UpdateCollectionPrefs", RpcTarget.Others, new object[] { i, j, RateB });

                        }
                    }
                    else if (RateB == 4 || RateB > 7)
                        manager.GetComponent<PhotonView>().RPC("UpdateCollectionPrefs", RpcTarget.Others, new object[] { i, HighStraight(HandB), RateB });
                    else
                        manager.GetComponent<PhotonView>().RPC("UpdateCollectionPrefs", RpcTarget.Others, new object[] { i, grpbB[1][0], RateB });
                }
                return -1;
            }
                
            if (checkStraight(HandA))
            {
                int highA = HandA[4].GetNum();
                int highB = HandB[4].GetNum();
                if (highA > highB)
                {
                    UpdateCollectionPrefs(i, HighStraight(HandA), RateA);
                    return 1;
                }
                    
                if (highA < highB)
                {
                    if(online)
                    {
                        manager.GetComponent<PhotonView>().RPC("UpdateCollectionPrefs", RpcTarget.Others, new object[] { i, HighStraight(HandB), RateB });

                    }
                    return -1;
                }

                if(highA == 2 && HandB[0].GetNum() == 14 && HandA[0].GetNum() == 6)
                {
                    if (online)
                    { 
                    manager.GetComponent<PhotonView>().RPC("UpdateCollectionPrefs", RpcTarget.Others, new object[] { i, HighStraight(HandB), RateB });
                    }
                    return -1;
                } 
                if (highB == 14 && HandA[0].GetNum() == 14 && HandB[0].GetNum() == 6)
                {
                    UpdateCollectionPrefs(i, HighStraight(HandA), RateA);
                    return 1;
                }




                    return 0;
            }
            
            else
            {
                if(grpbA[0][0] != 1)
                {
                    double extA = HighCardGrpby(HandA, grpbA[1]);
                    double extB = HighCardGrpby(HandB, grpbB[1]);
                    if (extA > extB)
                    {
                        if (RateA == 5)
                        {
                            for (int j = 0; j < 4; j++)
                            {
                                if (suits[j] == PA[i][0].GetSuit())
                                    UpdateCollectionPrefs(i, j, RateA);
                            }
                        }
                        else
                            UpdateCollectionPrefs(i, grpbA[1][0], RateA);


                        return 1;
                    }
                    if (extA < extB)
                    {
                        if (online)
                        {
                            if (RateB == 5)
                            {
                                for (int j = 0; j < 4; j++)
                                {
                                    if (suits[j] == PB[i][0].GetSuit())
                                        manager.GetComponent<PhotonView>().RPC("UpdateCollectionPrefs", RpcTarget.Others, new object[] { i, j, RateB });

                                }
                            }
                            else
                                manager.GetComponent<PhotonView>().RPC("UpdateCollectionPrefs", RpcTarget.Others, new object[] { i, grpbB[1][0], RateB });

                        }
                        return -1;
                    }
                  
               
                }

                else
                {
                    for (int j = 0; j < 5; j++)
                    {
                        if (HandA[j].GetNum() > HandB[j].GetNum())
                            return 1;
                        if (HandA[j].GetNum() < HandB[j].GetNum())
                            return -1;

                    }
                    return 0;

                }
            }
   
            return 0;
        }
        public int getRate(List<card> hand, int[] grpby)
        {
            if (grpby[0] == 1)
            {
                bool Flash = checkFlush(hand);
                bool Straight = checkStraight(hand);
                if (Flash && Straight)
                {
                    if (HighStraight(hand) == 14)
                        return 9;
                    else
                        return 8;
                }
                if (Flash)
                    return 5;
                if (Straight)
                    return 4;
                return 0;
            }
            if (grpby[0] == 3) // [3, ]
            {
                if (grpby[1] == 2) // [3, 2]
                    return 6;
                else
                    return 3;
            }
            if (grpby[0] == 2)
            {
                if (grpby[1] == 2)
                    return 2;
                else
                    return 1;
            }
            if (grpby[0] == 4)
                return 7;
            return 0;
        }


        [PunRPC]
        public void UpdateTheLoserLoser(int i)
        {
            Loser(PB[i],1,i);
        }

 

        public void Loser(card[] H,int ab,int index)
        {
            H[0].GetPrefab().GetComponent<SpriteRenderer>().color = Color.grey;
            if (online)
            {
                H[0].GetPrefab().GetComponent<PhotonView>().RPC("GreyMyCard", RpcTarget.All);
            }
            H[0].GetPrefab().GetComponent<SpriteRenderer>().color = Color.grey;
            for (int i = 1; i < 5; i++)
            {
                if (online)
                {
                    H[i].GetPrefab().GetComponent<PhotonView>().RPC("GreyMyCard", RpcTarget.All);

                }
                H[i].GetPrefab().transform.position = new Vector3(H[0].GetPrefab().transform.position.x, H[0].GetPrefab().transform.position.y + (25 * i*ab), H[0].GetPrefab().transform.position.z);
                H[i].GetPrefab().GetComponent<SpriteRenderer>().color = Color.grey;
                Debug.Log("Loser Debugger");
                
            }
        }

        [PunRPC]
        public void SinkIt(int i,int j,float x,float y, float z)
        {
            if(j==0)
                    print("sink it!!----------------------------------------------");
            PB[i][j].GetPrefab().transform.position = (new Vector3(x, y, z));
            PB[i][j].GetPrefab().GetComponent<SpriteRenderer>().color = Color.grey;
            PB[i][j].GetPrefab().GetComponent<PhotonView>().RPC("GreyMyCard", RpcTarget.All);
            


        }


        public void LoserB(int i)
        {
            //PB[i][0].GetPrefab().GetComponent<PhotonView>().TransferOwnership(2);
            PB[i][0].GetPrefab().GetComponent<PhotonView>().RPC("GreyMyCard", RpcTarget.All);
            manager.GetComponent<PhotonView>().RPC("SinkIt", RpcTarget.Others, new object[] { i, 0, PB[i][0].GetPrefab().transform.position.x, PB[i][0].GetPrefab().transform.position.y , PB[i][0].GetPrefab().transform.position.z });
            print("sank it-------------------------------");

            for (int j = 1; j < 5; j++)
            {
                manager.GetComponent<PhotonView>().RPC("SinkIt", RpcTarget.Others, new object[] { i, j, PB[i][0].GetPrefab().transform.position.x, PB[i][0].GetPrefab().transform.position.y + (25 * j), PB[i][0].GetPrefab().transform.position.z });


           //     H[i].GetPrefab().GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.PlayerList[0]);
           //     H[i].GetPrefab().transform.position = new Vector3(H[0].GetPrefab().transform.position.x, H[0].GetPrefab().transform.position.y + (25 * i * ab), H[0].GetPrefab().transform.position.z);
           //     Debug.Log("Loser Debugger");

            }
        }

        [PunRPC]
        public void UpdateCollectionPrefs(int i,int grpb,int rank)
        {
            if(typeofgame != "Practice")
            {
                if (rank == 9)
                    rank = 8;
                if (rank == 4 || rank == 8)
                {
                    grpb -= 5;

                }
                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
                else if (rank == 1 || rank == 3 || rank == 6 || rank == 7)
                    grpb -= 2;
                int a = PlayerPrefs.GetInt((CollectionNames[rank] + grpb).ToString());
                if (a != 1 && rank != 0 && rank != 2)
                {
                    int r;
                    if (rank == 1)
                    {
                        r = 0;
                    }
                    else
                    {
                        r = rank - 2;
                    }
                    if (PlayerPrefs.GetInt("RoomIndex") >= r || typeofgame == "Tour")
                    {
                        Debug.Log("Chilling in the Collect");
                        PlayerPrefs.SetInt((CollectionNames[rank] + grpb).ToString(), 1);
                        PlayerPrefs.SetInt("NewCollect", 1);
                        if (PlayerPrefs.GetInt("ColIndex") == 0)
                            PlayerPrefs.SetInt("ColIndex", i);
                        PlayerPrefs.SetString(i + "Collect", (CollectionNames[rank] + grpb).ToString());
                        //do something
                    }
                }
            }          
        }



        [PunRPC]
        public void ByeCard(int k,int ab)
        {
            if(ab ==0)
                 PA[k][4].GetPrefab().GetComponent<SpriteRenderer>().sprite = null;
            else
                PB[k][4].GetPrefab().GetComponent<SpriteRenderer>().sprite = null;
        }

        public int CheckWinGame(card[][] PA, card[][] PB)
        {
            int cnt = 0;
            PlayerPrefs.SetInt("ColIndex",0);
            int cntA = 0;
            int win = 0;
            int cntB = 0;
            for (int i = 0; i < 5; i++)
            {
                win = CheckWinHand(PA[i], PB[i], i);
                winScore[i] = win;
                if (win == 1)
                {
                    cntA += 1;            
                }
                    
                if (win == -1)
                {
                    cntB += 1;
                }
                    
                cnt += win;
            }
            if(typeofgame == "Tour")
            {
                PlayerPrefs.SetInt("CurrentGame", 1);
                PlayerPrefs.SetInt("Ascore", cntA);
                PlayerPrefs.SetInt("Bscore", cntB);
                PlayerPrefs.SetInt("TourInProgress", PlayerPrefs.GetInt("TourInProgress")+1);
            }
            if (cntA > cntB)
            {
                print("A Won " + cntA + ":" + cntB);
                if(typeofgame != "Practice")
                    PlayerPrefs.SetInt("GamesWon", PlayerPrefs.GetInt("GamesWon") + 1);
                print("Game is: "+ PlayerPrefs.GetInt("GamesWon"));
                if (online)
                {
                    //AR = PhotonNetwork.InstantiateRoomObject(rateprefab.name, new Vector3(deckcard.transform.position.x + 300, deckcard.transform.position.y, deckcard.transform.position.z), Quaternion.identity);
                    AR = Instantiate(rateprefab, new Vector3(deckcard.transform.position.x + 305, deckcard.transform.position.y, deckcard.transform.position.z), Quaternion.identity, rateprefab.transform);
                    manager.GetComponent<PhotonView>().RPC("InstanRate", RpcTarget.Others, new object[] { 0, 11,2 });
                    AR.GetComponent<showRate>().RevealRate(10);
                    GameRate = AR;
                    //
                    //AR.GetComponent<PhotonView>().RPC("RevealRate", RpcTarget.Others, 11);
                    //
                    //AR.GetComponent<showRate>().Spin();


                }
                else
                    AR = Instantiate(rateprefab, new Vector3(deckcard.transform.position.x + 305, deckcard.transform.position.y, deckcard.transform.position.z), Quaternion.identity, rateprefab.transform);
                AR.name = "A ";
                AR.GetComponent<showRate>().rate = Rates[10];
                //
                if (online)
                  manager.GetComponent<PhotonView>().RPC("ShoWon", RpcTarget.All);
                else
                    AR.GetComponent<SpriteRenderer>().enabled = false;
                if (typeofgame != "Practice" && typeofgame != "Tour")
                {
                    PlayerPrefs.SetInt("MoneyMove", 1);
                    //PlayerPrefs.SetInt("PlayerCoins", PlayerPrefs.GetInt("WinScore"));
                    Debug.Log("CurrScore IS " + PlayerPrefs.GetInt("PlayerCoins"));
                }
                if (same && PlayerPrefs.GetInt("Unlocked" + CollectionNames[Ascores[0]]) == 1)
                {
                    UpdateDATA(Ascores[0], 13, 5);
                }
             
            }

            else
            {
                if (cntB > cntA)
                {
                    print("B Won " + cntA + ":" + cntB);
                    if (typeofgame != "Practice")
                        PlayerPrefs.SetInt("GamesLost", PlayerPrefs.GetInt("GamesLost") + 1);
                    //     if (typeofgame != "Practice")
                    //         PlayerPrefs.SetInt("GamesLost", PlayerPrefs.GetInt("GamesLost") + 1);
                    //     if (typeofgame == "Bot" || typeofgame == "Tour")
                    //     {
                    //         string g = "GamesLost" + FindObjectOfType<BotScript>().BotMode.ToString();
                    //         PlayerPrefs.SetInt(g, PlayerPrefs.GetInt(g) + 1);
                    //     }
                    if (online)
                    {

                        //AR = PhotonNetwork.InstantiateRoomObject(rateprefab.name, new Vector3(deckcard.transform.position.x + 300, deckcard.transform.position.y, deckcard.transform.position.z), Quaternion.identity);
                        AR = Instantiate(rateprefab, new Vector3(deckcard.transform.position.x + 305, deckcard.transform.position.y, deckcard.transform.position.z), Quaternion.identity, rateprefab.transform);
                        manager.GetComponent<PhotonView>().RPC("InstanRate", RpcTarget.Others, new object[] { 0, 10, 2 });

                        //revealRate was deleted due the fact that we removed the photon view component in RATE prefab
                        //AR.GetComponent<PhotonView>().RPC("RevealRate", RpcTarget.Others, 10);
                        AR.GetComponent<showRate>().RevealRate(11);
                        GameRate = AR;
                        //AR.GetComponent<showRate>().Spin();
                        manager.GetComponent<PhotonView>().RPC("ShoWon", RpcTarget.All);
                    }
                    else
                    {
                        AR = Instantiate(rateprefab, new Vector3(deckcard.transform.position.x + 305, deckcard.transform.position.y, deckcard.transform.position.z), Quaternion.identity, rateprefab.transform);
                        AR.GetComponent<SpriteRenderer>().enabled = false;
                    }
                    AR.name = "A ";
                    AR.GetComponent<showRate>().rate = Rates[11];
                    if (typeofgame != "Practice" && typeofgame != "Tour")
                    {
                        PlayerPrefs.SetInt("PlayerCoins", PlayerPrefs.GetInt("LoseScore"));
                        Debug.Log("CurrScore IS " + PlayerPrefs.GetInt("PlayerCoins"));
                    }
                    if (online && sameB)
                    {
                        manager.GetComponent<PhotonView>().RPC(" UpdateDATA", RpcTarget.Others, Bscores[0], 13, 5);
                    }
                }
                else
                {
                    print("Its a Tie");
                    if (online)
                    {
                        //AR = PhotonNetwork.InstantiateRoomObject(rateprefab.name, new Vector3(deckcard.transform.position.x + 300, deckcard.transform.position.y, deckcard.transform.position.z), Quaternion.identity);
                        AR = Instantiate(rateprefab, new Vector3(deckcard.transform.position.x + 305, deckcard.transform.position.y, deckcard.transform.position.z), Quaternion.identity, rateprefab.transform);
                        manager.GetComponent<PhotonView>().RPC("InstanRate", RpcTarget.Others, new object[] { 0, 12, 2 });

                        //AR.GetComponent<PhotonView>().RPC("RevealRate", RpcTarget.All, 12);
                        AR.GetComponent<showRate>().RevealRate(12);
                        GameRate = AR;
                    }
                    else
                        AR = Instantiate(rateprefab, new Vector3(deckcard.transform.position.x + 305, deckcard.transform.position.y, deckcard.transform.position.z), Quaternion.identity, rateprefab.transform);
                    AR.name = "A ";
                    AR.GetComponent<showRate>().rate = Rates[12];
                }

            }


            return cnt;
        }

       

    }

    
}