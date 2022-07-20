using BountySystem.API;
using BountySystem.API.Entities;
using Exiled.Events.EventArgs;
using Exiled.Events.Handlers;

namespace BountySystem.Events
{
    internal class ServerEvents
    {
        public void RegisterEvents()
        {
            Server.EndingRound += OnRoundEnded;
        }

        public void UnregisterEvents()
        {
            Server.EndingRound -= OnRoundEnded;
        }

        private void OnRoundEnded(EndingRoundEventArgs e)
        {
            if (BountySystemApi.Bounties.Count > 0)
            {
                foreach (Bounty bounty in BountySystemApi.Bounties)
                    BountySystemApi.FailBounty(bounty);
            }
        }
    }
}
