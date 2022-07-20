using BountySystem.API;
using BountySystem.API.Entities;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using System;
using System.Linq;
using Transactions.API;
using Transactions.API.Interfaces;

namespace BountySystem.Commands
{
    internal class CancelBounty : IUsageCommand
    {
        public string Command => nameof(CancelBounty).ToLower();
        public string[] Aliases => new string[] { "cancel" };
        public string Description => "This command can be used to cancel any bounty that you have made.";
        public string[] Usage => new string[] { "Player" };

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            string permission = $"{nameof(Transactions)}.{nameof(BountySystem)}.{Command}";

            if (!sender.CheckPermission(permission))
            {
                response = $"You do not have the correct permission to run this command ({permission}).";
                return false;
            }

            if (arguments.Count < 1)
            {
                response = $"Usage: {Command} {string.Join(" ", Usage)}";
                return false;
            }

            if (!TransactionsApi.PlayerExists(Player.Get(sender)))
            {
                response = "You are not registered in the database, you must have DNT enabled.";
                return false;
            }

            Player player = Player.Get(arguments.At(0));

            if (player == null)
            {
                response = "You have not specified a valid player.";
                return false;
            }

            Player commandSender = Player.Get(sender);

            Bounty bounty = BountySystemApi.Bounties.FirstOrDefault(
                x => x.IssuerId == commandSender.Id
                && x.TargetId == player.Id);

            if (bounty == null)
            {
                response = $"You have not placed a bounty on {player.Nickname}'s head.";
                return false;
            }

            BountySystemApi.CancelBounty(bounty);
            player.ShowHint($"<color=green>+{TransactionsApi.FormatMoney(bounty.Reward)}</color>", 10);

            response = $"You have removed the bounty from {player.Nickname}'s head.";
            return true;
        }
    }
}
