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
    internal class SeeBounties : IUsageCommand
    {
        public string Command { get; } = nameof(SeeBounties).ToLower();
        public string[] Aliases { get; } = new string[] { "sb" };
        public string Description { get; } = "Use this command to see all the bounties that currently exist.";
        public string[] Usage { get; } = Array.Empty<string>();

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            string permission = $"{nameof(Transactions)}.{nameof(BountySystem)}.{Command}";

            if (!sender.CheckPermission(permission))
            {
                response = $"You do not have the correct permission to run this command ({permission}).";
                return false;
            }

            if (BountySystemApi.Bounties.Count > 0)
            {
                response = "These are all the bounties that currently exist:" +
                    "\nIssuer       Target      Reward      Reason<color=red>";

                foreach (Bounty bounty in BountySystemApi.Bounties)
                {
                    Player issuer = Player.Get(bounty.IssuerId);
                    Player target = Player.Get(bounty.TargetId);

                    response += $"\n{issuer.Nickname}   {target.Nickname}   {TransactionsApi.FormatPoints(bounty.Reward)}   {bounty.Reason}";
                }

                response += "</color>";
            }
            else
            {
                response = "There are no active bounties.";
            }

            return true;
        }
    }
}
