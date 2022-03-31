using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SwFormat.Entity;

namespace SwFormatConverter.Controllers
{
    //TODO save as

    [Route("api/[controller]")]
    [ApiController]
    public class SldFileSaveAsController : ControllerBase
    {
        public async Task<SldResult<string>> SaveAs(SldFormat format)
        {
            if (!ExistItems.Exist(format.Id))
            {
                return new SldResult<string>()
                {
                    Code = 404,
                    Msg = "Not Found File"
                };
            }

            //转换格式

            return new SldResult<string>()
            {

            };
        }
    }
}
