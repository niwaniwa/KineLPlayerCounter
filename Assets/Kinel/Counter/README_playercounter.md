# プレイヤーカウンター v1.1
## はじめに
- 本製品はUdon及びUdonSharpを用いて作成されたVRChat SDK3向けギミックです
- 主な機能としてワールド内に存在するプレイヤー数をカウントし、表示を行います。

## 更新履歴
- 1.0 初版
- 1.1 Unity2019対応しました。各種最適化とUdonSharp非依存化しました。

## 必須ライブラリ
- [VRChat SDK3 - World](https://vrchat.com/home/download)

導入時、最新版か確認を行ってください。

v1.1では移行の際は旧バージョンが残ったまま、そのままインポートを行ってください。

## 導入方法
1. Releaseにて最新バージョンを入手してください
2. 必須ライブラリを導入後、ダウンロードした.unitypackageをUnityにインポートを行ってください。
3. ```Assets/Kinel/Counter/Prefab```内のprefabを選択しシーンに配置します。
4. 位置調整後Prefabを選択し、Inspector内に存在する```Udon Behaviour```内のLimitをワールド人数上限に設定してください。

## ライセンス
- MIT License