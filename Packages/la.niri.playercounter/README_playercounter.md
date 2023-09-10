# プレイヤーカウンター v1.3.1
## はじめに
- 本製品はUdon及びUdonSharpを用いて作成されたVRChat SDK3向けギミックです
- 主な機能としてワールド内に存在するプレイヤー数をカウントし、表示を行います。

## 必須ライブラリ
- [VRChat SDK3 - World](https://vrchat.com/home/download)

導入時、最新版か確認を行ってください。

移行の際はそのまま上書きインポートを行えます。

## 導入方法
1. BoothやGithubにて最新バージョンを入手してください
2. 必須ライブラリを導入後、ダウンロードした.unitypackageをUnityにインポートを行ってください。
3. ```Assets/Kinel/Counter/Prefab```内のprefabを選択しシーンに配置します。
4. 位置調整後Prefabを選択し、Inspector内に存在する```Udon Behaviour```内のLimitをワールド人数上限に設定してください。

## ドキュメント

## ライセンス
- MIT License