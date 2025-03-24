using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public class PermissionChecking : MonoBehaviour
{
    // Start is called before the first frame update
    IEnumerator Start()
    {
        // カメラパーミッションが許可されているか調べる
        if (!Permission.HasUserAuthorizedPermission (Permission.Camera)) {
            // 権限が無いので、カメラパーミッションのリクエストをする
            yield return RequestUserPermission (Permission.Camera);
        }
        // マイクパーミッションが許可されているか調べる
        if (!Permission.HasUserAuthorizedPermission (Permission.Microphone)) {
            // 権限が無いので、マイクパーミッションのリクエストをする
            yield return RequestUserPermission (Permission.Microphone);
        }
        // リクエストの結果、アプリ機能に必要なパーミッションが全て許可されたか調べる
        if (Permission.HasUserAuthorizedPermission (Permission.Camera) && Permission.HasUserAuthorizedPermission (Permission.Microphone)) {
            // 権限が許可されたので、権限が必要なAPIを使用する処理へ進む
            findWebCams ();
            findMicrophones ();
        } else {
            // 権限が許可されなかったので、ユーザーに対して権限の使用用途の説明を表示してから再度のリクエストを行う。
            // もしも拒否時に「今後表示しない」がチェックされた場合は、次回からリクエスト自体が表示されなくなる、
            // そのためユーザーには自分でOSのアプリ設定画面で権限許可を行うようにアナウンスする必要がある。
            // （Permissionクラスにはそれらの違いを調べる方法は用意されていない）
        }
    }

    IEnumerator OnApplicationFocus(bool hasFocus)
    {
        // iOSプラットフォームでは1フレーム待つ処理がないと意図通りに動かない。
        yield return null;

        if (isRequesting && hasFocus)
        {
            isRequesting = false;
        }
    }

    bool isRequesting;

    IEnumerator RequestUserPermission(string permission)
    {
        isRequesting = true;
        Permission.RequestUserPermission(permission);
        // Androidでは「今後表示しない」をチェックされた状態だとダイアログは表示されないが、フォーカスイベントは通常通り発生する模様。
        // したがってタイムアウト処理は本来必要ないが、万が一の保険のために一応やっとく。

        // アプリフォーカスが戻るまで待機する
        float timeElapsed = 0;
        while (isRequesting)
        {
            if (timeElapsed > 0.5f){
                isRequesting = false;
                yield break;
            }
            timeElapsed += Time.deltaTime;

            yield return null;
        }
        yield break;
    }

    void findWebCams ()
    {
        foreach (var device in WebCamTexture.devices) {
            Debug.Log ("Name: " + device.name);
        }
    }

    void findMicrophones ()
    {
        foreach (var device in Microphone.devices) {
            Debug.Log ("Name: " + device);
        }
    }
}
