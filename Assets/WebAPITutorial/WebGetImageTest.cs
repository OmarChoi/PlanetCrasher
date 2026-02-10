using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Networking;
using UnityEngine.UI;

public class WebGetImageTest : MonoBehaviour
{
    [SerializeField] private RawImage _image;
    private async void Start() 
    {
        // URL이란 웹서버 어떤 "자원(텍스트/이미지/사운드/데이터/API)"이 있는 위치를 가리키는 주소
        // URL 구성
        // 프로토콜     : http(s)
        // 경로(주소)   : placecats.com/bella/300/200
        // 쿼리         : ?fit=inside
        //              - ?로 시작하고, &로 구분한다. (?키1=값1&키2=값2&키3=값3);
        //              - fit=contain
        //              - position=right
        //              ㄴ옵션인데.. 매번 다르므로 웹서버개발자와 이야기를 잘 하거나 문서를 잘 봐야한다.
        Texture result = await GetWebTexture("https://placecats.com/bella/300/200?fit=inside");
        _image.texture = result;
        Debug.Log(result);
    }

    private async UniTask<Texture> GetWebTexture(string url)
    {
        try
        {
            var texture = ((DownloadHandlerTexture)(await UnityWebRequestTexture.GetTexture(url).SendWebRequest()).downloadHandler).texture;
            return texture;
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            throw;
        }

    }
}
