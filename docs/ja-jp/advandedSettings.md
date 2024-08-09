<h1> 拡張設定 </h1>

# 概要
エリア機能について設定できます

## エリア機能とは
任意の範囲に設定したコライダー内の人数を表示することができます。

### 設定方法
PlayerCounter.prefab内の設定でエリア機能を有効にします。
PlayerCountArea.prefabのBoxColliderを任意のエリアに設定することで、そのCollider内に入ったプレイヤーを表示することができます。
複数のAreaを指定することができます。

## PlayerCounter.prefab
---
|名称|説明|type|
|--|--|:--:|
|エリア機能|エリア機能を有効にします||
|Areas|カウントしたいエリアを指定します|PlayerCountArea|

## PlayerCountArea.prefab
---
|名称|説明|type|
|--|--|:--:|
|Debug|コンソールにデバッグメッセージを表示します|boolean|