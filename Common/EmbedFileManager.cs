using System;
using System.IO;
using System.Reflection;

namespace Launcher.Common
{
    internal class EmbedFileManager
    {
        /// <summary>
        /// 释放内嵌资源至指定位置
        /// </summary>
        /// <param name="resource">嵌入的资源，此参数写作：命名空间.文件夹名.文件名.扩展名</param>
        /// <param name="path">释放到位置</param>
        public static void ExtractFile(string file, string path)
        {
            var resource = $"Launcher.{file}";
            Assembly assembly = Assembly.GetExecutingAssembly();
            BufferedStream input = new BufferedStream(assembly.GetManifestResourceStream(resource));
            FileStream output = new FileStream(path, FileMode.Create);
            byte[] data = new byte[1024];
            int lengthEachRead;
            while ((lengthEachRead = input.Read(data, 0, data.Length)) > 0)
            {
                output.Write(data, 0, lengthEachRead);
            }
            output.Flush();
            output.Close();
        }


    }

    public static class RawFileHelper
    {
        public static byte[] GetKey(string file)
        {
            Stream sr = null; ;
            try
            {
                var _assembly = Assembly.GetExecutingAssembly();//获取当前执行代码的程序集
                sr = _assembly.GetManifestResourceStream($"Launcher.RSAPatch.{file}");

            }
            catch
            {
                //AConsole.e(new Spectre.Console.Markup("访问资源错误"));
                throw;
            }

            return streamToByteArray(sr);
        }

        private static byte[] streamToByteArray(Stream input)
        {
            MemoryStream ms = new MemoryStream();
            input.CopyTo(ms);
            return ms.ToArray();
        }
    }

    public static class RSAPatchHelper
    {
        public static string WriteMhypbaseAllTo(string path1, Model.ServerItem item)
        {
            WriteDllTo(path1);
            WriteInITo(path1, item);
            return Path.Combine(path1, "rsa.dll");
        }


        public static void WriteDllTo(string path1)
        {

            try
            {
                //EmbedFileManager.ExtractFile("mhypbase.mhypbase.cr.dll",file);
                EmbedFileManager.ExtractFile("RSAPatch.RSAPatch.dll", Path.Combine(".\\", "rsa.dll"));

            }
            catch
            {
                throw;
            }

        }

        public static void WriteInITo(string path1, Model.ServerItem item)
        {
            try
            {
                

                if (App.launcherConfig.DebugMode)
                {
                    // debug
                }



                if (!string.IsNullOrEmpty(item.RSAPrivateKey))
                {
                    File.WriteAllText("PrivateKey.txt", item.RSAPrivateKey);
                }
                if (!string.IsNullOrEmpty(item.RSAPublicKey))
                {
                    File.WriteAllText("PublicKey.txt", item.RSAPublicKey);
                }
                else
                {
                    // use default
                    File.WriteAllBytes("PublicKey.txt", RawFileHelper.GetKey("PublicKey.txt"));
                }


            }
            catch
            {
                throw;
            }

        }

        private static byte[] streamToByteArray(Stream input)
        {
            MemoryStream ms = new MemoryStream();
            input.CopyTo(ms);
            return ms.ToArray();
        }
    }
}
