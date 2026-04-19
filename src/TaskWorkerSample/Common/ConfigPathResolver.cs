using System;
using System.IO;

namespace TaskWorkerSample.Common;

/// <summary>
/// 設定ファイルパスを解決します。
/// </summary>
internal static class ConfigPathResolver
{
    private const string ConfigArgumentName = "--config";
    private const string ConfigEnvironmentVariable = "TASKWORKERSAMPLE_CONFIG_PATH";

    /// <summary>
    /// 設定ファイルパスを解決します。
    /// 優先順位：
    /// ① コマンドライン引数
    /// ② 環境変数
    /// ③ カレントディレクトリ
    /// </summary>
    public static string Resolve(string[] args)
    {
        // ① コマンドライン引数
        string? pathFromArgs = GetPathFromArgs(args);
        if (!string.IsNullOrWhiteSpace(pathFromArgs))
        {
            return pathFromArgs;
        }

        // ② 環境変数
        string? pathFromEnv = Environment.GetEnvironmentVariable(ConfigEnvironmentVariable);
        if (!string.IsNullOrWhiteSpace(pathFromEnv))
        {
            return pathFromEnv;
        }

        // ③ カレントディレクトリ
        return Path.Combine(AppContext.BaseDirectory, "config", "appsettings.json");
    }

    private static string? GetPathFromArgs(string[] args)
    {
        int index = Array.IndexOf(args, ConfigArgumentName);
        if (index >= 0 && index < args.Length - 1)
        {
            return args[index + 1];
        }

        return null;
    }
}