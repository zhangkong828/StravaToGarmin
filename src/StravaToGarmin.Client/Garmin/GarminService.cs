using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StravaToGarmin.Client.Garmin
{
    public class GarminService
    {
        private readonly GarminConnectClient.Lib.Services.Client _client;
        public GarminService()
        {
            _client = new GarminConnectClient.Lib.Services.Client(null, new GarminConsoleLogger<GarminConnectClient.Lib.Services.Client>());
        }

        /// <summary>
        /// 登录认证
        /// </summary>
        /// <returns></returns>
        public async Task<bool> Authenticate()
        {
            await _client.Authenticate(Configure.Instance.GarminUsername, Configure.Instance.GarminPassword);
            return _client.IsAuthenticated;
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public async Task<bool> Upload(string filePath)
        {
            try
            {
                var (success, activityId) = await _client.UploadActivity(filePath, new GarminConnectClient.Lib.Dto.FileFormat() { FormatKey = "fit" });
                return success;
            }
            catch (Exception ex)
            {
                /*
                 新版本变动 这里会匹配失败
                 第一次请求：https://connect.garmin.com/upload-service/upload/.fit
                 第一次响应：
                    {
                        "detailedImportResult": {
                            "uploadId": 254577255734,
                            "uploadUuid": {
                                "uuid": "f6719eb4-80e8-4608-a927-bfb105665cb3"
                            },
                            "owner": 114458679,
                            "fileSize": 134903,
                            "processingTime": 140,
                            "creationDate": "2024-05-20 09:52:41.830 GMT",
                            "ipAddress": null,
                            "fileName": "11424322350.fit",
                            "report": null,
                            "successes": [],
                            "failures": []
                        }
                    }

                第二次请求：https://connect.garmin.com/activity-service/activity/status/1716198761830/f6719eb480e84608a927bfb105665cb3
                第二次响应：
                    {
                        "detailedImportResult": {
                            "uploadId": 254577255734,
                            "uploadUuid": {
                                "uuid": "f6719eb4-80e8-4608-a927-bfb105665cb3"
                            },
                            "owner": 114458679,
                            "fileSize": "",
                            "processingTime": "",
                            "creationDate": "2024-05-20 09:52:41.830 GMT",
                            "ipAddress": null,
                            "fileName": null,
                            "report": null,
                            "successes": [
                                {
                                    "internalId": 15485239979,
                                    "externalId": null,
                                    "messages": null
                                }
                            ],
                            "failures": []
                        }
                    }
                 */

                //通过判断第一次响应结果 简单判断 失败也无所谓
                if (ex.Message.Contains("fileName") && ex.Message.Contains("uuid"))
                {
                    Console.WriteLine($"[上传Garmin成功]");
                    return true;
                }
            }

            return false;
        }
    }
}
