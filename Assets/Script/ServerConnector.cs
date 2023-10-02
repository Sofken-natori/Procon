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
        public int mason;
        public string[] log;
    }

    public class Board
    {        
        public int[,] structure;
        public int[,] mason;
        public int[,] walls;
        public int[,] territories;
    }


    public class InfoConnector
    {
        public string CallAPIURL = "http://localhost:3000";
        public string token = "first";
        public async UniTask<Info> GetMatchesInfo()
        {
            return await GetInfo(UnityWebRequest.Get(CallAPIURL + "/matches"));
        }

        public async UniTask<Info> GetMatchInfo(int id)
        {
            return await GetInfo(UnityWebRequest.Get(CallAPIURL + "/matches/" + id));
        }
        


        public async UniTask<Info> GetInfo(UnityWebRequest request)
        {
            request.SetRequestHeader("procon-token", token);
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

