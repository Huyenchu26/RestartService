using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Restart.Controllers
{
    public class ParseFileController : ApiController
    {
        // GET api/ParseFile/{id}
        public string Get(int id)
        {
            return "Chu Huyen";
        }


        // GET api/ParseFile
        // return lastest record
        [System.Web.Http.HttpGet]
        public List<Dictionary<string, Dictionary<string, string>>> parse_file()
        {
            List<Dictionary<string, Dictionary<string, string>>> listJson = new List<Dictionary<string, Dictionary<string, string>>>();
            string[] listFile = LimitData.ProcessDirectory(Config.PathFile);
            for (int i = 0; i < listFile.Length; i++)
            {
                List<Dictionary<string, Dictionary<string, string>>> file_data = new List<Dictionary<string, Dictionary<string, string>>>();
                var filestream = new System.IO.FileStream(Config.PathFile + listFile[i], System.IO.FileMode.Open);
                var file = new System.IO.StreamReader(filestream);
                string line_of_text;
                do
                // chua handle error
                {
                    Dictionary<string, Dictionary<string, string>> record = new Dictionary<string, Dictionary<string, string>>();
                    line_of_text = file.ReadLine();
                    if (line_of_text == null || line_of_text.Trim('\n', '\r').Length == 0)
                        break;
                    record["timestamp_recv"] = LimitData.extract_data_begin(line_of_text);
                    line_of_text = file.ReadLine();
                    if (line_of_text == null || line_of_text.Trim('\n', '\r').Length == 0)
                        break;
                    record["data"] = LimitData.extract_data(line_of_text);
                    file_data.Add(record);
                } while (true);
                filestream.Close();

                listJson.Add(file_data.Last());
            }

            return listJson;
        }


        //input khoảng thời gian
        //public List<String> getHistory(String imei, String startDate, String endDate)
        //{
        //    List<String> listJson = new List<string>();
        //    String[] imeiArray = LimitData.ProcessDirectory(Config.PathFile);
        //    for (int i = 0; i < imeiArray.Length; i++)
        //    {
        //        if (imeiArray[i].Split('.')[0].Equals(imei))
        //        {

        //            List<Dictionary<string, Dictionary<string, string>>> file_data = new List<Dictionary<string, Dictionary<string, string>>>();
        //            var filestream = new System.IO.FileStream(Config.PathFile + imeiArray[i], System.IO.FileMode.Open);
        //            var file = new System.IO.StreamReader(filestream);
        //            string line_of_text;
        //            do
        //            // chua handle error
        //            {
        //                Dictionary<string, Dictionary<string, string>> record = new Dictionary<string, Dictionary<string, string>>();
        //                line_of_text = file.ReadLine();
        //                if (line_of_text == null || line_of_text.Trim('\n', '\r').Length == 0)
        //                    break;
        //                record["timestamp_recv"] = LimitData.extract_data_begin(line_of_text);
        //                line_of_text = file.ReadLine();
        //                if (line_of_text == null || line_of_text.Trim('\n', '\r').Length == 0)
        //                    break;
        //                record["data"] = LimitData.extract_data(line_of_text);

        //                DateTime curDate = LimitData.stringToDate(line_of_text.Split(',')[1]);

        //                if (LimitData.DateToTimestamp(curDate, LimitData.stringToDate(startDate)) >= 0
        //                    && LimitData.DateToTimestamp(LimitData.stringToDate(endDate), curDate) >= 0)
        //                {
        //                    file_data.Add(record);
        //                }


        //            } while (true);
        //            filestream.Close();

        //            listJson.Add(JsonConvert.SerializeObject(file_data));
        //        }
        //    }
        //    return listJson;
        //}


        [HttpGet]
        public List<Dictionary<string, Dictionary<string, string>>> getHistory(String imei, String startDate, String endDate)
        {
            List<Dictionary<string, Dictionary<string, string>>> file_data = new List<Dictionary<string, Dictionary<string, string>>>();
            String[] imeiArray = LimitData.ProcessDirectory(Config.PathFile);
            for (int i = 0; i < imeiArray.Length; i++)
            {
                bool bo = imeiArray[i].Split('.')[0].Equals(imei);
                if (bo)
                {
                    System.Diagnostics.Debug.WriteLine("Huyenchu imeiArray: " + imeiArray[i].ToString()
                    + " " + bo.ToString() + " " + imeiArray[i].Split('.')[0]);
                    //List<Dictionary<string, Dictionary<string, string>>> file_data = new List<Dictionary<string, Dictionary<string, string>>>();
                    var filestream = new System.IO.FileStream(Config.PathFile + imeiArray[i], System.IO.FileMode.Open);
                    var file = new System.IO.StreamReader(filestream);
                    string line_of_text;
                    do
                    // chua handle error
                    {
                        Dictionary<string, Dictionary<string, string>> record = new Dictionary<string, Dictionary<string, string>>();
                        line_of_text = file.ReadLine();
                        if (line_of_text == null || line_of_text.Trim('\n', '\r').Length == 0)
                            break;
                        record["timestamp_recv"] = LimitData.extract_data_begin(line_of_text);
                        line_of_text = file.ReadLine();
                        if (line_of_text == null || line_of_text.Trim('\n', '\r').Length == 0)
                            break;
                        record["data"] = LimitData.extract_data(line_of_text);

                        DateTime curDate = LimitData.stringToDate(line_of_text.Split(',')[1]);

                        if (LimitData.DateToTimestamp(curDate, LimitData.stringToDate(startDate)) >= 0
                            && LimitData.DateToTimestamp(LimitData.stringToDate(endDate), curDate) >= 0)
                        {
                            file_data.Add(record);
                        }

                    } while (true);
                    filestream.Close();

                } else System.Diagnostics.Debug.WriteLine("Huyenchu imeiArray: " + imeiArray[i].ToString() 
                    + " " + bo.ToString() + " " + imeiArray[i].Split('.')[0]);
            }
            return file_data;
        }

    }


}
