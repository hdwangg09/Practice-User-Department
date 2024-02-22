using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Common;
using Server.Common.Request;
using Server.Common.Response;
using Server.Models;
using System.Linq;
using System.Net;

namespace Server.Controllers
{
    [Route("user")]
    [ApiController]
    public class UserAPI : ControllerBase
    {
        private readonly PracticePrn231Context _context;
        public UserAPI(PracticePrn231Context context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<BaseResponse<object>>> GetListUser(string? userName)
        {
            BaseResponse<object> response = null;
            try
            {
                response = new BaseResponse<object>();
                var listUser = _context.Users
                                   .Include(u => u.Department)
                                   .Select(u => new
                                   {
                                       u.Id,
                                       u.Username,
                                       u.Dob,
                                       u.Gender,
                                       userDepartment = u.Department.Name
                                   });
                if (!string.IsNullOrEmpty(userName))
                {
                    listUser = listUser.Where(u => u.Username.Contains(userName));

                }
                response.Error = false;
                response.Code = HttpStatusCode.OK;
                response.Message = "Lấy danh sách User thành công";
                response.Data = await listUser.ToListAsync();
                return StatusCode((int)response.Code.GetValueOrDefault(), response);

            }
            catch (Exception ex)
            {
                response.Error = true;
                response.Code = HttpStatusCode.InternalServerError;
                response.Message = "Có lỗi xảy ra. Vui lòng thử lại sau!";
                return StatusCode((int)response.Code.GetValueOrDefault(), response);
            }
        }

        [HttpGet("details/{userId}")]
        public async Task<ActionResult<BaseResponse<object>>> GetUserById(int userId)
        {
            BaseResponse<object?> response = null;
            try
            {
                response = new BaseResponse<object?>();

                if (userId >= 0)
                {
                    var user = _context.Users
                                        .Include(u => u.Department)
                                        .Select(u => new
                                        {
                                            u.Id,
                                            u.Username,
                                            u.Dob,
                                            u.Gender,
                                            userDepartment = u.Department.Name
                                        })
                                        .Where(u => u.Id == userId);
                    if (user.Count() > 0)
                    {
                        response.Error = false;
                        response.Code = HttpStatusCode.OK;
                        response.Message = "Lấy User thành công!";
                        response.Data = user;
                        return StatusCode((int)response.Code.GetValueOrDefault(), response);
                    }
                    else
                    {
                        response.Error = true;
                        response.Code = HttpStatusCode.BadRequest;
                        response.Message = "User không tồn tại!";
                        return StatusCode((int)response.Code.GetValueOrDefault(), response);
                    }

                }
                else
                {
                    response.Error = true;
                    response.Code = HttpStatusCode.BadRequest;
                    response.Message = "Có lỗi xảy ra. Vui lòng thử lại sau!";
                    return StatusCode((int)response.Code.GetValueOrDefault(), response);
                }

            }
            catch (Exception ex)
            {
                response.Error = true;
                response.Code = HttpStatusCode.InternalServerError;
                response.Message = "Có lỗi xảy ra. Vui lòng thử lại sau!";
                return StatusCode((int)response.Code.GetValueOrDefault(), response);
            }

        }

        [HttpPost("update")]
        public async Task<ActionResult<BaseResponse<object>>> UpdateUserInfor([FromBody] UpdateUserRequest userRequest)
        {
            BaseResponse<object> response = new BaseResponse<object>();

            try
            {
                if (userRequest != null && userRequest.Id >= 0)
                {
                    var user = await _context.Users.FindAsync(userRequest.Id);

                    if (user != null)
                    {
                        // Cập nhật thông tin người dùng
                        user.Username = userRequest.Username;
                        user.Dob = userRequest.Dob;
                        user.Gender = userRequest.Gender;
                        user.Address = userRequest.Address;

                        _context.Users.Update(user);
                        await _context.SaveChangesAsync();

                        response.Error = false;
                        response.Code = HttpStatusCode.OK;
                        response.Message = "Cập nhật thông tin người dùng thành công!";

                        return StatusCode((int)response.Code, response);
                    }
                    else
                    {
                        response.Error = true;
                        response.Code = HttpStatusCode.NotFound;
                        response.Message = "Không tìm thấy người dùng để cập nhật";
                        return StatusCode((int)response.Code, response);
                    }
                }
                else
                {
                    response.Error = true;
                    response.Code = HttpStatusCode.BadRequest;
                    response.Message = "Yêu cầu không hợp lệ";
                    return StatusCode((int)response.Code, response);
                }
            }
            catch (Exception ex)
            {
                response.Error = true;
                response.Code = HttpStatusCode.InternalServerError;
                response.Message = "Có lỗi xảy ra. Vui lòng thử lại sau";
                return StatusCode((int)response.Code, response);
            }
        }
        [HttpPost("add")]
        public async Task<ActionResult<BaseResponse<object>>> AddUser([FromBody] User userRequest)
        {
            BaseResponse<object> response = new BaseResponse<object>();

            try
            {
                if (userRequest != null)
                {
                    _context.Users.Add(userRequest);
                    await _context.SaveChangesAsync();

                    response.Error = false;
                    response.Code = HttpStatusCode.OK;
                    response.Message = "Thêm người dùng thành công!";

                    return StatusCode((int)response.Code, response);
                }
                else
                {
                    response.Error = true;
                    response.Code = HttpStatusCode.BadRequest;
                    response.Message = "Yêu cầu không hợp lệ";
                    return StatusCode((int)response.Code, response);
                }
            }
            catch (Exception ex)
            {
                response.Error = true;
                response.Code = HttpStatusCode.InternalServerError;
                response.Message = "Có lỗi xảy ra. Vui lòng thử lại sau";
                return StatusCode((int)response.Code, response);
            }
        }

        [HttpPost("delete/{userId}")]
        public async Task<ActionResult<BaseResponse<object>>> DeleteUser(int userId)
        {
            BaseResponse<object?> response = null;
            try
            {
                response = new BaseResponse<object?>();

                if (userId >= 0)
                {
                    var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
                    if (user != null)
                    {
                        _context.Users.Remove(user);
                        _context.SaveChangesAsync();

                        response.Error = false;
                        response.Code = HttpStatusCode.OK;
                        response.Message = "Xoá người dùng thành công!";
                        return StatusCode((int)response.Code.GetValueOrDefault(), response);
                    }
                    else
                    {
                        response.Error = true;
                        response.Code = HttpStatusCode.BadRequest;
                        response.Message = "User không tồn tại!";
                        return StatusCode((int)response.Code.GetValueOrDefault(), response);
                    }

                }
                else
                {
                    response.Error = true;
                    response.Code = HttpStatusCode.BadRequest;
                    response.Message = "Có lỗi xảy ra. Vui lòng thử lại sau!";
                    return StatusCode((int)response.Code.GetValueOrDefault(), response);
                }

            }
            catch (Exception ex)
            {
                response.Error = true;
                response.Code = HttpStatusCode.InternalServerError;
                response.Message = "Có lỗi xảy ra. Vui lòng thử lại sau!";
                return StatusCode((int)response.Code.GetValueOrDefault(), response);
            }
        }

    }
}
