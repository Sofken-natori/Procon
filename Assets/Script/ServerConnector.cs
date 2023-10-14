using System.Collections;
using System.Collections.Generic;
using System.Text;
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
        public List<Move> actions = new List<Move>();
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
        public string CallAPIURL = "http://172.28.0.1:8080";
        public string token = "natori463152845d344e13cbe4cd1cb276cbaf8479aa6f3930baecbd9e977a9f";


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
            string reqStr = JsonConvert.SerializeObject(move);
            Debug.Log(reqStr);
            var reqJson = Encoding.UTF8.GetBytes(reqStr);
            UnityWebRequest req = new UnityWebRequest(CallAPIURL + "/matches/" + id + "?token=" + token, UnityWebRequest.kHttpVerbPOST)
            {
                uploadHandler = new UploadHandlerRaw(reqJson),
                downloadHandler = new DownloadHandlerBuffer()
            };
            req.SetRequestHeader("Content-Type", "application/json");
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