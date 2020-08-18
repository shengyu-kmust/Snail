using System;
using System.Collections.Generic;
using System.Text;

namespace Snail.Speech
{
    /// <summary>
    /// 语音合成接口
    /// </summary>
    public interface ISpeechSynthesis
    {
        /// <summary>
        /// 语音合成
        /// </summary>
        /// <param name="content">合成内容</param>
        /// <param name="eAudioType">合成音频类型</param>
        /// <returns>音频数据</returns>
        byte[] Synthesis(string content, EAudioType eAudioType);
    }
}
