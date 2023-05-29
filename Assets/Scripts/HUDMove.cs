using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDMove : MonoBehaviour
{
    public Text textUI;

    void Awake()
    {
        textUI = GetComponent<Text>();
    }
    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, 5.0f);
    }

    // Update is called once per frame
    void Update()
    {
        this.gameObject.transform.localPosition += new Vector3(0.0f, 1.2f, 0.0f);
    }
}