using UnityEngine;
using UnityEngine.SceneManagement;
using Common;
using System.Timers;
namespace Gamekit3D.Network
{
    public class MyNetwork : MonoBehaviour
    {
        public const string PORT = "port";
        public const string HOST = "host";

        static private Incoming incomming = new Incoming(Client.Instance);

        static public MyNetwork Instance { get { return m_instance; } }

        static private MyNetwork m_instance;
        static private bool connected = false;
        [Tooltip("auto connect to localhost")]
        public bool gameScene = false;

        [Tooltip("address")]
        public string address = "127.0.0.1";

        [Tooltip("port")]
        public short port = 7777;

        private Timer heartBeatTimer = null;
        void Awake()
        {
            m_instance = this;
            MessageBox.Init();
            Debug.Log(this.name);
            if (!connected)
            {
                if (PlayerPrefs.HasKey(HOST) && PlayerPrefs.HasKey(PORT))
                {
                    string host = PlayerPrefs.GetString(HOST);
                    short port = (short)PlayerPrefs.GetInt(PORT);
                    connected = MyNetwork.Connect(host, port);
                    if (!connected)
                    {
                        MessageBox.Show(string.Format("connect to {0}:{1} fail", host, port));
                    }
                }
            }
            if (!connected)
            {
                string host = this.address;
                short port = this.port;
                connected = MyNetwork.Connect(host, port);
                if (!connected)
                {
                    MessageBox.Show(string.Format("connect to {0}:{1} fail", host, port));
                }
            }
            if (gameScene)
            {
                if (connected)
                {
                    // for debug only ...
                    CLogin login = new CLogin();
                    login.user = "final";
                    login.password = "e10adc3949ba59abbe56e057f20f883e";
                   // Send(login);
                }
                SceneManager.sceneLoaded += RecvSceneLoaded;
            }
            if (connected && heartBeatTimer == null)
            {
                heartBeatTimer = new Timer();
                heartBeatTimer.Interval = 3000;
                heartBeatTimer.Elapsed += delegate
                {
                    CHeartBeat msg = new CHeartBeat();
                    Send(msg);
                };
                heartBeatTimer.Start();
            }
        }
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            Client.Instance.RecvMessage();
        }

        // Use this for initialization
        public bool Connect()
        {
            if (!connected)
            { // exactly connect once
                connected = Client.Instance.Connect(address, port);
            }
            return connected;
        }

        static public bool Connect(string host, int port)
        {
            return Client.Instance.Connect(host, (short)port);
        }

        static public void Send(Message message)
        {
            Client.Instance.Send(message);
        }
        static public void Register(Command command, MessageDelegate @delegate)
        {
            Client.Instance.Register(command, @delegate);
        }

        static public void Close()
        {
            Client.Instance.Close();
        }

        static private void RecvSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            incomming.InitNetworkEntity();
            CPlayerEnter message = new CPlayerEnter();
            Client.Instance.Send(message);
        }
    }
}
