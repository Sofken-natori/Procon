using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Cysharp.Threading.Tasks;
using System.Threading.Tasks;


namespace ServerConnector
{
    public class Info
    {
        public int id;
        public int turn;

        public string[] log;

        public class Board
        {   
            public int width;
            public int height;
            public int mason;
            public int[,] structures;
            public int[,] masons;
            public int[,] walls;
            public int[,] territories;
        }
    }

    


    public class InfoConnector
    {
        public string CallAPIURL = "http://localhost:3000";
        public string token = "FirstAttacker";
        public async UniTask<Info> GetMatchesInfo()
        {
            Debug.Log(CallAPIURL + "/matches?token=" + token);
            return await GetInfo(UnityWebRequest.Get(CallAPIURL + "/matches?token=" + token));
        }

        public async UniTask<Info> GetMatchInfo(int id)
        {
            Debug.Log(CallAPIURL + "/matches/" + id + "?token=" + token);
            return await GetInfo(UnityWebRequest.Get(CallAPIURL + "/matches/" + id + "?token=" + token));
        }
        


        public async UniTask<Info> GetInfo(UnityWebRequest request)
        {
            var info = await request.SendWebRequest();
            Info res = null;

            if (request.error != null)
            {
                Debug.LogError(request.error);
                return res;
            }

            try
            {
                var JSON = info.downloadHandler.text;
                res = JsonUtility.FromJson<Info>(JSON);
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
            }

            return res;
        }

       
    }

    
    


    

     
}

