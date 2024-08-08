using System;

using UnityEngine;

public class ResponseHandler
{
    public void HandleResponse(HttpResponse response)
    {
        if (response.IsSuccess)
        {
            response.Request.OnSuccess?.Invoke(response);
        }
        else
        {
            HandleError(response.Error);
        }
    }

    public void HandleError(Exception error)
    {
        Debug.LogError("HTTP Error: " + error.Message);
    }
}
