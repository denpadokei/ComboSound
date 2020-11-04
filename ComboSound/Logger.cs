using IPA.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ComboSound
{
    public static class Logger
    {
        private static IPA.Logging.Logger _logger = Plugin.Log;
        public static void Debug(string message, [CallerFilePath] string path = null, [CallerMemberName] string member = null, [CallerLineNumber] int? num = null)
        {
            _logger.Debug($"{Path.GetFileName(path)}[{member}({num})] : {message}");
        }
        
        public static void Debug(Exception e, [CallerFilePath] string path = null, [CallerMemberName] string member = null, [CallerLineNumber] int? num = null)
        {
            _logger.Debug($"{Path.GetFileName(path)}[{member}({num})] : {e}");
        }
        
        public static void Error(Exception e, [CallerFilePath] string path = null, [CallerMemberName] string member = null, [CallerLineNumber] int? num = null)
        {
            _logger.Error($"{Path.GetFileName(path)}[{member}({num})] : {e}");
        }
        
        public static void Error(string message, [CallerFilePath] string path = null, [CallerMemberName] string member = null, [CallerLineNumber] int? num = null)
        {
            _logger.Error($"{Path.GetFileName(path)}[{member}({num})] : {message}");
        }
        
        public static void Info(string message, [CallerFilePath] string path = null, [CallerMemberName] string member = null, [CallerLineNumber] int? num = null)
        {
            _logger.Info($"{Path.GetFileName(path)}[{member}({num})] : {message}");
        }
        
        public static void Info(Exception e, [CallerFilePath] string path = null, [CallerMemberName] string member = null, [CallerLineNumber] int? num = null)
        {
            _logger.Info($"{Path.GetFileName(path)}[{member}({num})] : {e}");
        }
        
        public static void Notice(Exception e, [CallerFilePath] string path = null, [CallerMemberName] string member = null, [CallerLineNumber] int? num = null)
        {
            _logger.Notice($"{Path.GetFileName(path)}[{member}({num})] : {e}");
        }
        
        public static void Notice(string message, [CallerFilePath] string path = null, [CallerMemberName] string member = null, [CallerLineNumber] int? num = null)
        {
            _logger.Notice($"{Path.GetFileName(path)}[{member}({num})] : {message}");
        }

    }
}
