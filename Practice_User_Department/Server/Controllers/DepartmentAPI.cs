using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Common;
using Server.Models;
using System.Net;

namespace Server.Controllers
{
    [Route("department")]
    [ApiController]
    public class DepartmentAPI : ControllerBase
    {
        private readonly PracticePrn231Context _context;
        public DepartmentAPI(PracticePrn231Context context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<BaseResponse<object>>> GetListDepartment()
        {
            BaseResponse<object> response = new BaseResponse<object>();
            try
            {
                var listDepartment = await _context.Departments
                    .Select(d => new
                    {
                        d.Id,
                        d.Name
                    })
                    .ToListAsync();

                response.Error = false;
                response.Code = HttpStatusCode.OK;
                response.Message = "Lấy danh sách Department thành công";
                response.Data = listDepartment;
                return StatusCode((int)response.Code, response);
            }
            catch (Exception ex)
            {
                response.Error = true;
                response.Code = HttpStatusCode.InternalServerError;
                response.Message = "Có lỗi xảy ra. Vui lòng thử lại sau!";
                return StatusCode((int)response.Code, response);
            }
        }

    }
}
