# About
Unity 1週間ゲームジャム お題「正」

[「ANTreasure」](https://unityroom.com/games/antreasure)のソースコードのみを抽出したリポジトリです。

## External Assets
- [VContainer](https://github.com/hadashiA/VContainer)
- [UniRx](https://github.com/neuecc/UniRx)
- [UniTask](https://github.com/Cysharp/UniTask)
- [DOTween Pro](http://dotween.demigiant.com)
- [UIEffect](https://github.com/mob-sakai/UIEffect)
- [AudioPlayer](https://github.com/kameffee/AudioPlayer)
- [CSV Serialize](https://assetstore.unity.com/packages/tools/integration/csv-serialize-135763)

# 説明

## EntryPoint

### MapLoop
- マップ上のキャラクターの動き
- バトルへの遷移
- バトル結果によるステータス変化イベントやカードの交換イベントの発火

### GameLoop
- バトル前と後の会話イベント
- プレイヤー、敵の初期化
- BattleLoopの開始
- バトル結果後の会話イベント

### BattleLoop
- GameLoopから呼び出される
- バトルの一連の流れを行う

## LifetimeScope
```
RootLifetimeScope
 ┣━ TitleLifetimeScope
 ┣━ MapLifetimeScope
 ┗━ InGameLifetimeScope
     ┣━ PlayerLifetimeScope
     ┗━ EnemyLifetimeScope
```
