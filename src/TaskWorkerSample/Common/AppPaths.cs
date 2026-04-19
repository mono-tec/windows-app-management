using System;
using System.IO;

namespace TaskWorkerSample.Common;

/// <summary>
/// アプリケーションで使用する各種パスを表します。
/// </summary>
public sealed class AppPaths
{
    /// <summary>
    /// ベースディレクトリを取得します。
    /// </summary>
    public string BaseDir { get; }

    /// <summary>
    /// 設定ディレクトリを取得します。
    /// </summary>
    public string ConfigDir => Path.Combine(BaseDir, "config");

    /// <summary>
    /// ログディレクトリを取得します。
    /// </summary>
    public string LogsDir => Path.Combine(BaseDir, "logs");

    /// <summary>
    /// 制御ディレクトリを取得します。
    /// </summary>
    public string ControlDir => Path.Combine(BaseDir, "control");

    /// <summary>
    /// 停止フラグファイルのパスを取得します。
    /// </summary>
    public string StopFlagPath => Path.Combine(ControlDir, "stop-request.flag");

    /// <summary>
    /// 外部設定ファイルのパスを取得します。
    /// </summary>
    public string ConfigFilePath => Path.Combine(ConfigDir, "appsettings.json");

    /// <summary>
    /// ベースディレクトリを指定して初期化します。
    /// </summary>
    /// <param name="baseDir">ベースディレクトリです。</param>
    public AppPaths(string baseDir)
    {
        if (string.IsNullOrWhiteSpace(baseDir))
        {
            throw new ArgumentException("ベースディレクトリが未指定です。", nameof(baseDir));
        }

        BaseDir = baseDir;
    }

    /// <summary>
    /// 設定ファイルパスから AppPaths を生成します。
    /// </summary>
    /// <param name="configFilePath">設定ファイルパスです。</param>
    /// <returns>AppPaths を返します。</returns>
    public static AppPaths FromConfigPath(string configFilePath)
    {
        string? configDir = Path.GetDirectoryName(configFilePath);
        if (string.IsNullOrWhiteSpace(configDir))
        {
            throw new InvalidOperationException("設定ファイルのディレクトリを取得できません。");
        }

        DirectoryInfo? parent = Directory.GetParent(configDir);
        if (parent is null)
        {
            throw new InvalidOperationException("ベースディレクトリを取得できません。");
        }

        return new AppPaths(parent.FullName);
    }
}