using Newtonsoft.Json;
using Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace IO
{
    public class ClassCallWebAPI
    {
        public ClassCallWebAPI()
        {

        }


        public async Task<ClassApiRates> GetRatesFromApiAsync()
        {
            try
            {
                while (true)
                {
                    string strJson = await GetURLContentsAsync("https://openexchangerates.org/api/latest.json?app_id=ab99a24a526b42089c85876ce136ccf0&base=USD");

                    JsonConvert.DeserializeObject<ClassApiRates>(strJson);
                    await Task.Delay(60000);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<string> GetURLContentsAsync(string inURL)
        {
            var content = new MemoryStream();
            
            var webReq = (HttpWebRequest)WebRequest.Create(inURL);
            try
            {
                using (WebResponse response = await webReq.GetResponseAsync())
                {
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        await responseStream.CopyToAsync(content);
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return System.Text.Encoding.UTF8.GetString(content.ToArray());

            //Windows1252
        }
    }
}
