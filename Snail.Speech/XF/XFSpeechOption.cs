namespace Snail.Speech.XF
{
    public class XFSpeechOption
    {
        /// <summary>
        /// appid
        /// </summary>
        public string AppId { get; set; }
        /// <summary>
        /// 语音合成的配置参数
        /// 示例：engine_type = local, voice_name = xiaoyan, tts_res_path = fo|res\\tts\\xiaoyan.jet;fo|res\\tts\\common.jet, sample_rate = 8000
        /// </summary>
        public string SessionBeginParams { get; set; }
    }
}
