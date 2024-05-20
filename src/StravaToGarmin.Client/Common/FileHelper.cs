using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace StravaToGarmin.Client.Common
{
    public class FileHelper
    {
        /// <summary>
        /// 读取文件内容,小文件
        /// </summary>
        public static string ReadAllText(string path)
        {
            try
            {
                return File.ReadAllText(path).Trim();
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 读取key value
        /// </summary>
        public static string ReadKeyValue(string path, string key)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(path);
                XmlNode node = doc.SelectSingleNode(@"//add[@key='" + key + "']");
                XmlElement element = (XmlElement)node;
                if (element == null)
                    return string.Empty;
                else
                    return element.GetAttribute("value");
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 保存key value
        /// </summary>
        public static void SaveKeyValue(string path, string key, string value)
        {
            var doc = new XmlDocument();
            doc.Load(path);

            var node = doc.SelectSingleNode(@"//add[@key='" + key + "']");
            var element = (XmlElement)node;
            element.SetAttribute("value", value);

            while (true)
            {
                try
                {
                    doc.Save(path);

                    Thread.Sleep(100);
                    if (string.IsNullOrWhiteSpace(File.ReadAllText(path)))
                        Console.WriteLine("修改配置保存后配置文件为空,正在重试...");
                    else
                        break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("修改配置保存时异常,正在重试...{0}", ex.Message);
                }
            }
        }

        /// <summary>
        /// 读取文件
        /// </summary>
        public static byte[] GetFile(string filePath)
        {
            //直接读取文件
            var data = new List<byte>();
            var buffer = new byte[1024 * 1024];
            int length = 0;
            FileStream file = new FileStream(filePath, FileMode.Open);
            while ((length = file.Read(buffer, 0, buffer.Length)) > 0)
            {
                for (int j = 0; j < length; j++)
                    data.Add(buffer[j]);

                if (length < buffer.Length)
                    break;
            }
            file.Close();

            return data.ToArray();
        }

        /// <summary>
        /// 保存到文件
        /// </summary>
        public static void SaveFile(string filePath, byte[] resource)
        {
            var arr = filePath.Split('/');
            var fileName = arr[arr.Length - 1];
            var dir = filePath.Remove(filePath.LastIndexOf(fileName));

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            if (File.Exists(filePath))
                File.Delete(filePath);

            FileStream fs = new FileStream(filePath, FileMode.Create);
            fs.Write(resource, 0, resource.Length);
            fs.Flush();
            fs.Close();
        }

        /// <summary>
        /// 搜索文件夹中的文件
        /// </summary>
        public static void GetAll(ArrayList FileList, DirectoryInfo dir, string dirName)
        {
            FileInfo[] allFile = dir.GetFiles();
            foreach (FileInfo fi in allFile)
            {
                FileList.Add(dirName + fi.Name);
            }

            DirectoryInfo[] allDir = dir.GetDirectories();
            foreach (DirectoryInfo d in allDir)
            {
                GetAll(FileList, d, dirName + d.Name + "/");
            }
        }

        /// <summary>  
        /// 复制文件夹中的所有文件夹与文件到另一个文件夹
        /// </summary>  
        /// <param name="sourcePath">源文件夹</param>
        /// <param name="destPath">目标文件夹</param>
        /// <param name="notCopy">不用复制的文件列表</param>
        public static void CopyDir(string sourcePath, string destPath, List<string> notCopy = null)
        {
            if (!Directory.Exists(destPath))
                Directory.CreateDirectory(destPath);

            //获得源文件下所有文件
            List<string> files = new List<string>(Directory.GetFiles(sourcePath));
            files.ForEach(c =>
            {
                if (notCopy != null)
                {
                    foreach (var file in notCopy)
                    {
                        if (file.Replace('/', '\\') == c.Replace('/', '\\'))
                            return;
                    }
                }
                string destFile = Path.Combine(new string[] { destPath, Path.GetFileName(c) });
                File.Copy(c, destFile, true); //覆盖
            });

            //获得源文件下所有目录文件
            List<string> folders = new List<string>(Directory.GetDirectories(sourcePath));
            folders.ForEach(c =>
            {
                string destDir = Path.Combine(new string[] { destPath, Path.GetFileName(c) });
                CopyDir(c, destDir); //递归
            });
        }

        public static void CreateDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        /// <summary>
        /// 将格式为zip的压缩文件解压到指定的目录
        /// </summary>
        /// <param name="rarFileName">要解压zip文件的路径</param>
        /// <param name="saveDir">解压后要保存到的目录</param>
        public static void DeCompressZip(string zipFileName, string saveDir)
        {
            CreateDirectory(saveDir);

            ZipFile.ExtractToDirectory(zipFileName, saveDir);
        }
    }
}
