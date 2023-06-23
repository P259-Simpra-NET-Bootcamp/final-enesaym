using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SimShop.Base;


public partial class ApiResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public T Data { get; set; }

    public ApiResponse(bool isSuccess)
    {
        Success = isSuccess;
        Data = default;
        Message = isSuccess ? "Success" : "Error";
    }
    public ApiResponse(T data)
    {
        Success = true;
        Data = data;
        Message = "Success";
    }
    public ApiResponse(string message)
    {
        Success = false;
        Data = default;
        Message = message;
    }
}
public partial class ApiResponse
{

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }

    public ApiResponse(string message = null)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            Success = true;
        }
        else
        {
            Success = false;
            Message = message;
        }
    }
    public bool Success { get; set; }
    public string Message { get; set; }
}

