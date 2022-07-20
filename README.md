# BountySystem

![GitHub release (latest by date)](https://img.shields.io/github/downloads/Heisenberg3666/BountySystem/total?style=for-the-badge)
[![GPLv3 License](https://img.shields.io/badge/License-GPL%20v3-yellow.svg?style=for-the-badge)](https://opensource.org/licenses/)

BountySystem is an SCP: SL plugin using the Exiled framework. The BountySystem uses the Transactions plugin as a base and allows players to place a bounty on other people.

## Authors

- [@Heisenberg3666](https://github.com/Heisenberg3666)

## Installation

Download BountySystem.dll from the latest release and place inside of the Plugins folder.
Note: You will need the Transactions plugin for this plugin to work.

## Support

For support, please create an issue on GitHub or message me on Discord (Heisenberg#3666).

## Features

- Provides players a way to spend their money.
- Adds difficulty to the gameplay.

## Developers

### API Examples

```csharp
using BountySystem.API;
...
Player issuer = Player.Get("Heisenberg");
Player target = Player.Get("Gabe Newell");
Player killer = Player.Get("Hubert Moszka");

Bounty bounty = new Bounty()
{
    IssuerId = issuer.Id,
    TargetId = target.Id,
    Reward = 100,
    Reason = "Because he owns Valve and I don't."
};

BountySystemApi.CreateBounty(bounty);
Bounty myBounty = BountySystemApi.Bounties.FirstOrDefault(x => x.IssuerId == issuer.Id);
...

// There are many ways to end a bounty, here are some:
BountySystemApi.CancelBounty(bounty);
BountySystemApi.CompleteBounty(bounty);
BountySystemApi.FailBounty(bounty);
```
## License

[GPLv3](https://choosealicense.com/licenses/gpl-3.0/)
