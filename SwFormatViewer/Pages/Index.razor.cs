using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using Microsoft.JSInterop;
using SwFormatViewer;
using SwFormatViewer.Shared;
using AntDesign;
using System.Text.Json;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Text.RegularExpressions;

namespace SwFormatViewer.Pages
{
    public partial class Index
    {
        public string APIUrl { get; set; } = PathManager.ApiUrl;

        public string Url { get; set; }
        
        public bool Loading { get; set; }

        [Inject]
        public HttpClient Http { get; set; }
        
        [Inject]
        public MessageService Message { get; set; }

        [Inject]
        public IConfiguration Configuration { get; set; }

        bool BeforeUpload(UploadFileItem file)
        {
            //APIUrl = Configuration["Url"];
            var isSldFile = Regex.IsMatch(file.Ext, ".sld", RegexOptions.IgnoreCase);
            if (!isSldFile)
            {
                Message.Error($"File Format({file.Ext}) Error,Please Re-Select!");
            }

            var IsLt500K = file.Size < 1024 * 10000;
            if (!IsLt500K)
            {
                Message.Error("File Size Should Smaller than 10000KB!");
            }

            return isSldFile && IsLt500K;
        }

        async Task OnCompleted(UploadInfo fileinfo)
        {
            Loading = fileinfo.File.State == UploadState.Uploading;
            if (fileinfo.File.State == UploadState.Success)
            {
                var rsObj = JsonSerializer.Deserialize<SldResult<SldFile>>(fileinfo.File.Response)!;
                if (rsObj.code == 200)
                {
                    //Thread.Sleep(4000);
                    //await UrlChanged.InvokeAsync(rsObj.Data.PreViewUrl);
                    _ = Message.Success("Success");
                    //��ѯ
                    var url = APIUrl + rsObj.data.previewUrl;
                    _= Query(url,rsObj.data.fileId);
                }
                else
                {
                    _ = Message.Error(rsObj.msg);
                }
            }

            if (fileinfo.File.State == UploadState.Fail)
            {
                _ = Message.Error("Failed...");
            }

            await Task.Delay(2000);
            Message.Destroy();
        }

        async Task Query(string url, string id)
        {
            System.Timers.Timer t = new System.Timers.Timer();
            int count = 0;
            t.Elapsed += async (s, e) =>
            {
                count++;
                if (count > 10)
                {
                    t.Stop();
                    t.Dispose();
                    Loading = false;
                    _ = Message.Error("Time out");
                    await InvokeAsync(StateHasChanged);
                }

                var res  = await Http.GetFromJsonAsync<SldResult<bool>>($"?id={id}");
                if (res != null && res.data)
                {
                    Url = url;
                    t.Stop();
                    t.Dispose();
                    Loading=false;
                    await InvokeAsync(StateHasChanged);
                }
            };
            t.Interval = 2000;
            t.Start();
        }
    }
}