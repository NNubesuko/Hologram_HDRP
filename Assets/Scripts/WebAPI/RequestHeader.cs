public class RequestHeader {

    public string Name { get; private set; }
    public string Value { get; private set; }

    public RequestHeader(string name, string value) {
        Name = name;
        Value = value;
    }

}