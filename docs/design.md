---
document_title: TaskWorkerSample 内部設計書
dest: ./output/TaskWorkerSample_内部設計書.pdf
---

<!-- 表紙 -->
<div class="cover">
  <div class="title">TaskWorkerSample 内部設計書</div>
  <div class="version">v1.0.0</div>
  <div class="date">2026-04-19</div>
  <div class="copyrights">sample Project</div>
</div>

<div class="page-break"></div>

---

本書は、TaskWorkerSample の内部構造を整理し、  
クラス構成、DI 構成、実行フローを明確にすることを目的とする。

---

# 目次

- 1. 設計方針
- 2. アーキテクチャ概要
- 3. クラス構成
- 4. DI 構成
- 5. 実行フロー
- 6. 処理モード設計
- 7. 設定ファイル設計
- 8. ログ設計
- 9. 多重起動防止設計
- 10. 拡張方針
- 11. 備考

---

# 1. 設計方針

本アプリケーションは、画面を持たないバックグラウンド実行型のコンソールアプリケーションである。  
そのため、MVC ではなく、以下の責務分離を基本とする。

- Host  
- Service  
- Model  
- Configuration  

また、非同期処理は Task / async / await を使用し、  
停止制御は CancellationToken を使用する。

---

# 2. アーキテクチャ概要

本アプリケーションは以下の構成で実装する。

```text
Program
  ↓
Configuration / Logger 初期化
  ↓
DI Container 構築
  ↓
Mutex による多重起動防止
  ↓
ExecutionServiceFactory
  ↓
ExecutionService 実行
  ↓
ログ出力・終了
```

---

# 3. クラス構成

## 3.1 全体構成
```text
Program
  ↓
IExecutionServiceFactory
  ↓
IExecutionService
  ├─ ContinuousExecutionService
  ├─ ImmediateExitService
  ├─ TimedExitExecutionService
  └─ ExceptionExecutionService

IExecutionService
  ↓
IPersonFactory
  ↓
Person
  ├─ JapanesePerson
  └─ AmericanPerson

Program
  ↓
AppSettings
  └─ LoggingSettings

Program
  ↓
MutexHelper
```

## 3.2 各クラスの役割

Program
- アプリケーションのエントリポイント
- 設定ファイル読込
- ロガー初期化
- DI 構築
- Mutex による多重起動チェック
- 実行モードに応じたサービス起動

AppSettings
- アプリケーション全体の設定値を保持する

LoggingSettings
- ログ出力先、保持日数などログ設定を保持する

IExecutionService
- 実行モードごとの処理サービスの共通インターフェース

ContinuousExecutionService
- 一定間隔で最大10分継続実行する処理

ImmediateExitService
- 起動後にメッセージを出力して即時終了する処理

TimedExitExecutionService
- 指定時間経過後に終了する継続実行処理

ExceptionExecutionService
- 異常系確認のために例外を発生させる処理

IExecutionServiceFactory
- 実行モードに応じたサービスを返すファクトリ

Person
- 挨拶メッセージを提供する基底モデル

JapanesePerson / AmericanPerson
- 言語ごとの挨拶メッセージを提供する派生モデル

IPersonFactory
- 言語設定に応じた Person を生成する

IClock / SystemClock
- 現在時刻の取得を抽象化し、テスト容易性を確保する

MutexHelper
- 多重起動防止のための Mutex 制御を補助する

LoggerConfigurator
- Serilog の初期設定を担当する

---

# 4. DI 構成

本アプリケーションでは、Microsoft.Extensions.DependencyInjection を用いて
依存関係を管理する。

## 4.1 登録方針

- 設定値は Singleton
- ロガーは Singleton
- Factory は Singleton
- 実行サービスは Transient
- 時刻取得は Singleton

## 4.2 DI構成図

```
ServiceCollection
  ├─ AppSettings                -> Singleton
  ├─ ILogger                    -> Singleton
  ├─ IClock                     -> SystemClock
  ├─ IPersonFactory             -> PersonFactory
  ├─ IExecutionServiceFactory   -> ExecutionServiceFactory
  ├─ ContinuousExecutionService -> Transient
  ├─ ImmediateExitService       -> Transient
  ├─ TimedExitExecutionService  -> Transient
  └─ ExceptionExecutionService  -> Transient
```

---

# 5. 実行フロー

1. Program 起動
2. appsettings.json 読込
3. 外部設定の読込（必要に応じて ProgramData 側を上書き）
4. LoggerConfigurator によるロガー初期化
5. DI コンテナ構築
6. Mutex による多重起動チェック
7. ExecutionMode を解釈
8. IExecutionServiceFactory から対象サービス取得
9. 対象サービス ExecuteAsync 実行
10. 終了ログ出力
11. 終了コード返却

---

# 6. 処理モード設計

処理モードは設定ファイルにより切り替える。

## 6.1 Continuous
- 最大10分間継続実行
- 指定秒数ごとに処理を実行
- 開始から1分後に挨拶ログを出力

## 6.2 Immediate

- 起動時にメッセージを出力し終了

## 6.3 TimedExit
- 基本は継続実行
- 設定値 TimeoutMinutes 到達時に終了

## 6.4 Exception
- 意図的に例外を発生させる
- Error ログを出力し異常終了する

---

# 7. 設定ファイル設計

## 7.1 配置方針
- 開発時
  プロジェクト配下の appsettings.json
- 運用時
  C:\ProgramData\Company\AppName\config

## 7.2 設定項目

| 項目名                   | 内容       |
| --------------------- | -------- |
| ExecutionMode         | 実行モード    |
| Language              | 挨拶言語     |
| TimeoutMinutes        | 処理終了時間   |
| IntervalSeconds       | 処理間隔（秒）  |
| MutexKey              | 多重起動防止キー |
| Logging.DirectoryPath | ログ出力先    |
| Logging.RetentionDays | ログ保持日数   |

---

# 8. ログ設計
## 8.1 出力内容
- アプリケーション開始
- アプリケーション終了
- 継続処理の進捗
- 挨拶実行
- 例外発生

## 8.2 ログレベル
- Information
- 通常動作、開始、終了、挨拶、進捗
- Error
- 例外発生時

## 8.3 出力先

```
C:\ProgramData\Company\AppName\logs
```

## 8.4 ローテーション
- 日次ローテーション
- 保持日数は設定ファイルにより制御

---

# 9. 多重起動防止設計

多重起動防止は Mutex により実現する。

## 9.1 方針
- 実行開始時に Mutex を取得する
- 取得できない場合は重複起動とみなし終了する
- Mutex キーは設定ファイルで管理する

# 9.2 目的
- タスク重複起動の防止
- 継続処理中の二重実行防止
- データ更新系処理の安全性確保

---

# 10. 拡張方針

本アプリケーションは以下の拡張を想定する。

- 実行モードの追加
- Person 派生クラスの追加
- 外部サービス呼出処理の追加
- 状態ファイル出力機能の追加
- 監視UI（Blazor等）との連携
- 
追加時は、既存の Program に直接ロジックを追加せず、
IExecutionService 実装クラスとして分離することを原則とする。

---

# 11. 備考

本設計は、Windowsアプリ運用標準の実装例として整理したものである。
Thread を用いた構成も可能ではあるが、本設計では Task ベースの非同期処理を採用する。

---

# 改訂履歴

| 版数 | 日付 | 内容 |
|---|---|---|
| v1.0.0 | 2026-04-19 | 初版 |
