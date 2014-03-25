string key = "<Your App key>";

// a generic method that can be used to make GET requests with REST.
private void GetResponse(Uri uri, Action<Response> callback)
{
    WebClient wc = new WebClient();
    wc.OpenReadCompleted += (o, a) =>
    {
        if (callback != null)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Response));
            callback(ser.ReadObject(a.Result) as Response);
        }
    };
    wc.OpenReadAsync(uri);
}

//A generic method that can be used to make HTTP POST requests to REST that support HTTP POST protocol.
private void GetPOSTResponse(Uri uri, string data, Action<Response> callback)
{
    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);

    request.Method = "POST";
    request.ContentType = "text/plain;charset=utf-8";

    //calculate data's length 
    System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
    byte[] bytes = encoding.GetBytes(data);

    request.ContentLength = bytes.Length;

    using (Stream requestStream = request.GetRequestStream())
    {
        // Send the data.
        requestStream.Write(bytes, 0, bytes.Length);
    }

    request.BeginGetResponse((x) =>
    {
        using (HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(x))
        {
            if (callback != null)
            {
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Response));
                callback(ser.ReadObject(response.GetResponseStream()) as Response);
            }
        }
    }, null);
}

// example to call ACS API by get way 
Uri Request = new Uri(string.Format("https://api.cloud.appcelerator.com/v1/users/show/me.json?key={0}", key));

GetResponse(Request, (x) =>
{
    Console.WriteLine(x.ResourceSets[0].Resources.Length + " result(s) found.");
    Console.ReadLine();
});
