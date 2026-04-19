---
document_title: TaskWorkerSample テストシート
dest: ./output/TaskWorkerSample_テストシート.pdf
---

<!-- 表紙 -->
<div class="cover">
  <div class="title">TaskWorkerSample テストシート</div>
  <div class="version">v1.0.0</div>
  <div class="date">2026-04-19</div>
  <div class="copyrights">sample Project</div>
</div>

<div class="page-break"></div>

---

本シートは、TaskWorkerSample の動作確認を効率的に実施するためのチェックリストです。  
実施結果を記録し、GitHub に保存することを想定しています。

---

## ■ テスト基本情報

| 項目 | 内容 |
|---|---|
| 実施日 |2026-04-19 |
| 実施者 |mono-tec |
| バージョン | |
| 環境 | Windows 11 |

---

## ■ テスト一覧

| No | テスト項目 | 手順 | 期待結果 | 結果 | 備考 |
|---|---|---|---|---|---|
| 1 | 起動確認 | 引数なしで起動 | 正常に起動・終了する |合格 | |
| 2 | Immediateモード | ExecutionMode=Immediate | 即時終了する |合格 | |
| 3 | Exceptionモード | ExecutionMode=Exception | エラー終了する |合格 | |
| 4 | Continuousモード | IntervalSeconds=1 | ループ実行される |合格| |
| 5 | TimedExitモード | TimeoutMinutes=1 | 指定時間で終了 |合格| |
| 6 | 設定ファイル引数 | --config指定 | 指定設定が反映 |合格| |
| 7 | デフォルト設定 | 引数・環境変数なし | exe配下設定使用 |合格| |
| 8 | ログ出力（指定あり） | Logging.DirectoryPath指定 | 指定先に出力 |未実施 | |
| 9 | ログ出力（未指定） | 未設定 | BaseDir\logsに出力 | 未実施| |
| 10 | 停止フラグ（起動前） | flag作成後起動 | 即終了 |合格| |
| 11 | 停止フラグ（実行中） | 実行中にflag作成 | 次ループで停止 |合格| |
| 12 | 多重起動（同一キー） | 2重起動 | 2つ目が終了 |合格| |
| 13 | 多重起動（別キー） | 別設定で起動 | 並行実行可能 |未実施 | |
| 14 | インストーラ確認 | インストール実行 | config/logs/control生成 |未実施 | |
| 15 | タスク確認 | タスク登録確認 | 引数設定済み |未実施 | |

---

## ■ チェック観点

- 設定ファイルの読み込み順序が正しいか
- ログ出力先が想定どおりか
- 停止フラグが確実に機能するか
- Mutex による多重起動防止が機能するか
- インストーラごとに環境が分離されているか

---

## ■ 総合評価

| 評価 | コメント |
|---|---|
| 合格 / 不合格 | |

---

## ■ 改訂履歴

| 版数 | 日付 | 内容 |
|---|---|---|
| v1.0.0 | 2026-04-19 | 初版 |
