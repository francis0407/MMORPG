using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Gamekit3D;
using FrontEnd;
using Common;
public class ChatUI : MonoBehaviour
{

    public GameObject messageView;
    public GameObject myMessage;
    public GameObject friendMessage;

    private string friendName;
    private int friendId;
    private int msgCount = 0;
    private List<GameObject> messageObjects = new List<GameObject>();
    private List<ChatEntry> messages = null;
    // my message info content layout | ----------------- message text | image |
    // friend's info content layout   | image | message text ----------------- |

    
    public void setFriendName(string name)
    {
        friendName = name;
        var player = World.Instance.players[friendName];
        messages = World.Instance.chatHistory[player];
        friendId = player;
        Debug.Log(string.Format("Set Friend Name {0}", name));
        if (messages == null)
            Debug.Log("message = null");
    }
    private void Awake()
    {
        myMessage.SetActive(false);
        friendMessage.SetActive(false);
    }
    // Use this for initialization
    void Start()
    {
        
        //Test();
    }

    private void OnEnable()
    {   
        PlayerMyController.Instance.EnabledWindowCount++;
        if (friendName == null)
            return;
    }

    private void OnDisable()
    {
        PlayerMyController.Instance.EnabledWindowCount--;
        friendName = null;
        messages = null;
        foreach (var msgOb in messageObjects)
            msgOb.SetActive(false);
        messageObjects.Clear();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (messages == null)
            return;
        if (messages.Count > messageObjects.Count)
        {
            for (int i = messageObjects.Count; i < messages.Count; i++)
            {
                var msg = messages[i];
                if (msg.self)
                    SendMyMessage(msg.message);
                else
                    ReceiveFriendMessage(msg.message);
            }
        }
    }

    public void ReceiveFriendMessage(string text)
    {
        if (friendMessage == null)
            return;

        GameObject cloned = GameObject.Instantiate(friendMessage);
        if (cloned == null)
            return;
        cloned.SetActive(true);
        AddElement(cloned, text);
        messageObjects.Add(cloned);
    }

    public void SendMyMessage(string text)
    {
        if (myMessage == null)
            return;

        GameObject cloned = GameObject.Instantiate(myMessage);
        if (cloned == null)
            return;
        cloned.SetActive(true);
        AddElement(cloned, text);
        messageObjects.Add(cloned);
        CChatMessage msg = new CChatMessage();
        msg.from = World.Instance.selfId;
        msg.to = friendId;
        msg.message = text;
        
        Gamekit3D.Network.Client.Instance.Send(msg);
    }

    public void OnSendButtonClick(GameObject inputField)
    {
        InputField input = inputField.GetComponent<InputField>();
        if (input == null)
            return;

        if (input.text.Trim().Length == 0)
            return;


        //SendMyMessage(input.text);
        messages.Add(new ChatEntry(true, input.text));
        input.text = "";
    }

    void AddElement(GameObject element, string text)
    {
        TextMeshProUGUI textMesh = element.GetComponentInChildren<TextMeshProUGUI>();
        if (textMesh == null)
            return;
        //float width = textMesh.GetPreferredValues().x; // get preferred width before assign text
        textMesh.text = text;
        RectTransform rectTransform = element.GetComponent<RectTransform>();
        if (rectTransform == null)
            return;

        RectTransform parentRect = this.GetComponent<RectTransform>();
        if (parentRect == null)
            return;

        if (textMesh.preferredWidth < parentRect.rect.width)
        {
            ContentSizeFitter filter = textMesh.GetComponent<ContentSizeFitter>();
            if (filter != null)
            {
                filter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
                textMesh.rectTransform.sizeDelta = new Vector2(textMesh.preferredWidth, textMesh.preferredHeight);
            }
        }

        element.transform.SetParent(this.transform, false);

        ScrollRect scrollRect = messageView.GetComponent<ScrollRect>();
        if (scrollRect == null)
            return;

        scrollRect.normalizedPosition = new Vector2(0, 0);
    }

    void Test()
    {
        //AddNewMessage(true, "my message send");
        //AddNewMessage(false, "friend message receive");

        SendMyMessage("hello");
        ReceiveFriendMessage("hello");
    }

    /*
    void AddNewMessage(bool mine, string message)
    {
        GameObject newContent = GameObject.Instantiate(content);
        if (newContent == null)
            return;
        GameObject newImage = GameObject.Instantiate(image);
        if (newImage == null)
            return;
        GameObject newText = GameObject.Instantiate(text);
        if (newText == null)
            return;

        HorizontalLayoutGroup layout = newContent.GetComponent<HorizontalLayoutGroup>();
        if (mine)
            layout.childAlignment = TextAnchor.UpperRight;
        else
            layout.childAlignment = TextAnchor.UpperLeft;

        TextMeshProUGUI textMesh = text.GetComponentInChildren<TextMeshProUGUI>();
        if (textMesh == null)
            return;

        //float width = textMesh.GetPreferredValues().x; // get preferred width before assign text
        textMesh.text = message;
        RectTransform rectTransform = text.GetComponent<RectTransform>();
        if (rectTransform == null)
            return;

        RectTransform viewRect = messageContent.GetComponent<RectTransform>();
        if (viewRect == null)
            return;

        if (textMesh.preferredWidth < viewRect.rect.width)
        {
            ContentSizeFitter filter = textMesh.GetComponent<ContentSizeFitter>();
            if (filter != null)
            {
                filter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
                rectTransform.sizeDelta = new Vector2(textMesh.preferredWidth, textMesh.preferredHeight);
            }
        }

        newImage.transform.SetParent(newContent.transform, false);
        newText.transform.SetParent(newContent.transform, false);
        newContent.transform.SetParent(messageContent.transform, false);
    }
    */
}
