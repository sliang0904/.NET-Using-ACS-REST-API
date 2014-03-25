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
