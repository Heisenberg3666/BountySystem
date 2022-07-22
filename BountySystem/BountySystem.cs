using BountySystem.Commands;
using BountySystem.Config;
using BountySystem.Events;
using ConsentManager.API;
using ConsentManager.API.Entities;
using Exiled.API.Features;
using MEC;
using System;
using System.Collections.Generic;
using Transactions.API;
using Transactions.API.Interfaces;

namespace BountySystem
{
    public class BountySystem : Plugin<BaseConfig>
    {
        private PlayerEvents _playerEvents;
        private ServerEvents _serverEvents;
        private List<IUsageCommand> _commands;
        private CoroutineHandle _commandChecker;
        private Guid _apiKey;

        public static BountySystem Instance;

        public override string Name { get; } = "BountySystem";
        public override string Author { get; } = "Heisenberg3666";
        public override Version Version { get; } = new Version(1, 1, 0, 0);
        public override Version RequiredExiledVersion { get; } = new Version(5, 2, 2);

        public override void OnEnabled()
        {
            Instance = this;

            _playerEvents = new PlayerEvents();
            _serverEvents = new ServerEvents();

            RegisterEvents();

            _commands = new List<IUsageCommand>()
            {
                new CreateBounty(),
                new CancelBounty(),
                new SeeBounties()
            };

            _commandChecker = Timing.CallPeriodically(60f, 5f, () =>
            {
                if (Transactions.Transactions.Instance != null)
                {
                    TransactionsApi.RegisterSubcommand(_commands[0]);
                    TransactionsApi.RegisterSubcommand(_commands[1]);
                    TransactionsApi.RegisterSubcommand(_commands[2]);
                    _commandChecker.IsRunning = false;
                }
            });

            _apiKey = PluginRegistration.Register(new PluginUsage()
            {
                Name = Name,
                Version = Version,
                DataUsage = "BountySystem will use player's data to allow the player to place/collect a bounty.",
                WhoCanSeeData = "Players in the server will be able to see if you have placed a bounty and how much it is, " +
                "players may also see that information when the bounty has been collected."
            });

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            PluginRegistration.Unregister(_apiKey);
            _apiKey = Guid.Empty;

            TransactionsApi.UnregisterSubcommand(_commands[0]);
            TransactionsApi.UnregisterSubcommand(_commands[1]);
            TransactionsApi.UnregisterSubcommand(_commands[2]);

            _commands = null;

            UnregisterEvents();

            _serverEvents = null;
            _playerEvents = null;

            Instance = null;

            base.OnDisabled();
        }

        public void RegisterEvents()
        {
            _playerEvents.RegisterEvents();
            _serverEvents.RegisterEvents();
        }

        public void UnregisterEvents()
        {
            _playerEvents.UnregisterEvents();
            _serverEvents.UnregisterEvents();
        }
    }
}
