using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using TMPro;
public class AppHandler : MonoBehaviour
{
    private SerialPort sp;
    private TextMeshProUGUI serialFeed;
    public GameObject itemFoundPrefab;
    public Transform gridParent;

    //Axis
    float dValue = 0;
    float lValue = 0;
    float uValue = 0;
    float rValue = 0;

    //Btn bools
    bool aDown = false;
    bool bDown = false;
    bool xDown = false;
    bool yDown = false;

    enum ChangeState {NOCHANGE = 0, CHANGETRUE = 1, CHANGEFALSE =2};
    
    void Start()
    {
        serialFeed = GameObject.Find("SerialFeed").GetComponent<TextMeshProUGUI>();
        sp = new SerialPort("COM7", 9600);
        sp.ReadTimeout = 50;
        sp.Open();
    }

    #region ChangeStates

    ChangeState CheckAxis(string axisName, ref float axisDown, int mod)
    {
        if(Input.GetAxis(axisName) != axisDown)
        {
            if(Input.GetAxis(axisName) * mod >= 0.3f && axisDown * mod < 1)
            {
                axisDown = 1;
                return ChangeState.CHANGETRUE;
            }
            else
            {
                axisDown = 0;
                return ChangeState.CHANGEFALSE;
            }
        }

        return ChangeState.NOCHANGE;
    }

    ChangeState CheckBtn(string btnName, ref bool btnDown)
    {
        if (Input.GetButton(btnName) != btnDown)
        {
            if (!btnDown)
            {
                btnDown = true;
                return (ChangeState.CHANGETRUE);
            }
            else
            {
                btnDown = false;
                return (ChangeState.CHANGEFALSE);
            }

        }
        else
            return (ChangeState.NOCHANGE);
    }
    #endregion

    void Update()
    {    
    #region DPAD
        ChangeState state = ChangeState.NOCHANGE;
        state = CheckAxis("dpadY", ref dValue, -1);
       if(state == ChangeState.CHANGETRUE)
        {
            //Send down message
            sp.Write("b");
            string temp = "\n Go Back";
            serialFeed.text = temp;
        }

        state = CheckAxis("dpadX", ref lValue, -1);
        if (state == ChangeState.CHANGETRUE)
        {
            //Send down message
            sp.Write("l");
            string temp = "\n Turn Left";
            serialFeed.text = temp;
        }

        state = CheckAxis("dpadY", ref uValue, 1);
        if (state == ChangeState.CHANGETRUE)
        {
            //Send down message
            sp.Write("f");
            string temp = "\n Go Forward";
            serialFeed.text = temp;
        }

        state = CheckAxis("dpadX", ref rValue, 1);
        if (state == ChangeState.CHANGETRUE)
        {
            //Send down message
            sp.Write("r");
            string temp = "\n Turn Right";
            serialFeed.text = temp;
        }
#endregion

    #region Buttons
        // X button
        if (CheckBtn("X", ref xDown) == ChangeState.CHANGETRUE)
        {           
            sp.Write("o");
            string temp = "Scan Left";
            serialFeed.text = temp;
        }
        else
        // B button
        if (CheckBtn("B", ref bDown) == ChangeState.CHANGETRUE)
        {
            sp.Write("p");
            string temp = "Scan Right";
            serialFeed.text = temp;
        }
        else
        // Y button
        if (CheckBtn("Y", ref yDown) == ChangeState.CHANGETRUE)
        {
            sp.Write("i");
            string temp = "Interrupt";
            serialFeed.text = temp;
        }
        else
        // A button
        if (CheckBtn("A", ref aDown) == ChangeState.CHANGETRUE)
        {
            sp.Write("a");
            string temp = "Forward";
            serialFeed.text = temp;
        }


        #endregion

      //  ListenForObj();
    }

    void ListenForObj()
    {
        if (!sp.IsOpen)
            sp.Open();
        string msg = sp.ReadLine();
        if(msg == "z")
        {
            Instantiate(itemFoundPrefab, gridParent);
        }
    }

    void WriteToSerial(string _msg)
    {
        sp.WriteLine(_msg);
        sp.BaseStream.Flush();          //Flush() to ensure all data is sent at the same time
    }
}
