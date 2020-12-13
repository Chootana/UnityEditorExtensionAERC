# All Every Rotation Constraint (AERC)
階層構造のあるゲームオブジェクトにRotation Constraintを簡単に追加・設定するためのエディタ拡張

ソース（コピー元）のゲームオブジェクトとターゲット（コピー先）のオブジェクトを指定すると，ターゲットの子オブジェクトそれぞれに対して，

    1. 対応するゲームオブジェクトにAdd Component (Rotation Constraint)する
    2. Is Active を On にする
    3. Constraint Settingsに同じ階層に存在するソースオブジェクトのTransformを追加する

を繰り返す行う．

# How to Use
![Result](https://user-images.githubusercontent.com/44863813/102007590-d2112280-3d6d-11eb-9870-5ca335b6973c.png)



# Author
chootana (ちゅーたな)

twitter: [@choo_zap](https://twitter.com/choo_zap)

# License

AERC is released under the [MIT license](https://opensource.org/licenses/mit-license.php).