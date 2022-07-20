using BountySystem.API.Entities;
using Exiled.API.Features;
using System.Collections.Generic;
using Transactions.API;

namespace BountySystem.API
{
    public static class BountySystemApi
    {
        private const int _duration = 10;

        public static List<Bounty> Bounties = new List<Bounty>();

        /// <summary>
        /// Adds a bounty to the list and removes Money from the issuer.
        /// </summary>
        /// <param name="newBounty"></param>
        public static void CreateBounty(Bounty newBounty)
        {
            Player issuer = Player.Get(newBounty.IssuerId);
            Player target = Player.Get(newBounty.TargetId);

            string creationMessage = BountySystem.Instance.Config.BountyMessages.Creation
                .Replace("%issuer%", issuer.Nickname)
                .Replace("%target%", target.Nickname)
                .Replace("%money%", TransactionsApi.FormatMoney(newBounty.Reward))
                .Replace("%reason%", newBounty.Reason);

            Map.Broadcast(_duration, creationMessage);

            TransactionsApi.RemoveMoney(issuer, newBounty.Reward);

            Bounties.Add(newBounty);

            target.CustomInfo = "<color=#C50000>BOUNTY</color>";
        }

        /// <summary>
        /// Removes the bounty from the list and refunds the issuer their money.
        /// </summary>
        /// <param name="bounty"></param>
        public static void CancelBounty(Bounty bounty)
        {
            Player issuer = Player.Get(bounty.IssuerId);
            Player target = Player.Get(bounty.TargetId);

            string cancellationMessage = BountySystem.Instance.Config.BountyMessages.Cancellation
                .Replace("%issuer%", issuer.Nickname)
                .Replace("%target%", target.Nickname)
                .Replace("%money%", TransactionsApi.FormatMoney(bounty.Reward))
                .Replace("%reason%", bounty.Reason);

            Map.Broadcast(_duration, cancellationMessage);

            TransactionsApi.AddMoney(issuer, bounty.Reward);

            Bounties.Remove(bounty);

            target.CustomInfo = "";
        }

        /// <summary>
        /// Removes the bounty from the list, marks it as complete and give the killer their reward.
        /// </summary>
        /// <param name="bounty"></param>
        /// <param name="killer"></param>
        public static void CompleteBounty(Bounty bounty, Player killer)
        {
            Player issuer = Player.Get(bounty.IssuerId);
            Player target = Player.Get(bounty.TargetId);

            string completionMessage = BountySystem.Instance.Config.BountyMessages.Completion
                .Replace("%issuer%", issuer.Nickname)
                .Replace("%target%", target.Nickname)
                .Replace("%killer%", killer.Nickname)
                .Replace("%money%", TransactionsApi.FormatMoney(bounty.Reward))
                .Replace("%reason%", bounty.Reason);

            string rewardMessage = BountySystem.Instance.Config.BountyMessages.Collection
                .Replace("%issuer%", issuer.Nickname)
                .Replace("%target%", target.Nickname)
                .Replace("%killer%", killer.Nickname)
                .Replace("%money%", TransactionsApi.FormatMoney(bounty.Reward))
                .Replace("%reason%", bounty.Reason);

            Map.Broadcast(_duration, completionMessage);
            killer.ShowHint(rewardMessage, _duration);

            TransactionsApi.AddMoney(killer, bounty.Reward);

            Bounties.Remove(bounty);
        }

        /// <summary>
        /// Removes the bounty from the list, marks it as failed and refunds the issuer their money.
        /// </summary>
        /// <param name="bounty"></param>
        public static void FailBounty(Bounty bounty)
        {
            Player issuer = Player.Get(bounty.IssuerId);
            Player target = Player.Get(bounty.TargetId);

            string failureMessage = BountySystem.Instance.Config.BountyMessages.Failure
                .Replace("%issuer%", issuer.Nickname)
                .Replace("%target%", target.Nickname)
                .Replace("%money%", TransactionsApi.FormatMoney(bounty.Reward))
                .Replace("%reason%", bounty.Reason);

            Map.Broadcast(_duration, failureMessage);

            TransactionsApi.AddMoney(issuer, bounty.Reward);

            Bounties.Remove(bounty);
        }
    }
}
