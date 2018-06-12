using org.zgl;
using org.zgl.gamelogic;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;
using UnityEngine.Networking;

public class Test : MonoBehaviour {

    // Use this for initialization
    void Start()
    {
        ITestLogic lo = InvokeService.Proxy<ITestLogic>();
        string res = lo.test();
        print(res);
        //bool isAss = typeof(IHttpSync).IsAssignableFrom(typeof(ITesst));
        //print(typeof(ITesst));
        //print(typeof(IHttpSync));
        //print(isAss);
        //ITesst socket = InvokeService.Proxy<ITesst>();
        //socket.xxx(1, "a");、
        // HttpWebRequest wbRequest = (HttpWebRequest)WebRequest.Create("");
        //wbRequest.Method = "POST";
        //wbRequest.ContentType = "application/octet-stream";
        //using (Stream requestStream = wbRequest.GetRequestStream())
        //{
        //    using (StreamWriter sw = new StreamWriter(requestStream))
        //    {
        //        //sw.Write(null);
        //    }
        //}
        //HttpWebResponse wbResponse = (HttpWebResponse)wbRequest.GetResponse();
        //using (Stream responseStream = wbResponse.GetResponseStream())
        //{
        //    using (StreamReader sread = new StreamReader(responseStream))
        //    {
        //        result = sread.ReadToEnd();
        //    }
        //}
    }

    // Update is called once per frame
    void Update () {
		
	}
 
}
