# 手順
## 1. Jenkinsの設定（mac）
1. Jenkinsのインストール
   - Jenkinsのインストール前にJavaとHomebrewをインストールしていなければインストールすること。やり方は割愛する。
   - Jenkinsをインストール
      ```
      brew install jenkins-lts
      ```
   - インストールしたらadminパスワードが出てくるのでメモしておくこと。
   - メモし忘れた場合はUnlock Jenkinの画面でパスワードが保存されているパスが表示されるのでそれを参照する。

2. Jenkinsの起動
   - Jenkinsを下記コマンドで起動する。
      ```
      brew services start jenkins
      ```
   - http://localhost:8080/　にアクセス
   - アクセスするとパスワードを聞かれるので先ほどのパスワードを入力する
   - Install suggested pluginsを選択。
   - ユーザーは作成せずadminユーザーとしてログインする

3. プラグインのインストールSSH鍵設定
   - プラグインGitHubをインストールする
     - Git Plugin
     - Slack Notification Plugin(任意)
   - SSH鍵 をJenkinsに登録（GitHub接続用）

4. Jobの作成 設定
   - 共通設定（全ジョブ）
     - 「ダッシュボード > 新規ジョブ作成 > パイプライン」
     - パラメータ設定：
       - Git Parameter：SelectBranch（デフォルト：origin/develop）
       - テキスト：VERSION（例：0.0.1）
       - 選択：BuildType（DEVELOP / STAGING / RELEASE）
     - 「古いビルドの破棄」ON
     - 「Do not allow concurrent builds」ON  
   - Androidビルド : Build_Binary_Android
     - パイプライン定義：Pipeline script
     - スクリプト内容：jenkins/Build_Binary_Android の内容を貼り付け 
   - iOSビルド：Build_Binary_iOS
     - パイプライン定義：Pipeline script
     - スクリプト内容：jenkins/Build_Binary_iOS の内容を貼り付け 
   - 両方ビルド（親ジョブ）：Build_Binary_All
     - パイプライン定義：Pipeline script from SCM
       - SCM：Git
       - URL：対象リポジトリURL
       - 認証情報：SSH鍵
       - ブランチ指定：*/master
       - Script Path：jenkins/Jenkinsfile
## 2. Gitリポジトリの作成
1. Gitリポジトリの作成 デフォルトをMasterにする
2. Gitリポジトリの構成
```     
├── README.md                            
├── Unityプロジェクト/                     
│   ├── Assets/                          
│   ├── Keystore/                        # Androidビルド用Keystore
│   ├── Packages/                        
│   └── ProjectSetting/                  
├── Tool/fastlane/                       
│   ├── Fastfile                         # Xcode Archive用
│   └── Appfile                          
└── Jenkinsfile                          # 両方ビルド用Jenkinsfile
```
## 3. Unityのセットアップ（mac）
1. Unityのインストール
2. Unity新規プロジェクトを上記Gitリポジトリの構成場所に作成
## 4. fastlane のセットアップ
1. FastlaneをMacにインストール
2. Appstore DeveloperからProvisioningProfileをMacにダウンロード任意の場所に保存
3. Fastfileを作成
    - jenkins/fastlane/Fastfileの設定
    - 上記Gitリポジトリの構成場所に保存
## 5. Keystore（Android）の配置
1. Unityを開きProject Setting>Player>Publishing Settings からKeystoreの作成
2. Keystoreを上記Gitリポジトリの構成Keystore直下に保存
## 6. Slack通知の設定
### Slack側の設定
1. Slack Appの作成
    1. ここにアクセス → https://api.slack.com/apps
    2. 「Create New App」からアプリ作成
    3. 「From scratch」→ 名前は何でもOK、ワークスペースを選択
2. Bot に通知権限をつける
    - 左メニューの「OAuth & Permissions」→ Bot Token Scopes に以下を追加：
       - chat:write
3. Bot Token をコピー
    - Install to Workspace > Allow > xoxb-... で始まる Bot User OAuth Token をコピー
4. 通知したいチャンネルにBotを招待
   - Slackアプリで通知したいチャンネルに移動
   ```     
   /invite @作成したBot名
   ```
### Jenkins側の設定
1. Slack Plugin をインストールしてなければインストール
2. Credentials に Bot Token を登録
    1. 「Jenkinsの管理 > Credentials > グローバルドメイン」へ
    2. 「Add Credentials」→ 種類：Secret text
       - Secret：さっきの xoxb-... Bot Token
       - ID：例 slack-bot-token
3. JenkinsのSlack設定
    1. 「Jenkinsの管理 > System」へ
    2. 下のほうに「Slack」セクションを設定する
    ------------------------------------------------------
    | 項目              | 値                              |
    | --------------- | ------------------------------ |
    | Workspace       | `https://[ワークスペース名].slack.com` |
    | Credential      | さっきの `slack-bot-token` を選択     |
    | Default Channel | `#通知したいチャンネル名`（例：`#build`）     |
    | Send as         | 任意（Jenkinsとか）                  |
    | Test Connection | ← 押して「Success」ならOK！            |
    ------------------------------------------------------

## 実行手順
1. Jenkinsトップページから `Build_Binary_All` ジョブを選択
2. 「ビルド実行」ボタンを押下
3. 入力パラメータで以下を指定：
   - SelectBranch：`develop`
   - BuildType：`DEVELOP`
   - VERSION：`1.0.0`
4. 成功時は Slack に通知が届きます。
