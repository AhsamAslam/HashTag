using EmergenceGuardian.FFmpeg;
using System;

namespace FFmpegLiabrary
{
    public class CallFfmpeg
    {
        public static string CallProcess(string config, string query)
        {
            FFmpegConfig.FFmpegPath = config;

            FFmpegProcess ffmpeg = new FFmpegProcess(null);
            CompletionStatus status = ffmpeg.RunFFmpeg(query);
            if (status.ToString() == "Success")
            {
                return status.ToString();
            }
            else
            {
                return ffmpeg.Output;
            }
        }
    }
}
