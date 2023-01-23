using System;

[Serializable]
public class RequestAccessToken {

    public string grantType;
    public string clientId;
    public string clientSecret;

    public RequestAccessToken(string grantType, string clientId, string clientSecret) {
        this.grantType = grantType;
        this.clientId = clientId;
        this.clientSecret = clientSecret;
    }

}