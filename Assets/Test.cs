using org.zgl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

	// Use this for initialization
	void Start () {
        ITesst socket = InvokeService.Proxy<ITesst>();
        //string s = (string)socket.aSync("test", 18);
        socket.xx(1,"x");
        socket.xxx(1,"x");
        socket.aa();
        socket.vv();
        socket.vx();
        socket.v2();
        socket.v4();
        socket.v3();
        socket.v5();
        socket.v6();
        socket.v1();
        //print(s);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
