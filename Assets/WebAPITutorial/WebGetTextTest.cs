using UnityEngine;
using System.Collections;
using Cysharp.Threading.Tasks;
using UnityEngine.Networking;


public class WebGetTextTest : MonoBehaviour
{
    // HTTP 프로토콜을 이용해서 웹 서버에게 데이터 작업을 요청할 수 있다.
    // 작업 요청은 크~게 4가지 약속이 있다.
    // 1. 데이터 내놔        : GET
    // 2. 내 데이터 줄게      : Post
    // 3. 데이터 수정해줘      : PUT
    // 4. 데이타 삭제해줘      : DELETE

    private async void Start()
    {
        string result = await GetWebText("https://www.google.com/search?q=%EB%8B%AC&oq=%EB%8B%AC&gs_lcrp=EgZjaHJvbWUyBggAEEUYOTINCAEQABiDARixAxiABDINCAIQABiDARixAxiABDINCAMQABiDARixAxiABDITCAQQLhiDARjHARixAxjRAxiABDINCAUQABiDARixAxiABDINCAYQABiDARixAxiABDITCAcQLhiDARjHARixAxjRAxiABDINCAgQABiDARixAxiABDINCAkQABiDARixAxiABNIBBzQzM2owajeoAgCwAgA&sourceid=chrome&ie=UTF-8");
        Debug.Log(result);
    }

    private async UniTask<string> GetWebText(string url)
    {
        var txt = (await UnityWebRequest.Get(url).SendWebRequest()).downloadHandler.text;
        return txt;
    }
}