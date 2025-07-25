# unity-jenkins-mobile-build-pipeline

## 💡 概要
UnityでiOS・Androidアプリを、Jenkins上で手動ビルドできるように構成。ビルド結果はSlack通知。バージョン管理（develop, staging, product）にも対応。

## 🧱 構成
- Jenkins Pipeline
- Unity CLI Build（iOS / Android）
- iOS：Xcode Archive + fastlane
- Slack通知：Slack pluginを使用
- 手動トリガー：Jenkins UIでパラメータ指定（ブランチ、バージョンなど）

## 🗂 ディレクトリ構成
```
unity-jenkins-mobile-build-pipeline/
├── README.md
├── docs/
│   ├── architecture_diagram.png         # システム全体構成図
│   ├── slack_notification.png           # 通知イメージ
│   └── setup_guide.md                   # 導入手順詳細
├── jenkins/
│   ├── Jenkinsfile                      # Build_Binary_All用
│   ├── Build_Binary_iOS                 # iOSビルド用のPipelineスクリプト
│   ├── Build_Binary_Android.            # Androidビルド用
│   └── fastlane/
│       ├── Fastfile                     # Xcode Archive用 fastlane スクリプト
│       └── Appfile                      # fastlane設定
└── unity/
    ├── BuildClass.cs                    # Unityのビルド用C#スクリプト
    ├── DefineSymbolUtility.cs           # スクリプト定義シンボルを追加するクラス
    ├── ProjectConfiguration.cs          # ビルド前にAndroidのキーストア設定を自動で行うクラス
    └── PlayerSettingsManager/
        ├── AutoProjectTypeEnum.cs       # 開発環境（DEVELOP／RELEASE／STAGING）を列挙型として扱うクラス
        └── PartialEnum.cs               # 定義された静的プロパティから列挙値のリストを動的に取得する
```