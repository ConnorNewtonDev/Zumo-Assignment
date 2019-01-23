using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;

public class AppHandler : MonoBehaviour
{
    private SerialPort sp;

    void Start()
    {
        sp = new SerialPort("COM4", 9600);
        sp.ReadTimeout = 50;
        sp.Open();
    }

#region Buttons
    public void ForwardBtn()
    {
        WriteToSerial("MF");
    }

    public void LeftBtn()
    {
        WriteToSerial("LT");
    }

    public void RightBtn()
    {
        WriteToSerial("RT");
    }
#endregion

    void WriteToSerial(string _msg)
    {
        sp.WriteLine(_msg);
        sp.BaseStream.Flush();          //Flush() to ensure all data is sent at the same time
    }
}
