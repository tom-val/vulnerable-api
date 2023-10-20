{
  "Logging": {
    "Region": "eu-central-1",
    "LogGroup": "VulnerableAPI.Logs",
    "IncludeLogLevel": true,
    "IncludeCategory": true,
    "IncludeNewline": true,
    "IncludeException": true,
    "IncludeEventId": false,
    "IncludeScopes": false,
    "LogLevel": {
      "Default": "Debug",
      "System": "Information",
      "Microsoft": "Information"
    }
  },
  "AllowedHosts": "*",
  "Redis": {
    "Connection": "redis-18529.c300.eu-central-1-1.ec2.cloud.redislabs.com",
    "Port": 18529,
    "Username": "test",
    "EncodedPassword": "VGVzdDEyMzQh"
  }
}
