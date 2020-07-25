﻿namespace Watchman.DomainModel.Settings.ConfigurationItems
{
    public class HowLongIsShortTimeInSeconds : MappedConfiguration<int>
    {
        public override int Value { get; set; } = 15;

        public HowLongIsShortTimeInSeconds(ulong serverId) : base(serverId)
        {
        }
    }
}
