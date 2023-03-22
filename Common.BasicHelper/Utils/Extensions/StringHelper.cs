﻿using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Common.BasicHelper.Utils.Extensions;

public static class StringHelper
{
    public static char[] A_Z = new char[26]
    {
        'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N',
        'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'
    };

    /// <summary>
    /// 将数字转换成大写字母
    /// </summary>
    /// <param name="source">源字符串</param>
    /// <returns>转换后字符串</returns>
    public static string Num2UpperChar(this string source)
    {
        var sb = new StringBuilder();
        for (int i = 0; i < source.Length; ++i)
            if (source[i] >= '0' && source[i] <= '9')
                sb.Append(A_Z[source[i] - '0']);
            else sb.Append(source[i]);
        return sb.ToString();
    }

    /// <summary>
    /// 从磁盘读取全部文本
    /// </summary>
    /// <param name="path">文件路径</param>
    /// <returns>文本内容, 若文件不存在则返回空</returns>
    public static string ReadAllTextFromDisk(this string path)
    {
        if (File.Exists(path))
            return File.ReadAllText(path);
        else return null;
    }

#if NETSTANDARD2_1_OR_GREATER

    /// <summary>
    /// 从磁盘异步读取全部文本
    /// </summary>
    /// <param name="path">文件路径</param>
    /// <returns>文本内容读取任务, 若文件不存在则返回空</returns>
    public static Task<string> ReadAllTextFromDiskAsync(this string path)
    {
        if (File.Exists(path))
            return File.ReadAllTextAsync(path);
        else return null;
    }

#endif

}
