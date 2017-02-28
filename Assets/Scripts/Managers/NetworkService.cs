using System;
using System.Collections;
using UnityEngine;

public class NetworkService
{
    private const string XML_API = "http://api.openweathermap.org/data/2.5/weather?q=Guangzhou,cn&mode=xml&APPID=00bbee9a6b1f7f5688043c695cf42193";
    private const string JSON_API = "http://api.openweathermap.org/data/2.5/weather?q=Guangzhou,cn&mode=json&APPID=00bbee9a6b1f7f5688043c695cf42193";
    private const string WEB_IMAGE = "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1487845895293&di=0ab74f9b93aea426b6cdbafe9c0a1755&imgtype=0&src=http%3A%2F%2Fpic1.win4000.com%2Fwallpaper%2F4%2F53b0ce450ef90.jpg";
    private const string SERVER_API = "http://caizicong.com/timeCapsule/api.php";

    public IEnumerator GetWeatherXML(Action<string> callback)
    {
        return CallAPI(XML_API, null, callback);
    }

    public IEnumerator GetWeatherJSON(Action<string> callback)
    {
        return CallAPI(JSON_API, null, callback);
    }

    public IEnumerator DownloadImage(Action<Texture2D> callback)
    {
        WWW www = new WWW(WEB_IMAGE);
        yield return www;

        callback(www.texture);
    }

    public IEnumerator LogWeather(string name, float cloudValue, Action<string> callback)
    {
        Hashtable args = new Hashtable();
        args.Add("message", name);
        args.Add("cloud_value", cloudValue);
        args.Add("timestamp", DateTime.UtcNow.Ticks);

        return CallAPI(SERVER_API, args, callback);
    }

    private IEnumerator CallAPI(string url, Hashtable args, Action<string> callback)
    {
        WWW www;

        if (null == args)
        {
            www = new WWW(url);
        }
        else
        {
            WWWForm form = new WWWForm();

            foreach (DictionaryEntry arg in args)
            {
                form.AddField(arg.Key.ToString(), arg.Value.ToString());
            }

            www = new WWW(url, form);
        }

        yield return www;

        if (!IsResponseValid(www))
        {
            yield break;
        }

        callback(www.text);
    }

    private bool IsResponseValid(WWW www)
    {
        if (null != www.error)
        {
            Debug.Log("bad network connection!");
            return false;
        }
        else if (string.IsNullOrEmpty(www.text))
        {
            Debug.Log("bad data!");
            return false;
        }
        else
        {
            return true;
        }
    }
}
