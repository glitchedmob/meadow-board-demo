# Meadow Board Demo

[Video Demo](https://youtu.be/Z0F0tp4QNok)

## Prerequisites

- [Meadow F7v2 Feather](https://store.wildernesslabs.co/collections/frontpage/products/meadow-f7-feather)
  - With [Meadow OS installed](http://developer.wildernesslabs.co/Meadow/Getting_Started/Deploying_Meadow/)
- Jumper wires and push button
  - Push button should be wired to ground and D10
- [.NET 6](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
- [Meadow CLI](http://developer.wildernesslabs.co/Meadow/Meadow_Basics/Meadow_CLI/)


## Setup
 - Copy `app.config.yaml.example` to `app.config.yaml`
   - Make sure to fill out missing values. You will need a [Twilio](https://www.twilio.com/) account to fill in the Twilio values
 - `cd MeadowBoardDemo`
 - `dotnet build`
 - `meadow app deploy --file bin/Debug/netstandard2.1/App.dll && meadow listen`
