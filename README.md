# All Every Rotation Constraint (AERC)
階層構造のあるゲームオブジェクトにRotation Constraintを簡単に追加・設定するためのエディタ拡張

ソース（コピー元）のゲームオブジェクトとターゲット（コピー先）のゲームオブジェクトを指定すると，ターゲットの子オブジェクトそれぞれに対して，

    1. 対応するゲームオブジェクトにAdd Component (Rotation Constraint)する
    2. Is Active を On にする
    3. Constraint Settingsに同じ階層に存在するソースオブジェクトのTransformを追加する

を繰り返す．

# How to Use


- UnityでUnityEditor_AERC.unitypackageをインポートする．
    - Assets直下にEditorディレクトリがあることを確認する．

- Unit上部のWindowタブにExtension Toolsが表示されるようになり，その下のAdd Every Rotation Constraintをクリックする．
![Window/ExtensionTools](https://user-images.githubusercontent.com/44863813/102008272-a8a6c580-3d72-11eb-8f91-269383dccf79.png)

- ↓のようなWindowが現れる．
![Explanation/Window](https://user-images.githubusercontent.com/44863813/102008730-f5d86680-3d75-11eb-903a-2d7aa2658e79.png)
項目の説明
    - Original Avatar: コピー元を指定する箇所
    - Target Abatar: コピー先を指定する箇所
    - GameObject(Joint): Rotation Constraintを追加したいゲームオブジェクトを指定する
    - Root Name: 指定したゲームオブジェクトの最上層の親の名前が表示される
    - Number of Joints: 指定したゲームオブジェクトとその子オブジェクトの数が表示される


# Example of Use
VRChat用のアバター改変をする時に，以下のような作業が簡単になる！

- 頭にアバターを追加し，両腕を本体と同期させる
- サブアームの追加する
- アバターのフルトラッキング適正向上（後日追記）
- 複数アバターのアニメーションを同期させる

などなど
![Result](https://user-images.githubusercontent.com/44863813/102007590-d2112280-3d6d-11eb-9870-5ca335b6973c.png)

(両腕含めて40程度のボーンがあるため，それを一つずつ設定するのは大変．．．)

# Notes
説明のため[ロポリこんちゃん](https://mido0021.booth.pm)，[稲荷原](https://booth.pm/ja/items/2297510)，[メカアーム](https://booth.pm/ja/items/1221319)を使用するが，本製品に含まれていない．

# Author
chootana (ちゅーたな)

twitter: [@choo_zap](https://twitter.com/choo_zap)

# License

AERC is released under the [MIT license](https://opensource.org/licenses/mit-license.php).