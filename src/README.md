# TaskWorkerSample

Windows環境でのバッチ／タスク実行アプリの  
**実運用を意識したサンプルプロジェクト**です。

---

## ■ 概要

本プロジェクトは、以下の課題を解決することを目的としています。

- タスクスケジューラ運用の属人化
- ログ出力のばらつき
- 設定ファイルの配置不統一
- 多重起動や停止制御の未整備

これらに対して、

👉 **設定・ログ・制御・実行を分離した構成**

を実装例として提供します。

---

## ■ 特徴

- 設定ファイルによる実行制御
- ログ出力の標準化（Serilog）
- 多重起動防止（Mutex）
- 停止要求ファイルによる制御
- 実行モード切替（Immediate / Continuous / TimedExit / Exception）
- インストーラ前提の構成
- タスクスケジューラ運用を前提とした設計

---

## ■ ディレクトリ構成

### 開発時（Debug）

```
bin/Debug/net10.0/
  ├─ config/
  │   └─ appsettings.json
  ├─ logs/
  └─ control/
```

### 本番（インストール後）

```
C:\ProgramData\Company\AppName\
  ├─ config/
  │   └─ appsettings.json
  ├─ logs/
  └─ control/
```

---

## ■ 設定ファイル

```json
{
  "ExecutionMode": "Continuous",
  "Language": "ja",
  "IntervalSeconds": 10,
  "TimeoutMinutes": 10,
  "MutexKey": "SampleMutex",
  "Logging": {
    "DirectoryPath": "",
    "RetentionDays": 30
  }
}
```

---

## ■ 起動方法

### ① デフォルト（Debug用）

```
TaskWorkerSample.exe
```

---

### ② 設定ファイル指定（推奨）

```
TaskWorkerSample.exe --config "C:\ProgramData\Company\AppName\config\appsettings.json"
```

---

## ■ 実行モード

| モード | 内容 |
|------|------|
| Immediate | 即時終了 |
| Continuous | 継続実行 |
| TimedExit | 指定時間で終了 |
| Exception | 異常終了 |

---

## ■ 停止制御

以下のファイルを配置することで処理を停止できます。

```
control/stop-request.flag
```

- 起動前 → 即終了
- 実行中 → 次ループで停止

---

## ■ 多重起動防止

- MutexKey により制御
- 同一キーの場合は2重起動を防止

---

## ■ ログ出力

### 出力先

- 設定ファイル指定あり → 指定パス
- 未指定 → BaseDir/logs

---

## ■ テスト

### ✔ テスト仕様書
- docs/test_spec.md

### ✔ テストシート
- docs/test_sheet.md

---

## ■ 開発環境

- .NET 10
- Visual Studio
- Serilog

---

## ■ 関連ドキュメント

- 開発標準: docs/development-guidelines.md
- 基本仕様書: docs/specifications.md
- 内部設計書: docs/design.md

---

## ■ 想定用途

- Windowsタスクスケジューラ運用
- バッチ処理の標準化
- 運用設計のサンプル
- 社内開発のベーステンプレート

---

## ■ 注意事項

本プロジェクトはサンプルです。  
実業務に適用する場合は要件に応じて調整してください。

---

## ■ 使用素材

- アプリアイコン  
  Google Fonts Icons - Schedule  
  https://fonts.google.com/icons

---

## ■ ライセンス

MIT License（または任意）
