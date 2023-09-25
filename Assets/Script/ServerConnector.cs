using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ServerConnector
{
    public class Info
    {
        public int id{get; set;}
        public int turn{get; set;}
        public int mason{get; set;}
        public class Board
        {
            
            public int[,] structure{get;set;}
            public int[,] mason {get;set;}
            public int[,] walls {get;set;}
            public int[,] territories {get;set;}

        }
        public string[] log {get;set;}
    }


    public class ServerConnector
    {
        public Info GetMatchesInfo(string CallAPIURL)
        {
            return StartCoroutine(GetInfo(CallAPIURL + "/matches"));
        }

        public Info GetMatchInfo(string CallAPIURL)
        {
            return StartCoroutine(GetInfo(CallAPIURL + "/match/" + id));
        }
        
        public Info PostMatchInfo(string CallAPIURL)
        {
            return StartCoroutine(PostInfo(CallAPIURL + "/match" + id));
        }



        public IEnumerator GetInfo(string endPoint)
        {
            try
            {
                var request = Unity.UnityEngine.Networking.UnityWebRequest.Get(endPoint);

                yield return request.SendWebRequest();

                var JSON = request.downloadHandler.text;
                var info = JsonUtility.FromJson<Info>(JSON);
                

                return info;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
            }

            return null;
        }
    }

    
    


    

     
}

