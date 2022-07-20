using BountySystem.API;
using BountySystem.API.Entities;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using System.Collections.Generic;
using System.Linq;
using Transactions.API;

namespace BountySystem.Events
{
    internal class PlayerEvents
    {
        public void RegisterEvents()
        {
            Exiled.Events.Handlers.Player.Dying += OnDying;
            Exiled.Events.Handlers.Player.Left += OnLeft;
        }

        public void UnregisterEvents()
        {
            Exiled.Events.Handlers.Player.Dying -= OnDying;
            Exiled.Events.Handlers.Player.Left -= OnLeft;
        }

        private void OnDying(DyingEventArgs e)
        {
            Bounty bounty = BountySystemApi.Bounties.FirstOrDefault(x => x.TargetId == e.Target.Id);
            IEnumerable<Bounty> issuedBounties = BountySystemApi.Bounties.Where(x => x.IssuerId == e.Target.Id);

            if (bounty != null)
            {
                if (e.Killer != null && !e.Killer.DoNotTrack)
                {
                    BountySystemApi.CompleteBounty(bounty, e.Killer);
                    e.Killer.ShowHint($"<color=green>+{TransactionsApi.FormatPoints(bounty.Reward)}</color>", 10);
                }
                else
                {
                    BountySystemApi.FailBounty(bounty);
                    Player.Get(bounty.IssuerId).ShowHint($"<color=green>+{TransactionsApi.FormatPoints(bounty.Reward)}</color>", 10);
                }
            }

            if (issuedBounties != null)
            {
                foreach (Bounty issuedBounty in issuedBounties)
                {
                    BountySystemApi.CancelBounty(bounty);
                    Player.Get(bounty.IssuerId).ShowHint($"<color=green>+{TransactionsApi.FormatPoints(bounty.Reward)}</color>", 10);
                }
            }
        }

        private void OnLeft(LeftEventArgs e)
        {
            IEnumerable<Bounty> issuedBounties = BountySystemApi.Bounties.Where(x => x.IssuerId == e.Player.Id).ToList();
            IEnumerable<Bounty> bounties = BountySystemApi.Bounties.Where(x => x.TargetId == e.Player.Id).ToList();

            if (bounties != null)
            {
                foreach (Bounty bounty in bounties)
                {
                    BountySystemApi.FailBounty(bounty);
                    Player.Get(bounty.IssuerId).ShowHint($"<color=green>+{TransactionsApi.FormatPoints(bounty.Reward)}</color>", 10);
                }
            }

            if (issuedBounties != null)
            {
                foreach (Bounty issuedBounty in issuedBounties)
                {
                    BountySystemApi.CancelBounty(issuedBounty);
                    Player.Get(issuedBounty.IssuerId).ShowHint($"<color=green>+{TransactionsApi.FormatPoints(issuedBounty.Reward)}</color>", 10);
                }
            }
        }
    }
}
