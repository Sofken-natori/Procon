using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Cysharp.Threading.Tasks;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ServerConnector
{

    public class MatchesInfo
    {
        public Match[] matches;
    }

    public class Match
    {
        public int id;
        public int turns;
        public int turnSeconds;
        public Bonus bonus;
        public InitBoard board;
        public string oponent;
        public bool first;
    }

    public class MatchInfo
    {
        public int id;
        public int turn;
        
        public NowBoard board;

        public Log[] logs;
    }

    public class NowBoard
    {

        public int width;
        public int height;
        public int mason;
        public int[,] structures;
        public int[,] masons;
        public int[,] walls;
        public int[,] territories;
    }

    public class InitBoard
    {
        public int width;
        public int height;
        public int mason;
        public int[,] structures;
        public int[,] masons;
    }

    public class PostInfo
    {
        public int turn;
        public List<Move> moves;
    }

    
    public class Bonus
    {
        public int wall;
        public int territory;
        public int castle;
    }

    public class Log
    {
        public int turn;
        public Action[] actions;

    }

    public class Action
    {
        public bool succeeded;
        public int type;
        public int dir;
    }

    public class Move
    {
        public int dir;
        public int type;
    }



    public class InfoConnector
    {
        public string CallAPIURL = "http://localhost:3000";
        public string token = "first";
        public async UniTask<MatchesInfo> GetMatchesInfo()
        {
            UnityWebRequest req = UnityWebRequest.Get(CallAPIURL + "/matches?token=" + token);
            var info = await req.SendWebRequest();
            MatchesInfo res = new MatchesInfo();

            if (req.error != null)
            {
                Debug.LogError(req.error);
                return res;
            }

            try
            {
                var JSON = info.downloadHandler.text;
                JObject obj = JObject.Parse(JSON);
                res.matches = obj["matches"].ToObject<Match[]>();
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
            }

            return res;
        }

        public async UniTask<MatchInfo> GetMatchInfo(int id)
        {
            UnityWebRequest req = UnityWebRequest.Get(CallAPIURL + "/matches/" + id + "?token=" + token);
            var info = await req.SendWebRequest();
            MatchInfo res = new MatchInfo();

            if (req.error != null)
            {
                Debug.LogError(req.error);
                return res;
            }
            try
            {
                var JSON = info.downloadHandler.text;
                JObject obj = JObject.Parse(JSON);
                res.id = obj["id"].Value<int>();
                res.turn = obj["turn"].Value<int>();
                res.board = obj["board"].ToObject<NowBoard>();
                res.logs = obj["logs"].ToObject<Log[]>();
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
            }

            return res;
        }

        public async void PostMatchInfo(int id, PostInfo move)
        {
            string reqJson = JsonConvert.SerializeObject(move);
            UnityWebRequest req = UnityWebRequest.Post(CallAPIURL + "/matches/" + id + "?token=" + token, reqJson);
            var res = await req.SendWebRequest();
            if (req.error != null)
            {
                Debug.LogError(req.error);
                return;
            }


            JObject obj = JObject.Parse(res.downloadHandler.text);
            string resJSON = obj["accepted_at"].Value<string>();
            Debug.Log(resJSON);

            return;
        }

       
    }

    
    


    

     
}

